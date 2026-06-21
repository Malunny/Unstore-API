using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.AddressType;

public class AddressTypeService : BaseService
{
    public AddressTypeService(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IServiceResult<List<AddressTypeReadDto>>> GetAllAsync()
    {
        try
        {
            var addressTypes = await Context.AddressTypes.AsNoTracking().ToListAsync();
            var dtos = Mapper.Map<List<AddressTypeReadDto>>(addressTypes);
            return new DataServiceResult<List<AddressTypeReadDto>>(true, dtos);
        }
        catch (Exception)
        {
            return new DataServiceResult<List<AddressTypeReadDto>>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<AddressTypeReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<AddressTypeReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var addressType = await Context.AddressTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (addressType == null)
                return new DataServiceResult<AddressTypeReadDto>(OperationStatus.NotFound, false);

            var dto = Mapper.Map<AddressTypeReadDto>(addressType);
            return new DataServiceResult<AddressTypeReadDto>(true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<AddressTypeReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<AddressTypeReadDto>> CreateAsync(AddressTypeCreateDto createDto)
    {
        if (createDto == null)
            return new DataServiceResult<AddressTypeReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var keyExists = await Context.AddressTypes.AnyAsync(x => x.Key == createDto.Key);
            if (keyExists)
                return new DataServiceResult<AddressTypeReadDto>(OperationStatus.ValidationError, false);

            var addressType = Mapper.Map<Models.AddressType>(createDto);

            await Context.AddressTypes.AddAsync(addressType);
            await Context.SaveChangesAsync();

            var dto = Mapper.Map<AddressTypeReadDto>(addressType);
            return new DataServiceResult<AddressTypeReadDto>(OperationStatus.Created, true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<AddressTypeReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<AddressTypeReadDto>> UpdateAsync(AddressTypeUpdateDto updateDto)
    {
        if (updateDto == null || updateDto.Id <= 0)
            return new DataServiceResult<AddressTypeReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var addressType = await Context.AddressTypes.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

            if (addressType == null)
                return new DataServiceResult<AddressTypeReadDto>(OperationStatus.NotFound, false);

            if (updateDto.Key != null && updateDto.Key != addressType.Key)
            {
                var keyExists = await Context.AddressTypes.AnyAsync(x => x.Key == updateDto.Key && x.Id != updateDto.Id);
                if (keyExists)
                    return new DataServiceResult<AddressTypeReadDto>(OperationStatus.ValidationError, false);
            }

            Mapper.Map(updateDto, addressType);
            Context.AddressTypes.Update(addressType);
            await Context.SaveChangesAsync();

            var dto = Mapper.Map<AddressTypeReadDto>(addressType);
            return new DataServiceResult<AddressTypeReadDto>(OperationStatus.Updated, true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<AddressTypeReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        try
        {
            var addressType = await Context.AddressTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (addressType == null)
                return new DataServiceResult<bool>(OperationStatus.NotFound, false);

            Context.AddressTypes.Remove(addressType);
            await Context.SaveChangesAsync();

            return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
        }
        catch (Exception)
        {
            return new DataServiceResult<bool>(OperationStatus.InternalServerError, false);
        }
    }
}
