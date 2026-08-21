using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unstore.DTOs;
using Unstore.Services;

namespace Unstore.Controllers.User;

[Authorize]
public class UserPurchaseController : UnstoreController
{
    [HttpGet]
    public async Task<IActionResult> GetMyPurchases([FromServices] UserPurchaseService userPurchaseService)
    {
        var username = User.Identity?.Name;
        
        if (username is null)
            return Unauthorized();
        
        var serviceResult = await userPurchaseService.GetPurchasesAsync(username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Purchase([FromServices] UserPurchaseService userPurchaseService, 
        [FromQuery] int addressId,
        [FromBody] ICollection<ProductPurchaseCreateDto> productPurchaseCreateDtos)
    {
        var username = User?.Identity?.Name;
        
        var serviceResult = await userPurchaseService.AddPurchaseAsync(username, addressId, productPurchaseCreateDtos);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }
}