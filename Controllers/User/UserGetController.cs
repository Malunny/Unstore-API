using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;

namespace Unstore.Controllers;

public partial class UserController(IMapper mapper) : ControllerBase
{
    private IMapper _mapper = mapper;
    [HttpGet("/v1/users/")]    
    public IActionResult GetAll([FromServices] AppDbContext context)
    {
        var users = context.Users.Include(u => u.Role).ToList();
        return Ok(_mapper.Map<IEnumerable<UserGetDto>>(users));
    }
}
