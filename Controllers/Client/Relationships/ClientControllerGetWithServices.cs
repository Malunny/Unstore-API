using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.Extensions;
using Unstore.Data;

namespace Unstore.Controllers;
/*
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
}*/