using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;

namespace Unstore.Controllers;

public partial class ClientController : ControllerBase
{
    [HttpDelete("/v1/clients/delete/{id:int}")]
    public async Task<IActionResult> DeleteByIdAsync
        ([FromRoute] int id, [FromServices] AppDbContext context)
    {
        var client = await context.Clients.FirstOrDefaultAsync(x => x.Id == id);
        
        if (client == null)
            return NotFound();
        
        context.Remove(client);
        await context.SaveChangesAsync();
        
        return Ok();
    }
}