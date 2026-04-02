using Microsoft.AspNetCore.Mvc;
using Unstore.Extensions;
using Unstore.ViewModels;
using Unstorekle.Data;
using Unstorekle.Models;

namespace Unstore.Controllers;

public partial class ClientController
{
    [HttpPost("/v1/clients/post")]
    public async Task<IActionResult> PostAsync
    ([FromBody] EditorClientViewModel client, [FromServices] AppDbContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<EditorClientViewModel>(client, errors: ModelState.GetErrors()));

        await context.Clients.AddAsync(client.MapModel());
        await context.SaveChangesAsync();
        return Created($"v1/clients/post", new ResultViewModel<EditorClientViewModel>(client));
    }
    
    [HttpPost("/v1/clients/post-range")]
    public async Task<IActionResult> PostRangeAsync
        ([FromBody] IEnumerable<EditorClientViewModel> clients, [FromServices] AppDbContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<IEnumerable<EditorClientViewModel>>(clients, ModelState.GetErrors()));

        await context.Clients.AddRangeAsync(clients.MapModel());
        await context.SaveChangesAsync();

        return Created
            (
                $"v1/clients/{clients.Count()}-quantity",
                new ResultViewModel<IEnumerable<EditorClientViewModel>>(clients)
            );
    }
}