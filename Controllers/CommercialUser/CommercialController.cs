using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;
using Unstore.Services;
using Unstore.Services.CommercialUser;
using Unstore.Services.User;

namespace Unstore.Controllers.CommercialUser;

[ApiController]
public class CommercialController : UnstoreController
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto dto,
        [FromServices] CommercialService service)
    {
        var username = GetRequestUser();
        
        if (string.IsNullOrEmpty(username))
            return Unauthorized("You are not logged in");
        
        var serviceResult = await service.CreateProductAsync(dto, username);

        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromServices] CommercialService service)
    {
        var username = GetRequestUser();

        if (string.IsNullOrEmpty(username))
            return Unauthorized("You are not logged in");

        var serviceResult = await service.GetOwnProductsAsync(username);

        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPut("{productId}")]
    public async Task<IActionResult> Update([FromBody] ProductUpdateDto dto,
        [FromServices] CommercialService service)
    {
        var username = User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
            return Unauthorized("You are not logged in");

        var serviceResult = await service.UpdateProductAsync(dto, username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPatch("[controller]/[action]/{productId:int}")]
    public async Task<IActionResult> InactivateProduct([FromRoute] int productId,
        [FromServices] CommercialService service,
        [FromServices] UserVerificationService userVerificationService)
    {
        var username = User.Identity?.Name;
        
        var serviceResult = await service.InactivateProduct(productId, username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }
    [HttpPatch("[controller]/[action]/{productId:int}")]
    public async Task<IActionResult> ActivateProduct([FromRoute] int productId,
        [FromServices] CommercialService service,
        [FromServices] UserVerificationService userVerificationService)
    {
        var username = User.Identity?.Name;
        
        var serviceResult = await service.ActivateProduct(productId, username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }
    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] int productId,
        [FromServices] CommercialService service)
    {
        var username = User.Identity?.Name;

        var serviceResult = await service.DeleteProductAsync(productId, username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }
}