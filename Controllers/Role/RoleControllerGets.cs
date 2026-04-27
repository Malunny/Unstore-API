using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.DTO;
using Unstore.DTOs;
using Unstore.Extensions;
using Unstore.Data;
using Unstore.Models;
using Unstore.Services;

namespace Unstore.Controllers;

public partial class RoleController : ControllerBase
{
    [HttpGet("/v1/roles/{skip:int}/{take:int}")]
    public async Task<IActionResult> GetSkipTake
        ([FromServices] RoleService roleService,
        [FromRoute] int skip,
        [FromRoute] int take)
    {
        var result = await roleService.GetRangeAsync(skip, take);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }
    
    [HttpGet("/v1/roles/{id:int}")]
    public async Task<IActionResult> GetByIdAsync
            ([FromRoute] int id,
            [FromServices] RoleService roleService)
    {
        var result = await roleService.GetByIdAsync(id);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }
}