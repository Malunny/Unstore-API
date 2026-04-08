using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.Services;

namespace Unstore.Controllers;

public partial class ClientController : ControllerBase
{
    [HttpDelete("/v1/clients/{id:int}")]
    public async Task<IActionResult> DeleteByIdAsync
        ([FromRoute] int id, [FromServices] ClientService service)
    {
        var result = await service.RemoveAsync(id);
        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);
        return Ok();
    }
    [HttpDelete("/v1/clients/")]
    public async Task<IActionResult> DeleteByIdAsync
        ([FromBody] IEnumerable<int> ids, [FromServices] ClientService service)
    {
        var result = await service.RemoveRangeAsync(ids);
        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);
        return Ok();
    }
}