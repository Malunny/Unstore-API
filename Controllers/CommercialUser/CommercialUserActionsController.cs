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
public class CommercialUserActionsController : UnstoreController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto,
        [FromServices] CommercialUserActionService service)
    {
        var username = User.Identity?.Name;
        
        if (string.IsNullOrEmpty(username))
            return Unauthorized("You are not logged in");
        
        var serviceResult = await service.CreateProductAsync(dto, username);

        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromServices] CommercialUserActionService service)
    {
        var username = User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
            return Unauthorized("You are not logged in");

        var serviceResult = await service.GetOwnProductsAsync(username);

        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPut("{productId}")]
    public async Task<IActionResult> Update([FromBody] ProductUpdateDto dto,
        [FromServices] CommercialUserActionService service)
    {
        var username = User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
            return Unauthorized("You are not logged in");

        var serviceResult = await service.UpdateProductAsync(dto, username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPatch("{productId}")]
    public async Task<IActionResult> SwitchActive([FromRoute] int productId,
        [FromServices] CommercialUserActionService service,
        [FromServices] UserVerificationService userVerificationService)
    {
        var username = User.Identity?.Name;
        
        var serviceResult = await service.SwitchProductActiveAsync(productId, username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }
    [HttpDelete("{productId}")]
    public async Task<IActionResult> Delete([FromRoute] int productId,
        [FromServices] CommercialUserActionService service)
    {
        var username = User.Identity?.Name;

        var serviceResult = await service.DeleteProductAsync(productId, username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }
}