using System.Linq;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.DTOs.Mapping;
using Unstore.Models;

namespace Unstore.Services.User;

public class AddressService : BaseService
{
    public AddressService(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IServiceResult<List<UserAddressReadDto>>> GetAllAsync()
    {
        var addresses = await Context.Addresses.AsNoTracking().ToListAsync();
        var dtos = addresses.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<UserAddressReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<UserAddressReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<UserAddressReadDto>(OperationStatus.InvalidInput, false);

        var address = await Context.Addresses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (address == null)
            return new DataServiceResult<UserAddressReadDto>(OperationStatus.NotFound, false);

        var dto = address.MapToDto();
        return new DataServiceResult<UserAddressReadDto>(true, dto);
    }

    public async Task<IServiceResult<List<UserAddressReadDto>>> GetByUserIdAsync(int userId)
    {
        if (userId <= 0)
            return new DataServiceResult<List<UserAddressReadDto>>(OperationStatus.InvalidInput, false);

        var addresses = await Context.Addresses
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync();

        var dtos = addresses.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<UserAddressReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<UserAddressReadDto>> CreateAsync(UserAddressCreateDto createDto)
    {
        if (createDto.UserId <= 0 || createDto.TypeId <= 0)
            return new DataServiceResult<UserAddressReadDto>(OperationStatus.InvalidInput, false);

        var userExists = await Context.Users.AnyAsync(x => x.Id == createDto.UserId);
        if (!userExists)
            return new DataServiceResult<UserAddressReadDto>(OperationStatus.NotFound, false);

        var typeExists = await Context.AddressTypes.AnyAsync(x => x.Id == createDto.TypeId);
        if (!typeExists)
            return new DataServiceResult<UserAddressReadDto>(OperationStatus.NotFound, false);

        var address = createDto.MapToModel();

        await Context.Addresses.AddAsync(address);
        await Context.SaveChangesAsync();

        var dto = address.MapToDto();
        return new DataServiceResult<UserAddressReadDto>(OperationStatus.Created, true, dto);
    }

    public async Task<IServiceResult<UserAddressReadDto>> UpdateAsync(UserAddressUpdateDto updateDto)
    {
        if (updateDto.Id <= 0)
            return new DataServiceResult<UserAddressReadDto>(OperationStatus.InvalidInput, false);

        var address = await Context.Addresses.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

        if (address == null)
            return new DataServiceResult<UserAddressReadDto>(OperationStatus.NotFound, false);

        if (string.IsNullOrWhiteSpace(updateDto.AddressTypeKey))
            return new DataServiceResult<UserAddressReadDto>(OperationStatus.InvalidInput, false);
        
        var typeExists = await Context.AddressTypes.AnyAsync(x => x.Key == updateDto.AddressTypeKey);
        if (!typeExists)
            return new DataServiceResult<UserAddressReadDto>(OperationStatus.NotFound, false);

        address.MapFromUpdateDto(updateDto);
        Context.Addresses.Update(address);
        await Context.SaveChangesAsync();
        var dto = address.MapToDto();
        return new DataServiceResult<UserAddressReadDto>(OperationStatus.Updated, true, dto);
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        var address = await Context.Addresses.FirstOrDefaultAsync(x => x.Id == id);

        if (address == null)
            return new DataServiceResult<bool>(OperationStatus.NotFound, false);

        Context.Addresses.Remove(address);
        await Context.SaveChangesAsync();

        return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
    }
}
