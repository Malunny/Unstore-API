using Microsoft.AspNetCore.Mvc;
using Unstore.DTO;
using Unstore.DTOs;
using Unstore.Extensions;
using Unstore.Data;
using Unstore.Models;
using Microsoft.AspNetCore.Authorization;

namespace Unstore.Controllers;

public partial class ClientController
{
    
    [HttpPost("/v1/clients/post")]
    public async Task<IActionResult> PostAsync
    ([FromBody] ClientCreateDto client, [FromServices] AppDbContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultDto<ClientCreateDto>(client, errors: ModelState.GetErrors()));

        var mappedClient = _mapper.Map<ClientCreateDto, Client>(client);

        await context.Clients.AddAsync(mappedClient);
        await context.SaveChangesAsync();

        return Created("/v1/clients/post", client);
    }
    
    [HttpPost("/v1/clients/post-range")]
    public async Task<IActionResult> PostRangeAsync
        ([FromBody] IEnumerable<ClientUpdateDto> clients, [FromServices] AppDbContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultDto<IEnumerable<ClientUpdateDto>>(clients, ModelState.GetErrors()));

        var mappedClients = _mapper.Map<IEnumerable<ClientUpdateDto>, IEnumerable<Client>>(clients);

        await context.Clients.AddRangeAsync(mappedClients);
        await context.SaveChangesAsync();

        return Created("/v1/clients/post", clients);
    }
}