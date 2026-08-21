using System.Linq;
using Unstore.DTOs.Mapping;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.Service;

public class ServiceService : BaseService
{
    public ServiceService(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IServiceResult<List<ServiceReadDto>>> GetAllAsync()
    {
        var services = await Context.Services
            .AsNoTracking()
            .Where(x => x.Active)
            .ToListAsync();
        var dtos = services.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<ServiceReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<ServiceReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<ServiceReadDto>(OperationStatus.InvalidInput, false);

        var service = await Context.Services.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (service == null)
            return new DataServiceResult<ServiceReadDto>(OperationStatus.NotFound, false);

        var dto = service.MapToDto();
        return new DataServiceResult<ServiceReadDto>(true, dto);
    }

    public async Task<IServiceResult<List<ServiceReadDto>>> GetByProviderIdAsync(int providerId)
    {
        if (providerId <= 0)
            return new DataServiceResult<List<ServiceReadDto>>(OperationStatus.InvalidInput, false);

        var services = await Context.Services
            .AsNoTracking()
            .Where(x => x.ProviderId == providerId && x.Active)
            .ToListAsync();

        var dtos = services.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<ServiceReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<ServiceReadDto>> CreateAsync(ServiceCreateDto createDto)
    {
        if (createDto.ProviderId <= 0)
            return new DataServiceResult<ServiceReadDto>(OperationStatus.InvalidInput, false);

        var providerExists = await Context.CommercialUsers.AnyAsync(x => x.Id == createDto.ProviderId);
        if (!providerExists)
            return new DataServiceResult<ServiceReadDto>(OperationStatus.NotFound, false);

        var service = createDto.MapToModel();
        service.Active = true;

        await Context.Services.AddAsync(service);
        await Context.SaveChangesAsync();

        var dto = service.MapToDto();
        return new DataServiceResult<ServiceReadDto>(OperationStatus.Created, true, dto);
    }

    public async Task<IServiceResult<ServiceReadDto>> UpdateAsync(ServiceUpdateDto updateDto)
    {
        if (updateDto.Id <= 0)
            return new DataServiceResult<ServiceReadDto>(OperationStatus.InvalidInput, false);

        var service = await Context.Services.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

        if (service == null)
            return new DataServiceResult<ServiceReadDto>(OperationStatus.NotFound, false);

        service.MapFromUpdateDto(updateDto);
        Context.Services.Update(service);
        await Context.SaveChangesAsync();

        var dto = service.MapToDto();
        return new DataServiceResult<ServiceReadDto>(OperationStatus.Updated, true, dto);
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        var service = await Context.Services.FirstOrDefaultAsync(x => x.Id == id);

        if (service == null)
            return new DataServiceResult<bool>(OperationStatus.NotFound, false);

        Context.Services.Remove(service);
        await Context.SaveChangesAsync();

        return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
    }
}
