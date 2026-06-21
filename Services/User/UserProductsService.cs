using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services;

public class UserProductsService : BaseService
{
    private readonly IServiceResultFactory _serviceResultFactory;
    public UserProductsService(AppDbContext dbContext, IMapper mapper, IServiceResultFactory serviceResultFactory) : base(dbContext, mapper)
    {
        _serviceResultFactory = serviceResultFactory;
    }

    public async Task<IServiceResult<Purchase>> AddPurchaseAsync(int userId,
        int userAddressId,
        ICollection<ProductPurchaseCreateDto> productPurchasesDtos)
    {
        if (userId < 1)
            return _serviceResultFactory.Failure<Purchase>(OperationStatus.InvalidInput);

        var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
        
        if (user == null)
            return _serviceResultFactory.Failure<Purchase>(OperationStatus.InvalidInput); 

        var productPurchaseCreates =
            productPurchasesDtos.ToDictionary(x => x.ProductId, x => x.Quantity);
        var productsPurchasesIds = productPurchaseCreates.Select(x => x.Key);
        
        var productsIds = await Context.Products.AsNoTracking()
            .Where(x => productsPurchasesIds.Contains(x.Id) && x.Active)
            .Select(x => x.Id)
            .ToListAsync();
        
        var purchase = new Purchase();
        
        return _serviceResultFactory.Failure<Purchase>(OperationStatus.InvalidInput);

        // var idQuantityProduct = new Dictionary<int, int>();
    }
}