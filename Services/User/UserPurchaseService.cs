using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.DTOs.Mapping;
using Unstore.Models;

namespace Unstore.Services;

public class UserPurchaseService : BaseService
{
    private readonly IServiceResultFactory _serviceResultFactory;
    public UserPurchaseService(AppDbContext dbContext, IServiceResultFactory serviceResultFactory) : base(dbContext)
    {
        _serviceResultFactory = serviceResultFactory;
    }

    private async Task<IServiceResult<decimal>> CalculateProductPurchaseValueAsync(ICollection<ProductPurchaseCreateDto> productPurchasesDtos)
    {
        var productIds = productPurchasesDtos.Select(y => y.ProductId).ToList();
        
        Dictionary<int, decimal> productsIdValues = await Context.Products
            .AsNoTracking()
            .Where(product => productIds.Contains(product.Id))
            .ToDictionaryAsync(product => product.Id, product => product.Value);
        
        if (productsIdValues.Count < productPurchasesDtos.Count)
            return _serviceResultFactory.Failure<decimal>(OperationStatus.InvalidInput);
        
        decimal totalValue = 0m;
        
        foreach (var productPurchaseDto in productPurchasesDtos)
            totalValue += productPurchaseDto.Quantity * productsIdValues[productPurchaseDto.ProductId];
        
        return _serviceResultFactory.Success(totalValue);
    }

    public async Task<IServiceResult<PurchaseReadDto>> AddPurchaseAsync(string? username,
        int userAddressId,
        ICollection<ProductPurchaseCreateDto> productPurchasesDtos)
    {
        if (string.IsNullOrWhiteSpace(username))
            return _serviceResultFactory.Failure<PurchaseReadDto>(OperationStatus.InvalidInput);

        var user = await Context.Users
            .AsNoTracking()
            .Include(x => x.Addresses)
            .FirstOrDefaultAsync(x => x.Username == username);
        
        if (user == null)
            return _serviceResultFactory.Failure<PurchaseReadDto>(OperationStatus.InvalidInput);
        
        if (!user.Addresses.Any(address => address.Id == userAddressId))
            return _serviceResultFactory.Failure<PurchaseReadDto>(OperationStatus.InvalidInput);
        
        var totalValueResult = await CalculateProductPurchaseValueAsync(productPurchasesDtos);
        
        if (!totalValueResult.Ok)
            return _serviceResultFactory.Failure<PurchaseReadDto>(OperationStatus.InvalidInput);
        
        decimal totalValue = totalValueResult.Data;

        ICollection<ProductPurchase> productPurchases = productPurchasesDtos.MapToModels();
        
        Purchase purchase = new Purchase
        {
            TotalValue = totalValue,
            BoughtDate = DateTime.UtcNow,
            UserId = user.Id,
            AddressId =  userAddressId,
            ProductPurchases = productPurchases
        };

        await Context.Purchases.AddAsync(purchase);
        await Context.SaveChangesAsync();
        
        return _serviceResultFactory.Success(purchase.MapToReadDto());
    }

    public async Task<IServiceResult<List<PurchaseReadDto>>> GetPurchasesAsync(string username)
    {
        var userIdUsername = await Context.Users.AsNoTracking()
            .Select(x => new { x.Id, x.Username })
            .FirstOrDefaultAsync(x => x.Username == username);
        
        if  (userIdUsername == null)
            return _serviceResultFactory.Failure<List<PurchaseReadDto>>(OperationStatus.NotFound);
        
        var userPurchases = await Context.Purchases
            .AsNoTracking()
            .Include(purchase => purchase.ProductPurchases)
            .Where(x => x.UserId == userIdUsername.Id)
            .ToListAsync();

        var purchasesDtos = userPurchases.Select(purchase => purchase.MapToReadDto()).ToList();
        
        return _serviceResultFactory.Success(purchasesDtos);
    }
}