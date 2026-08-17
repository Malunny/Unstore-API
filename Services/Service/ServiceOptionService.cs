using System.Linq;
using Unstore.DTOs.Mapping;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.Service;

public class ServiceOptionService : BaseService
{
    public ServiceOptionService(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IServiceResult<List<ServiceServiceOptionReadDto>>> GetAllAsync()
    {
        var options = await Context.ServiceOptions.AsNoTracking().ToListAsync();
        var dtos = options.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<ServiceServiceOptionReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<ServiceServiceOptionReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<ServiceServiceOptionReadDto>(OperationStatus.InvalidInput, false);

        var option = await Context.ServiceOptions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (option == null)
            return new DataServiceResult<ServiceServiceOptionReadDto>(OperationStatus.NotFound, false);

        var dto = option.MapToDto();
        return new DataServiceResult<ServiceServiceOptionReadDto>(true, dto);
    }

    public async Task<IServiceResult<List<ServiceServiceOptionReadDto>>> GetByServiceIdAsync(int serviceId)
    {
        if (serviceId <= 0)
            return new DataServiceResult<List<ServiceServiceOptionReadDto>>(OperationStatus.InvalidInput, false);

        var options = await Context.ServiceOptions
            .AsNoTracking()
            .Where(x => x.ServiceId == serviceId)
            .ToListAsync();

        var dtos = options.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<ServiceServiceOptionReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<ServiceServiceOptionReadDto>> CreateAsync(ServiceServiceOptionCreateDto createDto)
    {
        if (createDto == null || createDto.ServiceId <= 0)
            return new DataServiceResult<ServiceServiceOptionReadDto>(OperationStatus.InvalidInput, false);

        var serviceExists = await Context.Services.AnyAsync(x => x.Id == createDto.ServiceId);
        if (!serviceExists)
            return new DataServiceResult<ServiceServiceOptionReadDto>(OperationStatus.NotFound, false);

        var option = createDto.MapToModel();

        await Context.ServiceOptions.AddAsync(option);
        await Context.SaveChangesAsync();

        var dto = option.MapToDto();
        return new DataServiceResult<ServiceServiceOptionReadDto>(OperationStatus.Created, true, dto);
    }

    public async Task<IServiceResult<ServiceServiceOptionReadDto>> UpdateAsync(ServiceServiceOptionUpdateDto updateDto)
    {
        if (updateDto == null || updateDto.Id <= 0)
            return new DataServiceResult<ServiceServiceOptionReadDto>(OperationStatus.InvalidInput, false);

        var option = await Context.ServiceOptions.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

        if (option == null)
            return new DataServiceResult<ServiceServiceOptionReadDto>(OperationStatus.NotFound, false);

        option.MapFromUpdateDto(updateDto);
        Context.ServiceOptions.Update(option);
        await Context.SaveChangesAsync();

        var dto = option.MapToDto();
        return new DataServiceResult<ServiceServiceOptionReadDto>(OperationStatus.Updated, true, dto);
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        var option = await Context.ServiceOptions.FirstOrDefaultAsync(x => x.Id == id);

        if (option == null)
            return new DataServiceResult<bool>(OperationStatus.NotFound, false);

        Context.ServiceOptions.Remove(option);
        await Context.SaveChangesAsync();

        return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
    }
}
