using System.Linq;
using Unstore.DTOs.Mapping;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.Role;

public class RoleService : BaseService
{
    public RoleService(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IServiceResult<List<RoleReadDto>>> GetAllAsync()
    {
        var roles = await Context.Roles.AsNoTracking().ToListAsync();
        var dtos = roles.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<RoleReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<RoleReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<RoleReadDto>(OperationStatus.InvalidInput, false);

        var role = await Context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (role == null)
            return new DataServiceResult<RoleReadDto>(OperationStatus.NotFound, false);

        var dto = role.MapToDto();
        return new DataServiceResult<RoleReadDto>(true, dto);
    }

    public async Task<IServiceResult<RoleReadDto>> CreateAsync(RoleCreateDto createDto)
    {
        if (createDto == null)
            return new DataServiceResult<RoleReadDto>(OperationStatus.InvalidInput, false);

        var roleExists = await Context.Roles.AnyAsync(x => x.Name == createDto.Name);
        if (roleExists)
            return new DataServiceResult<RoleReadDto>(OperationStatus.ValidationError, false);

        var role = createDto.MapToModel();

        await Context.Roles.AddAsync(role);
        await Context.SaveChangesAsync();

        var dto = role.MapToDto();
        return new DataServiceResult<RoleReadDto>(OperationStatus.Created, true, dto);
    }

    public async Task<IServiceResult<RoleReadDto>> UpdateAsync(RoleUpdateDto updateDto)
    {
        if (updateDto == null || updateDto.Id <= 0)
            return new DataServiceResult<RoleReadDto>(OperationStatus.InvalidInput, false);

        var role = await Context.Roles.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

        if (role == null)
            return new DataServiceResult<RoleReadDto>(OperationStatus.NotFound, false);

        if (updateDto.Name != null && updateDto.Name != role.Name)
        {
            var nameExists = await Context.Roles.AnyAsync(x => x.Name == updateDto.Name && x.Id != updateDto.Id);
            if (nameExists)
                return new DataServiceResult<RoleReadDto>(OperationStatus.ValidationError, false);
        }

        role.MapFromUpdateDto(updateDto);
        Context.Roles.Update(role);
        await Context.SaveChangesAsync();

        var dto = role.MapToDto();
        return new DataServiceResult<RoleReadDto>(OperationStatus.Updated, true, dto);
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        var role = await Context.Roles.FirstOrDefaultAsync(x => x.Id == id);

        if (role == null)
            return new DataServiceResult<bool>(OperationStatus.NotFound, false);

        Context.Roles.Remove(role);
        await Context.SaveChangesAsync();

        return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
    }
}
