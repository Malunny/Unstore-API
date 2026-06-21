using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.Service;

public class ServiceRequestService : BaseService
{
    public ServiceRequestService(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IServiceResult<List<ServiceServiceRequestReadDto>>> GetAllAsync()
    {
        try
        {
            var requests = await Context.ServiceRequests.AsNoTracking().ToListAsync();
            var dtos = Mapper.Map<List<ServiceServiceRequestReadDto>>(requests);
            return new DataServiceResult<List<ServiceServiceRequestReadDto>>(true, dtos);
        }
        catch (Exception)
        {
            return new DataServiceResult<List<ServiceServiceRequestReadDto>>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<ServiceServiceRequestReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var request = await Context.ServiceRequests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
                return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.NotFound, false);

            var dto = Mapper.Map<ServiceServiceRequestReadDto>(request);
            return new DataServiceResult<ServiceServiceRequestReadDto>(true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<List<ServiceServiceRequestReadDto>>> GetByServiceIdAsync(int serviceId)
    {
        if (serviceId <= 0)
            return new DataServiceResult<List<ServiceServiceRequestReadDto>>(OperationStatus.InvalidInput, false);

        try
        {
            var requests = await Context.ServiceRequests
                .AsNoTracking()
                .Where(x => x.ServiceId == serviceId)
                .ToListAsync();

            var dtos = Mapper.Map<List<ServiceServiceRequestReadDto>>(requests);
            return new DataServiceResult<List<ServiceServiceRequestReadDto>>(true, dtos);
        }
        catch (Exception)
        {
            return new DataServiceResult<List<ServiceServiceRequestReadDto>>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<List<ServiceServiceRequestReadDto>>> GetByRequesterIdAsync(int requesterId)
    {
        if (requesterId <= 0)
            return new DataServiceResult<List<ServiceServiceRequestReadDto>>(OperationStatus.InvalidInput, false);

        try
        {
            var requests = await Context.ServiceRequests
                .AsNoTracking()
                .Where(x => x.RequesterId == requesterId)
                .ToListAsync();

            var dtos = Mapper.Map<List<ServiceServiceRequestReadDto>>(requests);
            return new DataServiceResult<List<ServiceServiceRequestReadDto>>(true, dtos);
        }
        catch (Exception)
        {
            return new DataServiceResult<List<ServiceServiceRequestReadDto>>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<ServiceServiceRequestReadDto>> CreateAsync(ServiceServiceRequestCreateDto createDto)
    {
        if (createDto == null || createDto.ServiceId <= 0 || createDto.RequesterId <= 0)
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var serviceExists = await Context.Services.AnyAsync(x => x.Id == createDto.ServiceId);
            if (!serviceExists)
                return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.NotFound, false);

            var requesterExists = await Context.Users.AnyAsync(x => x.Id == createDto.RequesterId);
            if (!requesterExists)
                return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.NotFound, false);

            var request = Mapper.Map<ServiceRequest>(createDto);
            request.RequestedAt = DateTime.Now;

            await Context.ServiceRequests.AddAsync(request);
            await Context.SaveChangesAsync();

            var dto = Mapper.Map<ServiceServiceRequestReadDto>(request);
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.Created, true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<ServiceServiceRequestReadDto>> UpdateAsync(ServiceServiceRequestUpdateDto updateDto)
    {
        if (updateDto == null || updateDto.Id <= 0)
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var request = await Context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

            if (request == null)
                return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.NotFound, false);

            Mapper.Map(updateDto, request);
            Context.ServiceRequests.Update(request);
            await Context.SaveChangesAsync();

            var dto = Mapper.Map<ServiceServiceRequestReadDto>(request);
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.Updated, true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<ServiceServiceRequestReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        try
        {
            var request = await Context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
                return new DataServiceResult<bool>(OperationStatus.NotFound, false);

            Context.ServiceRequests.Remove(request);
            await Context.SaveChangesAsync();

            return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
        }
        catch (Exception)
        {
            return new DataServiceResult<bool>(OperationStatus.InternalServerError, false);
        }
    }
}
