using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTO;
using Unstore.Extensions;
using Unstore.Models;

namespace Unstore.Services
{
    public class ClientService
    {
        private readonly IMapper mapper;
        private readonly AppDbContext context;

        public ClientService(IMapper map, AppDbContext context)
        {
            mapper = map;
            this.context = context;
        }
        public async Task<ServiceResult<ClientCreateDto>> CreateAsync(ModelStateDictionary modelstate, ClientCreateDto clientCreateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<ClientCreateDto>.Failure(modelstate.GetErrors());

            var clientMapped = mapper.Map<ClientCreateDto,Client>(clientCreateDto);

            await context.Clients.AddAsync(clientMapped);
            await context.SaveChangesAsync();

            return ServiceResult<ClientCreateDto>.Success(clientCreateDto);
        }

        public async Task<ServiceResult<IEnumerable<ClientCreateDto>>> CreateRangeAsync(ModelStateDictionary modelstate, IEnumerable<ClientCreateDto> clientCreateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<ClientCreateDto>>.Failure(modelstate.GetErrors());

            var clientMapped = mapper.Map<IEnumerable<ClientCreateDto>,IEnumerable<Client>>(clientCreateDtos);

            await context.Clients.AddRangeAsync(clientMapped);
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<ClientCreateDto>>.Success(clientCreateDtos);
        }

        public async Task<ServiceResult<ClientUpdateDto>> UpdateAsync(ModelStateDictionary modelstate, ClientUpdateDto clientUpdateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<ClientUpdateDto>.Failure(modelstate.GetErrors());
            
            var clientFromDb = await context.Clients.FirstOrDefaultAsync(x => x.Id == clientUpdateDto.Id);

            if (clientFromDb is null)
                return ServiceResult<ClientUpdateDto>.Failure(OperationStatus.NotFound);

            var mapped = mapper.Map<ClientUpdateDto, Client>(clientUpdateDto, clientFromDb);
            var returnedClient = mapper.Map(mapped, new ClientUpdateDto());
            await context.SaveChangesAsync();

            return ServiceResult<ClientUpdateDto>.Success(returnedClient, OperationStatus.Updated);
        }
        public async Task<ServiceResult<IEnumerable<ClientUpdateDto>>> UpdateRangeAsync
            (ModelStateDictionary modelstate, IEnumerable<ClientUpdateDto> clientUpdateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<ClientUpdateDto>>.Failure(modelstate.GetErrors());
            
            var clientsList = clientUpdateDtos.ToList();
            List<int> clientIds = new(clientsList.Select(c => c.Id));
            var clientsFromDb = await context.Clients.Where(x => clientIds.Contains(x.Id)).ToListAsync();

            if (clientsList.Count > clientsFromDb.Count)
                return ServiceResult<IEnumerable<ClientUpdateDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more clients wasn't found"));

            List<Client> mappedClientsList = new();

            for (int i = 0; i < clientsFromDb.Count; i++)
                 mappedClientsList.Add(mapper.Map(clientsList[i], clientsFromDb[i]));

            var returnedClients = mapper.Map(mappedClientsList, new List<ClientUpdateDto>());
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<ClientUpdateDto>>.Success(returnedClients, OperationStatus.Updated);
        }

        public async Task<ServiceResult<ClientReadDto>> GetByIdAsync(int id)
        {
            var client = await context.Clients.FirstOrDefaultAsync(cli => cli.Id == id);

            if (client is null)
                return ServiceResult<ClientReadDto>.Failure(OperationStatus.NotFound);

            return ServiceResult<ClientReadDto>
                .Success(mapper.Map<Client,ClientReadDto>(client), OperationStatus.Ok);
        }

        public async Task<ServiceResult<IEnumerable<ClientReadDto>>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            var clientsFromDb = await context.Clients.Where(cli => idsList.Contains(cli.Id)).ToListAsync();

            if (idsList.Count > clientsFromDb.Count)
                return ServiceResult<IEnumerable<ClientReadDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more clients wasn't found"));

            var clientsDtos = mapper.Map<IEnumerable<Client>, IEnumerable<ClientReadDto>>(clientsFromDb);
            return ServiceResult<IEnumerable<ClientReadDto>>.Success(clientsDtos);
        }

        public async Task<ServiceResult<IEnumerable<ClientReadDto>>> GetRangeAsync(int skip, int take)
        {
            if (skip < 0 || take < 0)
                return ServiceResult<IEnumerable<ClientReadDto>>.Failure(OperationStatus.InvalidInput);

            var clientsFromDb = await context.Clients.Skip(skip).Take(take).ToListAsync();
            var clientsDtos = mapper.Map<IEnumerable<Client>, IEnumerable<ClientReadDto>>(clientsFromDb);
            return ServiceResult<IEnumerable<ClientReadDto>>.Success(clientsDtos);
        }

        public async Task<ServiceResult<object?>> RemoveAsync(int id)
        {
            Client? client = await context.Clients.FirstOrDefaultAsync(cli => cli.Id == id);
            if (client is null)
                return ServiceResult<object?>.Failure(OperationStatus.NotFound);
            context.Clients.Remove(client);
            await context.SaveChangesAsync();
            return ServiceResult<object?>.Success(null);
        }
        public async Task<ServiceResult<object?>> RemoveRangeAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            IEnumerable<Client> clients = await context.Clients.Where(cli => idsList.Contains(cli.Id)).ToListAsync();
            if (!clients.Any())
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't clients with these ids."));
            if (clients.Count() < idsList.Count)
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't clients with one or more of these ids."));
            context.Clients.RemoveRange(clients);
            await context.SaveChangesAsync();
            return ServiceResult<object?>.Success(null);
        }
    }
}