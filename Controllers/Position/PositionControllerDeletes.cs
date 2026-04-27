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
    
    [HttpDelete("/v1/positions/{id:int}")]
    public async Task<IActionResult> DeleteAsync
    ([FromRoute] int id,
    [FromServices] PositionService positionService)
    {
        var result = await positionService.RemoveAsync(id);
        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);
        else
            return NoContent();
    }
    
    [HttpDelete("/v1/positions/many")]
    public async Task<IActionResult> DeleteRangeAsync
        ([FromBody] IEnumerable<int> ids,
        [FromServices] PositionService positionService)
    {
        var result = await positionService.RemoveRangeAsync(ids);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return NoContent();
    }
}