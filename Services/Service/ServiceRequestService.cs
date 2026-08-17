using Unstore.DTOs.Mapping;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.Service;

public class ServiceRequestService : BaseService
{
    public ServiceRequestService(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IServiceResult<List<ServiceServiceRequestReadDto>>> GetAllAsync()
    {
        var requests = await Context.ServiceRequests.AsNoTracking().ToListAsync();
        var dtos = requests.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<ServiceServiceRequestReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<ServiceServiceRequestReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.InvalidInput, false);

        var request = await Context.ServiceRequests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (request == null)
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.NotFound, false);

        var dto = request.MapToDto();
        return new DataServiceResult<ServiceServiceRequestReadDto>(true, dto);
    }

    public async Task<IServiceResult<List<ServiceServiceRequestReadDto>>> GetByServiceIdAsync(int serviceId)
    {
        if (serviceId <= 0)
            return new DataServiceResult<List<ServiceServiceRequestReadDto>>(OperationStatus.InvalidInput, false);

        var requests = await Context.ServiceRequests
            .AsNoTracking()
            .Where(x => x.ServiceId == serviceId)
            .ToListAsync();

        var dtos = requests.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<ServiceServiceRequestReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<List<ServiceServiceRequestReadDto>>> GetByRequesterIdAsync(int requesterId)
    {
        if (requesterId <= 0)
            return new DataServiceResult<List<ServiceServiceRequestReadDto>>(OperationStatus.InvalidInput, false);

        var requests = await Context.ServiceRequests
            .AsNoTracking()
            .Where(x => x.RequesterId == requesterId)
            .ToListAsync();

        var dtos = requests.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<ServiceServiceRequestReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<ServiceServiceRequestReadDto>> CreateAsync(ServiceServiceRequestCreateDto createDto)
    {
        if (createDto == null || createDto.ServiceId <= 0 || createDto.RequesterId <= 0)
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.InvalidInput, false);

        var serviceExists = await Context.Services.AnyAsync(x => x.Id == createDto.ServiceId);
        if (!serviceExists)
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.NotFound, false);

        var requesterExists = await Context.Users.AnyAsync(x => x.Id == createDto.RequesterId);
        if (!requesterExists)
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.NotFound, false);

        var request = createDto.MapToModel();
        request.RequestedAt = DateTime.Now;

        await Context.ServiceRequests.AddAsync(request);
        await Context.SaveChangesAsync();

        var dto = request.MapToDto();
        return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.Created, true, dto);
    }

    public async Task<IServiceResult<ServiceServiceRequestReadDto>> UpdateAsync(ServiceServiceRequestUpdateDto updateDto)
    {
        if (updateDto == null || updateDto.Id <= 0)
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.InvalidInput, false);

        var request = await Context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

        if (request == null)
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.NotFound, false);

        request.MapFromUpdateDto(updateDto);
        Context.ServiceRequests.Update(request);
        await Context.SaveChangesAsync();

        var dto = request.MapToDto();
        return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.Updated, true, dto);
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        var request = await Context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == id);

        if (request == null)
            return new DataServiceResult<bool>(OperationStatus.NotFound, false);

        Context.ServiceRequests.Remove(request);
        await Context.SaveChangesAsync();

        return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
    }
}
