using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.Extensions;
using Unstore.ViewModels;
using Unstorekle.Data;
using Unstorekle.Models;

namespace Unstore.Controllers;

public partial class ClientController
{
    [HttpPut("/v1/clients/put/")]
    public async Task<IActionResult> PutByIdAsync
        ([FromBody] UpdateClientViewModel client,
            [FromServices] AppDbContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<UpdateClientViewModel>(client,errors: ModelState.GetErrors()));

        var clientTrack = await context.Clients.FirstOrDefaultAsync(x => x.Id == client.Id);

        Console.WriteLine("Tried PUT");
        
        clientTrack.Name = client.Name ?? clientTrack.Name;
        clientTrack.Address = client.Address ?? clientTrack.Address;
        clientTrack.ContactNumber = client.ContactNumber ?? clientTrack.ContactNumber;
        clientTrack.Email = client.Email ?? clientTrack.Email;
        
        context.Update(clientTrack);
        await context.SaveChangesAsync();
        
        return Ok(new ResultViewModel<UpdateClientViewModel>(client));
    }
    
    [HttpPut("/v1/clients/put-range")]
    public async Task<IActionResult> PutByIdAsync
        ([FromBody] IEnumerable<UpdateClientViewModel> clientsViewModels, [FromServices] AppDbContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<IEnumerable<UpdateClientViewModel>>(clientsViewModels, ModelState.GetErrors()));

        List<int> clientsIds = clientsViewModels.Select(x => x.Id).ToList();
        
        List<string>? warnings = null;
        
        var clientsTracked = await context.Clients
            .Where(x => clientsIds.Contains(x.Id))
            .ToListAsync();

        foreach (var clientTracked in clientsTracked)
        {
            var client = clientsViewModels.FirstOrDefault(x => x.Id == clientTracked.Id);
            if (client is null)
            {
                if (warnings is null)
                {
                    warnings = new List<string>();
                    warnings.Add($"{clientTracked.Id} was not found");
                }
                else
                    warnings.Add($"{clientTracked.Email} was not found");
                continue;
            }
                
            clientTracked.Name = client.Name ?? clientTracked.Name;
            clientTracked.Address = client.Address ?? clientTracked.Address;
            clientTracked.ContactNumber = client.ContactNumber ?? clientTracked.ContactNumber;
            clientTracked.Email = client.Email ?? clientTracked.Email;
        }
        context.UpdateRange(clientsTracked);
        await context.SaveChangesAsync();
        return Ok(new ResultViewModel<IEnumerable<UpdateClientViewModel>>(clientsViewModels, warnings: warnings));
    }
}