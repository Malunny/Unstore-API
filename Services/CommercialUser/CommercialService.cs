using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.DTOs.Mapping;
using Unstore.Services.User;

namespace Unstore.Services.CommercialUser;

public class CommercialService([FromServices] AppDbContext context,
    [FromServices] IServiceResultFactory serviceResultFactory,
    [FromServices] UserVerificationService userVerificationService)
{
    private async Task<IServiceResult<Models.Product>> VerifyProductOwnerAndExistence(int productId, int userId)
    {
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == productId);

        if (product is null)
            return serviceResultFactory.Failure<Models.Product>(OperationStatus.NotFound);

        var commercialUser = await context.CommercialUsers
            .AsNoTracking()
            .Select(x => new { x.Id, x.OriginalUserId })
            .FirstOrDefaultAsync(x => x.OriginalUserId == userId);
        
        if (commercialUser is null)
            return serviceResultFactory.Failure<Models.Product>(OperationStatus.Unauthorized);

        var userIsOwner = product.SellerId == commercialUser.Id;

        if (!userIsOwner)
            return serviceResultFactory.Failure<Models.Product>(OperationStatus.Unauthorized);
        
        return serviceResultFactory.Success(product);
    }
    public async Task<IServiceResult<ProductReadDto[]>> GetOwnProductsAsync(string username)
    {
        var userId = await userVerificationService.VerifyUserExistenceAsync(username);

        if (userId == -1)
            return serviceResultFactory.Failure<ProductReadDto[]>(OperationStatus.NotFound);

        var commercialUser = await context.CommercialUsers
            .AsNoTracking()
            .Select(x => new { x.OriginalUserId, x.SellingProducts })
            .FirstOrDefaultAsync(x => x.OriginalUserId == userId);
        
        if (commercialUser is null)
            return serviceResultFactory.Failure<ProductReadDto[]>(OperationStatus.NotFound);

        var products = commercialUser.SellingProducts.ToArray();
        
        if (products.Length == 0)
            return serviceResultFactory.Failure<ProductReadDto[]>(OperationStatus.NotFound);
        
        var productsCount = products.Length;
        var dtos = new ProductReadDto[productsCount];
        
        for (int i = 0; i < productsCount; i++)
            dtos[i] = products[i].MapToDto();

        return serviceResultFactory.Success(dtos);
    }
    public async Task<IServiceResult<ProductCreateDto>> CreateProductAsync(ProductCreateDto dto, string username)
    {
        var userId = await userVerificationService.VerifyUserExistenceAsync(username);
        
        if (userId == -1)
            return serviceResultFactory.Failure<ProductCreateDto>(OperationStatus.NotFound);

        var commercialUser = await context.CommercialUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.OriginalUserId == userId);

        if (commercialUser is null)
            return serviceResultFactory.Failure<ProductCreateDto>(OperationStatus.NotFound);

        var existingCategoriesCount = await context.ProductCategories.CountAsync(category => dto.ProductCategories.Contains(category.Key));
        var allDtoCategoriesExists = dto.ProductCategories.Count == existingCategoriesCount;
        
        if (!allDtoCategoriesExists)
            return serviceResultFactory.Failure<ProductCreateDto>(OperationStatus.NotFound);
        
        var product = dto.MapToModel(commercialUser.Id);
        
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        return serviceResultFactory.Success(dto);
    }

    public async Task<IServiceResult<ProductUpdateDto>> UpdateProductAsync(ProductUpdateDto dto, string username)
    {
        var userId = await userVerificationService.VerifyUserExistenceAsync(username);

        if (userId == -1)
            return serviceResultFactory.Failure<ProductUpdateDto>(OperationStatus.InvalidCredentials);
        
        var product = await context.Products
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (product is null)
            return serviceResultFactory.Failure<ProductUpdateDto>(OperationStatus.NotFound);

        product.Description = dto.Description;
        product.Name = dto.Name;
        product.Value = dto.Value;
        
        await context.SaveChangesAsync();
        return serviceResultFactory.Success(data: dto, status: OperationStatus.Updated);
    }

    public async Task<IServiceResult<bool>> InactivateProduct(int productId, string? username)
    {
        var userId = await userVerificationService.VerifyUserExistenceAsync(username);

        if (userId == -1)
            return serviceResultFactory.Failure<bool>(OperationStatus.InvalidCredentials);
        
        var productVerification = await VerifyProductOwnerAndExistence(productId, userId);
        
        if (productVerification.OperationStatus.IsBadResult())
            return serviceResultFactory.Failure<bool>(productVerification.OperationStatus);
        
        var product = productVerification.Data;

        product.Active = false;

        await context.SaveChangesAsync();

        return serviceResultFactory.Success(OperationStatus.Patched, true);
    }
    
    public async Task<IServiceResult<bool>> ActivateProduct(int productId, string? username)
    {
        var userId = await userVerificationService.VerifyUserExistenceAsync(username);

        if (userId == -1)
            return serviceResultFactory.Failure<bool>(OperationStatus.InvalidCredentials);
        
        var productVerification = await VerifyProductOwnerAndExistence(productId, userId);
        
        if (productVerification.OperationStatus.IsBadResult())
            return serviceResultFactory.Failure<bool>(productVerification.OperationStatus);
        
        var product = productVerification.Data;

        product.Active = true;

        await context.SaveChangesAsync();

        return serviceResultFactory.Success(OperationStatus.Patched, true);
    }

    public async Task<IServiceResult<bool>> DeleteProductAsync(int productId, string? username)
    {
        var userId = await userVerificationService.VerifyUserExistenceAsync(username);

        if (userId == -1)
            return serviceResultFactory.Failure<bool>(OperationStatus.InvalidCredentials);
        
        var productVerification = await VerifyProductOwnerAndExistence(productId, userId);
        
        var product = productVerification.Data;

        
        if (productVerification.OperationStatus.IsBadResult())
            return serviceResultFactory.Failure<bool>(productVerification.OperationStatus);

        context.Products.Remove(product);
        await context.SaveChangesAsync();
        
        return serviceResultFactory.Success(OperationStatus.Deleted, true);
    }
}