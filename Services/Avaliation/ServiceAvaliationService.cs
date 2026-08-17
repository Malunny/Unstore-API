using System.Linq;
using Unstore.DTOs.Mapping;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.Avaliation;

public class ServiceAvaliationService : BaseService
{
    public ServiceAvaliationService(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IServiceResult<List<ServiceAvaliationReadDto>>> GetAllAsync()
    {
        var avaliations = await Context.ServiceAvaliations.AsNoTracking().ToListAsync();
        var dtos = avaliations.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<ServiceAvaliationReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<ServiceAvaliationReadDto>> GetByIdAsync(int clientId, int serviceId)
    {
        if (clientId <= 0 || serviceId <= 0)
            return new DataServiceResult<ServiceAvaliationReadDto>(OperationStatus.InvalidInput, false);

        var avaliation = await Context.ServiceAvaliations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ClientId == clientId && x.ServiceId == serviceId);

        if (avaliation == null)
            return new DataServiceResult<ServiceAvaliationReadDto>(OperationStatus.NotFound, false);

        var dto = avaliation.MapToDto();
        return new DataServiceResult<ServiceAvaliationReadDto>(true, dto);
    }

    public async Task<IServiceResult<List<ServiceAvaliationReadDto>>> GetByServiceIdAsync(int serviceId)
    {
        if (serviceId <= 0)
            return new DataServiceResult<List<ServiceAvaliationReadDto>>(OperationStatus.InvalidInput, false);

        var avaliations = await Context.ServiceAvaliations
            .AsNoTracking()
            .Where(x => x.ServiceId == serviceId)
            .ToListAsync();

        var dtos = avaliations.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<ServiceAvaliationReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<ServiceAvaliationReadDto>> CreateAsync(ServiceAvaliationCreateDto createDto)
    {
        if (createDto == null || createDto.ClientId <= 0 || createDto.ServiceId <= 0)
            return new DataServiceResult<ServiceAvaliationReadDto>(OperationStatus.InvalidInput, false);

        var clientExists = await Context.Users.AnyAsync(x => x.Id == createDto.ClientId);
        if (!clientExists)
            return new DataServiceResult<ServiceAvaliationReadDto>(OperationStatus.NotFound, false);

        var serviceExists = await Context.Services.AnyAsync(x => x.Id == createDto.ServiceId);
        if (!serviceExists)
            return new DataServiceResult<ServiceAvaliationReadDto>(OperationStatus.NotFound, false);

        var avaliation = createDto.MapToModel();

        await Context.ServiceAvaliations.AddAsync(avaliation);
        await Context.SaveChangesAsync();

        var dto = avaliation.MapToDto();
        return new DataServiceResult<ServiceAvaliationReadDto>(OperationStatus.Created, true, dto);
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int clientId, int serviceId)
    {
        if (clientId <= 0 || serviceId <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        var avaliation = await Context.ServiceAvaliations
            .FirstOrDefaultAsync(x => x.ClientId == clientId && x.ServiceId == serviceId);

        if (avaliation == null)
            return new DataServiceResult<bool>(OperationStatus.NotFound, false);

        Context.ServiceAvaliations.Remove(avaliation);
        await Context.SaveChangesAsync();

        return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
    }
}
