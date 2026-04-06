using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Unstore.DTOs;
using Unstore.Services;
using Unstore.Models;
using Unstore.Extensions;
using Unstore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Unstore.Controllers;

public partial class AccountController(IMapper mapper) : ControllerBase
{
    private IMapper _mapper = mapper;

    [HttpPost("/v1/users/login")]
    public IActionResult Login(
        [FromServices] TokenService tokenService,
        [FromBody] UserLoginDto user,
        [FromServices] AppDbContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ServiceResult<UserLoginDto>.MultipleResults(user, ModelState.GetErrors()));
        var userTracked = context.Users.Include(u => u.Role).FirstOrDefault(x => x.Email == user.Email);

        if (userTracked == null)
            return Unauthorized(ServiceResult<UserLoginDto>.Failure(OperationStatus.NotFound));
        if (userTracked.Password != user.Password)
            return Unauthorized(ServiceResult<UserLoginDto>.Failure(OperationStatus.InvalidCredentials));

        var token = tokenService.GenerateToken(userTracked);
        return Ok(token);
    }

    [HttpPost("/v1/users/register")]
    public async Task<IActionResult> Register(
        [FromBody] UserCreationDto user,
        [FromServices] AppDbContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ServiceResult<UserCreationDto>.Failure(user, ModelState.GetErrors()));
        if (await context.Users.AnyAsync(x => x.Email == user.Email || x.Username == user.Username))
            return BadRequest(ServiceResult<UserCreationDto>.Failure(OperationStatus.UserAlreadyExists.ToResultStatusMessage()));
        
        var userMapped = _mapper.Map<Models.User>(user);
        var role = await context.Roles.FirstAsync(x => x.Id == 3);

        userMapped.RoleId = role.Id;
        userMapped.Role = role;

        await context.Users.AddAsync(userMapped);
        await context.SaveChangesAsync();

        return Ok(ServiceResult<UserCreationDto>.Success(user, OperationStatus.Created));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("v1/users/getall")]
    public async Task<IActionResult> GetAllUser([FromServices] AppDbContext context)
    {
        var usersTracked = await context.Users.Include(x => x.Role).ToListAsync();
        var users = _mapper.Map<IEnumerable<User>, IEnumerable<UserCreationDto>>(usersTracked);
        return Ok(ServiceResult<IEnumerable<UserCreationDto>>.Success(users));
    }
}