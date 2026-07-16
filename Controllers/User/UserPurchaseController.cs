using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unstore.Services;

namespace Unstore.Controllers.User;

[ApiController]
public class UserPurchaseController : ControllerBase
{
    [HttpGet("v1/user/purchases")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPurchases([FromServices] UserPurchaseService userPurchaseService)
    {
        var username = User.Identity?.Name;
        
        if (username is null)
            return Unauthorized();
        
        var serviceResult = await userPurchaseService.GetPurchasesAsync(username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }
}