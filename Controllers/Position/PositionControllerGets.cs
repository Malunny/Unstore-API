using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.DTO;
using Unstore.DTOs;
using Unstore.Extensions;
using Unstore.Data;
using Unstore.Models;
using Unstore.Services;

namespace Unstore.Controllers;

public partial class PositionController : ControllerBase
{
    [HttpGet("/v1/positions/{skip:int}/{take:int}")]
    public async Task<IActionResult> GetSkipTake
        ([FromServices] PositionService positionService,
        [FromRoute] int skip,
        [FromRoute] int take)
    {
        var result = await positionService.GetRangeAsync(skip, take);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }
    
    [HttpGet("/v1/positions/{id:int}")]
    public async Task<IActionResult> GetByIdAsync
            ([FromRoute] int id,
            [FromServices] PositionService positionService)
    {
        var result = await positionService.GetByIdAsync(id);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }
}