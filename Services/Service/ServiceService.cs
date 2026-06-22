using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.Service;

public class ServiceService : BaseService
{
    public ServiceService(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IServiceResult<List<ServiceReadDto>>> GetAllAsync()
    {
        var services = await Context.Services
            .AsNoTracking()
            .Where(x => x.Active)
            .ToListAsync();
        var dtos = Mapper.Map<List<ServiceReadDto>>(services);
        return new DataServiceResult<List<ServiceReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<ServiceReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<ServiceReadDto>(OperationStatus.InvalidInput, false);

        var service = await Context.Services.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (service == null)
            return new DataServiceResult<ServiceReadDto>(OperationStatus.NotFound, false);

        var dto = Mapper.Map<ServiceReadDto>(service);
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

        var dtos = Mapper.Map<List<ServiceReadDto>>(services);
        return new DataServiceResult<List<ServiceReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<ServiceReadDto>> CreateAsync(ServiceCreateDto createDto)
    {
        if (createDto == null || createDto.ProviderId <= 0)
            return new DataServiceResult<ServiceReadDto>(OperationStatus.InvalidInput, false);

        var providerExists = await Context.CommercialUsers.AnyAsync(x => x.Id == createDto.ProviderId);
        if (!providerExists)
            return new DataServiceResult<ServiceReadDto>(OperationStatus.NotFound, false);

        var service = Mapper.Map<Models.Service>(createDto);
        service.Active = true;

        await Context.Services.AddAsync(service);
        await Context.SaveChangesAsync();

        var dto = Mapper.Map<ServiceReadDto>(service);
        return new DataServiceResult<ServiceReadDto>(OperationStatus.Created, true, dto);
    }

    public async Task<IServiceResult<ServiceReadDto>> UpdateAsync(ServiceUpdateDto updateDto)
    {
        if (updateDto == null || updateDto.Id <= 0)
            return new DataServiceResult<ServiceReadDto>(OperationStatus.InvalidInput, false);

        var service = await Context.Services.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

        if (service == null)
            return new DataServiceResult<ServiceReadDto>(OperationStatus.NotFound, false);

        Mapper.Map(updateDto, service);
        Context.Services.Update(service);
        await Context.SaveChangesAsync();

        var dto = Mapper.Map<ServiceReadDto>(service);
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
