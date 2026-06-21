using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Unstore.DTOs;
using Unstore.Services;
using Unstore.Models;
using Unstore.Extensions;
using Unstore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Unstore.Services.Account;

namespace Unstore.Controllers;

public partial class AccountController(IMapper mapper) : ControllerBase
{
    private IMapper _mapper = mapper;

    [AllowAnonymous]
    [HttpPost("/login")]
    public async Task<IActionResult> Login(
        [FromBody] UserLoginDto user,
        [FromServices] AccountService accountService)
    {
        var result = await accountService.TryLoginAsync(user, ModelState);
        
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine(result.OperationStatus);
        Console.WriteLine(result.Data);
        Console.WriteLine(result.Ok);
        Console.ResetColor();
        
        if (!result.Ok)
            return BadRequest(result.OperationStatus);

        return Ok(result.Data);
    }

    [AllowAnonymous]
    [HttpPost("/register")]
    public async Task<IActionResult> Register(
        [FromBody] UserCreateDtos user,
        [FromServices] AccountService accountService)
    {
        var result = await accountService.TryRegisterAsync(user, ModelState);
        
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine(result.OperationStatus);
        Console.WriteLine(result.Data);
        Console.WriteLine(result.Ok);
        Console.ResetColor();

        if (!result.Ok)
            return BadRequest(result.OperationStatus);

        return Ok(result.Data);
    }

    [HttpGet("/users")]
    public async Task<IActionResult> GetAllUser([FromServices] AppDbContext context)
    {
        var usersTracked = await context.Users.Include(x => x.Roles).ToListAsync();
        var users = _mapper.Map<IEnumerable<User>, IEnumerable<UserReadDto>>(usersTracked);
        
        return Ok(users);
    }
}