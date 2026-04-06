using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.DTO;
using Unstore.DTOs;
using Unstore.Extensions;
using Unstore.Data;
using Unstore.Models;
using Unstore.Services;

namespace Unstore.Controllers;

public partial class ClientController
{
    
    [HttpPut("/v1/clients/put/")]
    public async Task<IActionResult> PutByIdAsync
        ([FromBody] ClientUpdateDto client,
        [FromServices] AppDbContext context,
        [FromServices] ClientService service)
    {
        var result = await service.Update(ModelState, client);
        if (result.IsBadResult())
            return BadRequest(result);

        return Ok(result);
    }
    /*
    [HttpPut("/v1/clients/put-range")]
    public async Task<IActionResult> PutByIdAsync
        ([FromBody] IEnumerable<ClientUpdateDto> clientsUpdateDtos, [FromServices] AppDbContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ServiceResult<IEnumerable<ClientUpdateDto>>.Failure(clientsUpdateDtos, ModelState.GetErrors()));

        int[] clientsIds = clientsUpdateDtos.Select(x => x.Id).ToArray();

        var trackedClients = await context.Clients.Where(x => clientsIds.Contains(x.Id)).ToListAsync();

        foreach (var client in clientsUpdateDtos)
            _mapper.Map(client, trackedClients.FirstOrDefault(x => x.Id == client.Id));
        
        await context.SaveChangesAsync();
        return Ok(ServiceResult<IEnumerable<ClientUpdateDto>>.Success(clientsUpdateDtos));
    }
    */
}