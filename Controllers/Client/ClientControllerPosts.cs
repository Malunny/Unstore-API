using Microsoft.AspNetCore.Mvc;
using Unstorekle.Data;
using Unstorekle.Models;

namespace Unstore.Controllers;

public partial class ClientController
{
    [HttpPost("/v1/clients/post")]
    public async Task<IActionResult> PostAsync
    ([FromBody] Client client, [FromServices] AppDbContext context)
    {
        await context.Clients.AddAsync(client);
        await context.SaveChangesAsync();
        return Created($"v1/clients/{client.Id}",client);
    }
    
    [HttpPost("/v1/clients/post-range")]
    public async Task<IActionResult> PostRangeAsync
        ([FromBody] IEnumerable<Client> clients, [FromServices] AppDbContext context)
    {
        await context.Clients.AddRangeAsync(clients);
        await context.SaveChangesAsync();

        return Created($"v1/clients/{clients.Count()}-quantity", clients);
    }
}