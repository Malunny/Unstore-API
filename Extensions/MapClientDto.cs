using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Unstore.DTO;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Extensions;

public static class MapClientDto
{
    public static IQueryable<ClientReadDto> Map(this IQueryable<Client> clients)
    {
        return clients.Select(client => new ClientReadDto
            (client.Name,client.Email,client.Address,client.ContactNumber));
    }
    
    public static async Task<ResultDto<ClientReadDto>> MapOne(this IQueryable<Client> clients, int id)
    {
        var client = await clients.FirstOrDefaultAsync(c => c.Id == id);
        
        if (client == null)
            return new ResultDto<ClientReadDto>(null, errors: new List<ResultErrorMessage>()
            {
                new ResultErrorMessage(ErrorCode.NotFound, "Client not found")
            });
        return new ResultDto<ClientReadDto>(new ClientReadDto
            (client.Name,client.Email,client.Address,client.ContactNumber));
    }
    /*
    public static async Task<IEnumerable<ServiceResponse>> MapServices(this DbSet<Client> clientsTable, int id)
    {
        var clients = await clientsTable
            .Include(c => c.Services)
            .ThenInclude(s => s.Products)
            .Include(c => c.Services)
            .ThenInclude(s => s.Tools)
            .Include(c => c.Services)
            .ThenInclude(s => s.Employee)
            .Where(x => x.Id == id)
            .ToListAsync();
        List<ServiceResponse> serviceResponses = new();
        foreach (var client in clients)
        {
            foreach (var service in client.Services)
            {
                serviceResponses.Add(
                    new ServiceResponse()
                {
                    ClientName = client.Name,
                    ClientEmail = client.Email,
                    ClientContact = client.ContactNumber,
                    EmployeeName = service.Employee.Name,
                    EmployeeEmail = service.Employee.Email,
                    EmployeePhone = service.Employee.ContactNumber,
                    Address = service.Address,
                    Details = service.Details,
                    Products = service.Products.Select
                        (p => new ProductResponse
                            {
                                Name = p.Name,
                                Description = p.Description,
                                Value = p.Value
                            }
                        ).ToList(),
                    Tools = service.Tools.Select
                        (t => new ToolResponse
                            {
                                Name = t.Name,
                                Description = t.Description
                            }
                        ).ToList()
                });
            }
        }
        return serviceResponses;
    }
    */
}