using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Unstore.DTOs;
using Unstore.Services;
using Unstore.Models;
using Unstore.Extensions;
using Unstore.Data;

namespace Unstore.Controllers;
public partial class UserController
{
    [HttpPost("/v1/users/create")]
    public IActionResult Create
    (
        [FromBody] UserCreationDto user,
        [FromServices] AppDbContext context
    )
    {
        System.Console.WriteLine($"{user.Email} - {user.Password}");
        System.Console.WriteLine($"{user.FirstName} - {user.LastName}");
        if (!ModelState.IsValid)
            return BadRequest(new ResultDto<UserCreationDto>(user, ModelState.GetErrors()));
        
        var userMapped = _mapper.Map<Models.User>(user);
        var role = context.Roles.First(x => x.Id == 3);
        userMapped.RoleId = role.Id;
        userMapped.Role = role;
        context.Users.Add(userMapped);
        context.SaveChanges();
        return Created("/v1/users/", user.Email);
    }
}
