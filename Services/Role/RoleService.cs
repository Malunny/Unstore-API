using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.Role;

public class RoleService : BaseService
{
    public RoleService(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IServiceResult<List<RoleReadDto>>> GetAllAsync()
    {
        try
        {
            var roles = await Context.Roles.AsNoTracking().ToListAsync();
            var dtos = Mapper.Map<List<RoleReadDto>>(roles);
            return new DataServiceResult<List<RoleReadDto>>(true, dtos);
        }
        catch (Exception)
        {
            return new DataServiceResult<List<RoleReadDto>>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<RoleReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<RoleReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var role = await Context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (role == null)
                return new DataServiceResult<RoleReadDto>(OperationStatus.NotFound, false);

            var dto = Mapper.Map<RoleReadDto>(role);
            return new DataServiceResult<RoleReadDto>(true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<RoleReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<RoleReadDto>> CreateAsync(RoleCreateDto createDto)
    {
        if (createDto == null)
            return new DataServiceResult<RoleReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var roleExists = await Context.Roles.AnyAsync(x => x.Name == createDto.Name);
            if (roleExists)
                return new DataServiceResult<RoleReadDto>(OperationStatus.ValidationError, false);

            var role = Mapper.Map<Models.Role>(createDto);

            await Context.Roles.AddAsync(role);
            await Context.SaveChangesAsync();

            var dto = Mapper.Map<RoleReadDto>(role);
            return new DataServiceResult<RoleReadDto>(OperationStatus.Created, true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<RoleReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<RoleReadDto>> UpdateAsync(RoleUpdateDto updateDto)
    {
        if (updateDto == null || updateDto.Id <= 0)
            return new DataServiceResult<RoleReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var role = await Context.Roles.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

            if (role == null)
                return new DataServiceResult<RoleReadDto>(OperationStatus.NotFound, false);

            if (updateDto.Name != null && updateDto.Name != role.Name)
            {
                var nameExists = await Context.Roles.AnyAsync(x => x.Name == updateDto.Name && x.Id != updateDto.Id);
                if (nameExists)
                    return new DataServiceResult<RoleReadDto>(OperationStatus.ValidationError, false);
            }

            Mapper.Map(updateDto, role);
            Context.Roles.Update(role);
            await Context.SaveChangesAsync();

            var dto = Mapper.Map<RoleReadDto>(role);
            return new DataServiceResult<RoleReadDto>(OperationStatus.Updated, true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<RoleReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        try
        {
            var role = await Context.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (role == null)
                return new DataServiceResult<bool>(OperationStatus.NotFound, false);

            Context.Roles.Remove(role);
            await Context.SaveChangesAsync();

            return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
        }
        catch (Exception)
        {
            return new DataServiceResult<bool>(OperationStatus.InternalServerError, false);
        }
    }
}
