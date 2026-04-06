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

public partial class UserController
{
    [HttpGet("/v1/users/{skip:int}/{take:int}")]    
    public async Task<IActionResult> GetAsync([FromServices] AppDbContext context, [FromRoute] int skip, [FromRoute] int take)
    {
        var users = await context.Users
            .Skip(skip)
            .Take(take)
            .Include(u => u.Role)
            .ToListAsync();
        return Ok(_mapper.Map<IEnumerable<UserReadDto>>(users));
    }
}
