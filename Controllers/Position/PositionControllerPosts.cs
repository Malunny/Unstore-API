using Microsoft.AspNetCore.Mvc;
using Unstore.DTO;
using Unstore.DTOs;
using Unstore.Extensions;
using Unstore.Data;
using Unstore.Models;
using Microsoft.AspNetCore.Authorization;
using Unstore.Services;
using System.Runtime.InteropServices;

namespace Unstore.Controllers;

public partial class PositionController
{
    
    [HttpPost("/v1/positions/")]
    public async Task<IActionResult> PostAsync
    ([FromBody] PositionCreateDto positionCreateDto,
    [FromServices] AppDbContext context,
    [FromServices] PositionService positionService)
    {
        var result = await positionService.CreateAsync(ModelState, positionCreateDto);
        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);
        else
            return Created("/v1/positions/post", result.Data);
    }
    
    [HttpPost("/v1/positions/many")]
    public async Task<IActionResult> PostRangeAsync
        ([FromBody] IEnumerable<PositionCreateDto> positionCreateDtos,
        [FromServices] PositionService positionService)
    {
        var result = await positionService.CreateRangeAsync(ModelState, positionCreateDtos);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Created($"/v1/positions/", result.Data);
    }
}