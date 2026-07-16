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
public class CommercialUserActionsController : ControllerBase
{
    [HttpPost("v1/commercial/products")]
    public async Task<IActionResult> CreateProductAsync([FromBody] ProductCreateDto dto,
        [FromServices] CommercialUserActionService service)
    {
        var username = User.Identity?.Name;
        
        if (string.IsNullOrEmpty(username))
            return Unauthorized("You are not logged in");
        
        var serviceResult = await service.CreateProductAsync(dto, username);

        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpGet("v1/commercial/products")]
    public async Task<IActionResult> GetProductsAsync([FromServices] CommercialUserActionService service)
    {
        var username = User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
            return Unauthorized("You are not logged in");

        var serviceResult = await service.GetOwnProductsAsync(username);

        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPut("v1/commercial/products")]
    public async Task<IActionResult> UpdateProductAsync([FromBody] ProductUpdateDto dto,
        [FromServices] CommercialUserActionService service)
    {
        var username = User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
            return Unauthorized("You are not logged in");

        var serviceResult = await service.UpdateProductAsync(dto, username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPatch("v1/commercial/products/active/{productId:int}")]
    public async Task<IActionResult> SwitchProductActiveAsync([FromRoute] int productId,
        [FromServices] CommercialUserActionService service,
        [FromServices] UserVerificationService userVerificationService)
    {
        var username = User.Identity?.Name;
        
        var serviceResult = await service.SwitchProductActiveAsync(productId, username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpDelete("v1/commercial/products/{productId:int}")]
    public async Task<IActionResult> DeleteProductAsync(int productId,
        [FromServices] CommercialUserActionService service)
    {
        var username = User.Identity?.Name;

        var serviceResult = await service.DeleteProductAsync(productId, username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }
}