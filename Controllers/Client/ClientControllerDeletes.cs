using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstorekle.Data;

namespace Unstore.Controllers;

[ApiController]
public partial class ClientController : ControllerBase
{
    [HttpGet("/v1/clients/delete/{id:int}")]
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