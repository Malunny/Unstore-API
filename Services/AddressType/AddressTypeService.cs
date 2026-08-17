using System.Linq;
using Unstore.DTOs.Mapping;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.AddressType;

public class AddressTypeService : BaseService
{
    public AddressTypeService(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IServiceResult<List<AddressTypeReadDto>>> GetAllAsync()
    {
        var addressTypes = await Context.AddressTypes.AsNoTracking().ToListAsync();
        var dtos = addressTypes.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<AddressTypeReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<AddressTypeReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<AddressTypeReadDto>(OperationStatus.InvalidInput, false);

        var addressType = await Context.AddressTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (addressType == null)
            return new DataServiceResult<AddressTypeReadDto>(OperationStatus.NotFound, false);

        var dto = addressType.MapToDto();
        return new DataServiceResult<AddressTypeReadDto>(true, dto);
    }

    public async Task<IServiceResult<AddressTypeReadDto>> CreateAsync(AddressTypeCreateDto createDto)
    {
        if (createDto == null)
            return new DataServiceResult<AddressTypeReadDto>(OperationStatus.InvalidInput, false);

        var keyExists = await Context.AddressTypes.AnyAsync(x => x.Key == createDto.Key);
        if (keyExists)
            return new DataServiceResult<AddressTypeReadDto>(OperationStatus.ValidationError, false);

        var addressType = createDto.MapToModel();

        await Context.AddressTypes.AddAsync(addressType);
        await Context.SaveChangesAsync();

        var dto = addressType.MapToDto();
        return new DataServiceResult<AddressTypeReadDto>(OperationStatus.Created, true, dto);
    }

    public async Task<IServiceResult<AddressTypeReadDto>> UpdateAsync(AddressTypeUpdateDto updateDto)
    {
        if (updateDto == null || updateDto.Id <= 0)
            return new DataServiceResult<AddressTypeReadDto>(OperationStatus.InvalidInput, false);

        var addressType = await Context.AddressTypes.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

        if (addressType == null)
            return new DataServiceResult<AddressTypeReadDto>(OperationStatus.NotFound, false);

        if (updateDto.Key != null && updateDto.Key != addressType.Key)
        {
            var keyExists = await Context.AddressTypes.AnyAsync(x => x.Key == updateDto.Key && x.Id != updateDto.Id);
            if (keyExists)
                return new DataServiceResult<AddressTypeReadDto>(OperationStatus.ValidationError, false);
        }

        addressType.MapFromUpdateDto(updateDto);
        Context.AddressTypes.Update(addressType);
        await Context.SaveChangesAsync();

        var dto = addressType.MapToDto();
        return new DataServiceResult<AddressTypeReadDto>(OperationStatus.Updated, true, dto);
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        var addressType = await Context.AddressTypes.FirstOrDefaultAsync(x => x.Id == id);

        if (addressType == null)
            return new DataServiceResult<bool>(OperationStatus.NotFound, false);

        Context.AddressTypes.Remove(addressType);
        await Context.SaveChangesAsync();

        return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
    }
}
