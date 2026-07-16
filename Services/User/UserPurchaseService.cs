using AutoMapper;
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
    public UserPurchaseService(AppDbContext dbContext, IMapper mapper, IServiceResultFactory serviceResultFactory) : base(dbContext, mapper)
    {
        _serviceResultFactory = serviceResultFactory;
    }

    public async Task<IServiceResult<Purchase>> AddPurchaseAsync(int userId,
        int userAddressId,
        ICollection<ProductPurchaseCreateDto> productPurchasesDtos)
    {
        if (userId < 1)
            return _serviceResultFactory.Failure<Purchase>(OperationStatus.InvalidInput);

        var user = await Context.Users.AsNoTracking()
            .Include(x => x.Addresses)
            .FirstOrDefaultAsync(x => x.Id == userId);
        
        if (user == null)
            return _serviceResultFactory.Failure<Purchase>(OperationStatus.InvalidInput);

        if (!user.Addresses.Select(x => x.Id).Contains(userAddressId))
            return _serviceResultFactory.Failure<Purchase>(OperationStatus.InvalidInput);

        var productIds = productPurchasesDtos.Select(y => y.ProductId).ToList();
        
        Dictionary<int, decimal> productsIdValues = await Context.Products
            .AsNoTracking()
            .Where(x => 
                productIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.Value);
        
        if (productsIdValues.Count < productPurchasesDtos.Count)
            return _serviceResultFactory.Failure<Purchase>(OperationStatus.InvalidInput);
        
        decimal totalValue = 0m;
        List<ProductPurchase> productPurchases = Mapper.Map<List<ProductPurchase>>(productPurchasesDtos);
        
        foreach (var productPurchaseDto in productPurchasesDtos)
            totalValue += productPurchaseDto.Quantity * productsIdValues[productPurchaseDto.ProductId];
        
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
        
        return _serviceResultFactory.Success(purchase);
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
            .Where(x => x.UserId == userIdUsername.Id)
            .ToListAsync();

        var purchasesDtos = new List<PurchaseReadDto>();
        
        foreach (var purchase in userPurchases)
            purchasesDtos.Add(purchase.MapToReadDto());
        
        return _serviceResultFactory.Success(purchasesDtos);
    }
}