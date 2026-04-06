using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using Unstore.Data;
using Unstore.DTO;
using Unstore.Extensions;
using Unstore.Models;
using Unstore.Mapper;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
        public async Task<ServiceResult<ClientCreateDto>> Create(ModelStateDictionary modelstate, ClientCreateDto client)
        {
            if (!modelstate.IsValid)
                return ServiceResult<ClientCreateDto>.Failure(modelstate.GetErrors());

            var clientMapped = mapper.Map<ClientCreateDto,Client>(client);

            await context.Clients.AddAsync(clientMapped);
            await context.SaveChangesAsync();

            return ServiceResult<ClientCreateDto>.Success(client);
        }

        public async Task<ServiceResult<IEnumerable<ClientCreateDto>>> CreateRange(ModelStateDictionary modelstate, IEnumerable<ClientCreateDto> clients)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<ClientCreateDto>>.Failure(modelstate.GetErrors());

            var clientMapped = mapper.Map<IEnumerable<ClientCreateDto>,IEnumerable<Client>>(clients);

            await context.Clients.AddRangeAsync(clientMapped);
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<ClientCreateDto>>.Success(clients);
        }

        public async Task<ServiceResult<ClientUpdateDto>> Update(ModelStateDictionary modelstate, ClientUpdateDto client)
        {
            if (!modelstate.IsValid)
                return ServiceResult<ClientUpdateDto>.Failure(modelstate.GetErrors());
            
            var clientTrack = await context.Clients.FirstOrDefaultAsync(x => x.Id == client.Id);

            var mapped = mapper.Map<ClientUpdateDto, Client>(client, clientTrack);
            System.Console.WriteLine($"""

                {mapped.Name}
                {mapped.Address}
                {mapped.Email}
                {mapped.ContactNumber}
            """);
            var returnedClient = mapper.Map(mapped, new ClientUpdateDto());
            await context.SaveChangesAsync();

            return ServiceResult<ClientUpdateDto>.Success(returnedClient, OperationStatus.Updated);
        }

        public async Task<ServiceResult<ClientReadDto>> GetById(int id)
        {
            var client = await context.Clients.FirstOrDefaultAsync(cli => cli.Id == id);

            if (client is null)
                return ServiceResult<ClientReadDto>.Failure(OperationStatus.NotFound);

            return ServiceResult<ClientReadDto>
                .Success(mapper.Map<Client,ClientReadDto>(client), OperationStatus.Ok);
        }

        public async Task<ServiceResult<IEnumerable<ClientReadDto?>>> GetByIds(IEnumerable<int> ids)
        {
            Dictionary<int,Client> clientsWithId = await context.Clients.Where(cli => ids.Contains(cli.Id)).ToDictionaryAsync(c => c.Id);
            List<ClientReadDto?> clients = new();
            var results = new List<ResultStatusMessage>();

            foreach (var id in ids)
            {
                if (clientsWithId.TryGetValue(id, out var client))
                {
                    var clientReadDto = mapper.Map<Client, ClientReadDto>(client);
                    results.Add(OperationStatus.Ok.ToResultStatusMessage());
                }
                else
                {
                    clients.Add(null);
                    results.Add(OperationStatus.NotFound.ToResultStatusMessage());
                }
            }
            return ServiceResult<IEnumerable<ClientReadDto?>>.MultipleResults(clients, results);
        }

        public async Task<ServiceResult<IEnumerable<ClientReadDto>>> GetRangeAsync(int skip, int take)
        {
            var clients = await context.Clients.Skip(skip).Take(take).ToListAsync();
            var clientsMapped = new List<ClientReadDto>();
            foreach (var client in clients)
                clientsMapped.Add(mapper.Map<Client, ClientReadDto>(client));
            var result = ServiceResult<IEnumerable<ClientReadDto>>.Success(clientsMapped);
            return result;
        }
    }
}