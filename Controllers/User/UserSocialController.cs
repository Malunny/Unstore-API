using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unstore.Services;

namespace Unstore.Controllers.User;

[ApiController]
[Authorize]
public class UserSocialController : ControllerBase
{
    [HttpGet("v1/[controller]/[action]/{id:int}")]
    public async Task<IActionResult> GetUserData([FromRoute] int id, [FromServices] UserService userService)
    {
        var result = await userService.GetByIdAsync(id);

        return result.OperationStatus.ToObjectResult(result.Data);
    }
    
    [HttpGet("v1/[controller]/[action]")]
    public async Task<IActionResult> GetUsersData([FromQuery] int[] ids, [FromServices] UserService userService)
    {
        var result = await userService.GetByIdsAsync(ids);

        return result.OperationStatus.ToObjectResult(result.Data);
    }
}