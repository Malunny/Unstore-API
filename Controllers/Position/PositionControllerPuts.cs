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
    
    [HttpPut("/v1/positions/")]
    public async Task<IActionResult> PutAsync
    ([FromBody] PositionUpdateDto positionUpdateDto,
    [FromServices] AppDbContext context,
    [FromServices] PositionService positionService)
    {
        var result = await positionService.UpdateAsync(ModelState, positionUpdateDto);
        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);
        else
            return Ok(result.Data);
    }
    
    [HttpPut("/v1/positions/many")]
    public async Task<IActionResult> PutRangeAsync
        ([FromBody] IEnumerable<PositionUpdateDto> positionUpdateDtos,
        [FromServices] PositionService positionService)
    {  
        var result = await positionService.UpdateRangeAsync(ModelState, positionUpdateDtos);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }
}