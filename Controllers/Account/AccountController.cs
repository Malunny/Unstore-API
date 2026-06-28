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

    [HttpPost("/api/users/commercial")]
    public async Task<IActionResult> AddCommercialUser([FromBody] CommercialUserCreateDto dto, [FromServices] AccountService accountService)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var username = User?.Identity?.Name;
        
        if (username is null)
            return BadRequest();
        
        var serviceResult = await accountService.AddCommercialUserAsync(dto, username);
        
        if (!serviceResult.Ok)
            return BadRequest(serviceResult.OperationStatus);
        
        return Created("/users/commercial", dto);
    }

    [HttpGet("/users/commercial")]
    public IActionResult CommercialUsers([FromServices]  AppDbContext context)
    {
        return Ok(context.CommercialUsers.ToList());
    }
}