using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.Extensions;
using Unstore.ViewModels;
using Unstorekle.Data;

namespace Unstore.Controllers;

public partial class ClientController : ControllerBase
{
    [HttpGet("/v1/clients/get-all")]
    public async Task<IActionResult> GetAllAsync
        ([FromServices] AppDbContext context)
    {
        var clients = await context.Clients.MapEditor().ToListAsync();
        Console.WriteLine("GET ALL - CLIENTS EXECUTED");
        
        return Ok(new ResultViewModel<IEnumerable<EditorClientViewModel>>(clients));    
    }
    
    [HttpGet("/v1/clients/get/{id:int}")]
    public async Task<IActionResult> GetByIdAsync
            ([FromRoute] int id,
            [FromServices] AppDbContext context)
    {
        var client = await context.Clients.Where(client => client.Id == id)
                                          .MapEditor()
                                          .FirstOrDefaultAsync();
        Console.WriteLine("GET BY ID - CLIENTS EXECUTED");
        
        if (client == null)
            return NotFound(new ResultViewModel<object>([$"Client with id {id} not found"]));

        return Ok(new ResultViewModel<EditorClientViewModel>(client));    
    }
}