using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Services;
using Unstore.Services.Account;

namespace Unstore.Controllers.User;

[Authorize]
public class UserAccountController : UnstoreController
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto,
        [FromServices] AccountService accountService)
    {
        var serviceResult = await accountService.TryLoginAsync(userLoginDto);   

        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserCreateDtos registerDto,
        [FromServices] AccountService accountService)
    {
        var serviceResult = await accountService.TryRegisterAsync(registerDto);

        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpGet]
    public async Task<IActionResult> OwnUser([FromServices] AccountService accountService)
    {
        string? username = User?.Identity?.Name;
        
        if (username == null)
            return BadRequest();

        var serviceResult = await accountService.GetOwnUserAsync(username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpGet]
    public async Task<IActionResult> OwnAddresses([FromServices] AccountService accountService)
    {
        string? username = User?.Identity?.Name;
        
        if (username == null)
            return BadRequest();

        var serviceResult = await accountService.GetOwnAddressesAsync(username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPost]
    public async Task<IActionResult> AddAddress([FromServices] AccountService accountService,
        [FromBody] UserAddressCreateDto addressDto)
    {
        var username = User.Identity?.Name;

        Console.WriteLine(username);
        
        if (username == null)
            return BadRequest();
        
        var serviceResult = await accountService.AddAddressAsync(addressDto, addressDto.AddressTypeKey, username);
        
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPatch]
    public async Task<IActionResult> ChangePassword([FromBody] string newPassword,
        [FromServices] AccountService accountService)
    {
        var username = User.Identity?.Name;
        
        if (username == null)
            return BadRequest();
        
        var serviceResult = await accountService.ChangePasswordAsync(newPassword, username);

        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [Authorize(Roles = "Administrator, Manager")]
    [HttpPatch("{username}")]
    public async Task<IActionResult> ChangePassword([FromBody] string newPassword,
        [FromHeader] string username,
        [FromServices] AccountService accountService)
    {
        var serviceResult = await accountService.ChangePasswordAsync(newPassword, username);

        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserUpdateDto userUpdateDto,
        [FromServices] AccountService accountService)
    {
        var username = User.Identity?.Name;
        
        if (username == null)
            return Unauthorized();
        
        var serviceResult = await accountService.UpdateUserAsync(userUpdateDto, username);
        Console.WriteLine(serviceResult.OperationStatus.ToObjectResult(serviceResult.Data));
        return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
    }
    
    [HttpGet]
    [Authorize(Roles = "Administrator, Manager")]
    public IActionResult Users([FromServices] AppDbContext db)
    {
        return Ok(db.Users.Include(x => x.Addresses)
            .Include(x => x.Roles).AsNoTracking());
    }
    
}