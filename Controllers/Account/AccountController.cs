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
        var result = await accountService.ValidateLoginAsync(ModelState, user);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }

    [AllowAnonymous]
    [HttpPost("/register")]
    public async Task<IActionResult> Register(
        [FromBody] UserCreationDto user,
        [FromServices] AccountService accountService)
    {
        var result = await accountService.RegisterAsync(ModelState, user);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }

    [HttpGet("/users")]
    public async Task<IActionResult> GetAllUser([FromServices] AppDbContext context)
    {
        var usersTracked = await context.Users.Include(x => x.Role).ToListAsync();
        var users = _mapper.Map<IEnumerable<User>, IEnumerable<UserReadDto>>(usersTracked);
        return Ok(ServiceResult<IEnumerable<UserReadDto>>.Success(users));
    }
}