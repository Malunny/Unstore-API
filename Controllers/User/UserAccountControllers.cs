using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Services;
using Unstore.Services.Account;

namespace Unstore.Controllers.User;

public partial class UserController
{
    [HttpPost("/login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto userLoginDto,
        [FromServices] AccountService accountService)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var serviceResult = await accountService.TryLoginAsync(userLoginDto);   

        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPost("/register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromBody] UserCreateDtos registerDto,
        [FromServices] AccountService accountService)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var serviceResult = await accountService.TryRegisterAsync(registerDto);

        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPatch("v1/user/password")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] string newPassword,
        [FromServices] AccountService accountService)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var username = User.Identity?.Name;
        
        if (username == null)
            return BadRequest("User not authenticated");
        
        var serviceResult = await accountService.ChangePasswordAsync(newPassword, username);

        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPatch("v1/user/password/{username}")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] string newPassword,
        [FromHeader] string username,
        [FromServices] AccountService accountService,
        [FromServices] AuthorizationService authorizationService)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        if (!authorizationService.IsManagerOrAdmin(User))
            return Unauthorized();

        var serviceResult = await accountService.ChangePasswordAsync(newPassword, username);

        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPut("v1/user")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateAsync([FromBody] UserUpdateDto userUpdateDto,
        [FromServices] AccountService accountService)
    {
        var username = User.Identity?.Name;
        
        if (username == null)
            return Unauthorized();
        
        var serviceResult = await accountService.UpdateUserAsync(userUpdateDto, username);
        Console.WriteLine(serviceResult.OperationStatus.ToObjectResult(serviceResult.Data));
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [AllowAnonymous]
    [HttpGet("v1/user")]
    public IActionResult GetUsers([FromServices] AppDbContext db)
    {
        return Ok(db.Users.Include(x => x.Addresses)
            .Include(x => x.Roles).AsNoTracking());
    }
    
}