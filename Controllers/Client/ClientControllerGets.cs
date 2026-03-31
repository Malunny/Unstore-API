using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstorekle.Data;

namespace Unstore.Controllers;

public partial class ClientController : ControllerBase
{
    [HttpGet("/v1/clients/get-all")]
    public async Task<IActionResult> GetAllAsync
        ([FromServices] AppDbContext context)
    {
        var clients = await context.Clients.ToListAsync();
        Console.WriteLine("GET ALL - CLIENTS EXECUTED");
        
        return Ok(clients);    
    }
    
    [HttpGet("/v1/clients/get/{id:int}")]
    public async Task<IActionResult> GetByIdAsync
            ([FromRoute] int id,
            [FromServices] AppDbContext context)
    {
        var client = await context.Clients.FirstOrDefaultAsync(x => x.Id == id);
        Console.WriteLine("GET BY ID - CLIENTS EXECUTED");
        
        if (client == null)
            return NotFound();

        return Ok(client);    
    }
}