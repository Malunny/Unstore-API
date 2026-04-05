using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.DTO;
using Unstore.DTOs;
using Unstore.Extensions;
using Unstore.Data;
using Unstore.Models;

namespace Unstore.Controllers;

public partial class ClientController
{
    
    [HttpPut("/v1/clients/put/")]
    public async Task<IActionResult> PutByIdAsync
        ([FromBody] ClientUpdateDto client,
        [FromServices] AppDbContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultDto<ClientUpdateDto>(client,errors: ModelState.GetErrors()));
        
        var clientTrack = await context.Clients.FirstOrDefaultAsync(x => x.Id == client.Id);
        var map = _mapper.Map(client, clientTrack);

        await context.SaveChangesAsync();
        
        return Ok(new ResultDto<ClientUpdateDto>(client));
    }
    
    [HttpPut("/v1/clients/put-range")]
    public async Task<IActionResult> PutByIdAsync
        ([FromBody] IEnumerable<ClientUpdateDto> clientsUpdateDtos, [FromServices] AppDbContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultDto<IEnumerable<ClientUpdateDto>>(clientsUpdateDtos, ModelState.GetErrors()));

        int[] clientsIds = clientsUpdateDtos.Select(x => x.Id).ToArray();

        var trackedClients = await context.Clients.Where(x => clientsIds.Contains(x.Id)).ToListAsync();

        foreach (var client in clientsUpdateDtos)
            _mapper.Map(client, trackedClients.FirstOrDefault(x => x.Id == client.Id));
        
        await context.SaveChangesAsync();
        return Ok(new ResultDto<IEnumerable<ClientUpdateDto>>(clientsUpdateDtos));
    }
}