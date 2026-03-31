using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstorekle.Data;
using Unstorekle.Models;

namespace Unstore.Controllers;

public partial class ClientController
{
    [HttpPut("/v1/clients/put/{id:int}")]
    public async Task<IActionResult> PutByIdAsync
        ([FromBody] Client client, [FromServices] AppDbContext context)
    {
        var clientTrack = await context.Clients.FirstOrDefaultAsync(x => x.Id == client.Id);

        Console.WriteLine("Tried PUT");
        
        clientTrack.Name = client.Name;
        clientTrack.Address = client.Address;
        clientTrack.ContactNumber = client.ContactNumber;
        clientTrack.Email = client.Email;
        clientTrack.Services = client.Services;
        
        context.Update(clientTrack);
        await context.SaveChangesAsync();
        
        return Ok(client);
    }
    
    [HttpPut("/v1/clients/put-range")]
    public async Task<IActionResult> PutByIdAsync
        ([FromBody] IEnumerable<Client> clients, [FromServices] AppDbContext context)
    {
        List<int> ids = clients.Select(x => x.Id).ToList();
        
        var clientsTrack = await context.Clients
            .Where(x => ids.Contains(x.Id))
            .FirstOrDefaultAsync();

        foreach (var clientTrack in clients)
        {
            var client = clients.FirstOrDefault(x => x.Id == clientTrack.Id);
            
            clientTrack.Name = client.Name;
            clientTrack.Address = client.Address;
            clientTrack.ContactNumber = client.ContactNumber;
            clientTrack.Email = client.Email;
            clientTrack.Services = client.Services;
        }
        
        context.UpdateRange(clientsTrack);
        await context.SaveChangesAsync();
        return Ok(clients);
    }
}