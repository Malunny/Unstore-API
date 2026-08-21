using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unstore.Services;

namespace Unstore.Controllers.User;

[Authorize]
public class UserSocialController : UnstoreController
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserData([FromRoute] int id, [FromServices] UserService userService)
    {
        var result = await userService.GetByIdAsync(id);

        return result.OperationStatus.ToObjectResult(result.Data);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsersData([FromQuery] int[] ids, [FromServices] UserService userService)
    {
        var result = await userService.GetByIdsAsync(ids);

        return result.OperationStatus.ToObjectResult(result.Data);
    }
}