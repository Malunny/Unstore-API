using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Unstore.DTOs;
using Unstore.Services;
using Unstore.Models;
using Unstore.Extensions;
using Unstore.Data;

namespace Unstore.Controllers;
public partial class UserController([FromServices] IMapper mapper) : ControllerBase
{
    private IMapper _mapper = mapper;
    [HttpPost("/v1/users/create")]
    public IActionResult Create
    (
        [FromBody] UserCreationDto userCreationDto,
        [FromServices] AppDbContext context
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ServiceResult<UserCreationDto>.Failure(ModelState.GetErrors()));
        
        var userMapped = _mapper.Map<Models.User>(userCreationDto);
        var role = context.Roles.First(x => x.Id == 3);

        userMapped.RoleId = role.Id;
        userMapped.Role = role;

        context.Users.Add(userMapped);
        context.SaveChanges();
        return Created("/v1/users/", userCreationDto.Email);
    }
}
