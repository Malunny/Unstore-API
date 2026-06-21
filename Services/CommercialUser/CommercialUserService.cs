using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.CommercialUser;

public class CommercialUserService : BaseService
{
    public CommercialUserService(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IServiceResult<List<CommercialUserReadDto>>> GetAllAsync()
    {
        try
        {
            var commercialUsers = await Context.CommercialUsers
                .AsNoTracking()
                .Where(x => x.Active)
                .ToListAsync();
            var dtos = Mapper.Map<List<CommercialUserReadDto>>(commercialUsers);
            return new DataServiceResult<List<CommercialUserReadDto>>(true, dtos);
        }
        catch (Exception)
        {
            return new DataServiceResult<List<CommercialUserReadDto>>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<CommercialUserReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<CommercialUserReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var commercialUser = await Context.CommercialUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (commercialUser == null)
                return new DataServiceResult<CommercialUserReadDto>(OperationStatus.NotFound, false);

            var dto = Mapper.Map<CommercialUserReadDto>(commercialUser);
            return new DataServiceResult<CommercialUserReadDto>(true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<CommercialUserReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<CommercialUserReadDto>> CreateAsync(CommercialUserCreateDto createDto)
    {
        if (createDto == null || createDto.OriginalUserId <= 0)
            return new DataServiceResult<CommercialUserReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var userExists = await Context.Users.AnyAsync(x => x.Id == createDto.OriginalUserId);
            if (!userExists)
                return new DataServiceResult<CommercialUserReadDto>(OperationStatus.NotFound, false);

            var commercialUserExists = await Context.CommercialUsers
                .AnyAsync(x => x.OriginalUserId == createDto.OriginalUserId);
            if (commercialUserExists)
                return new DataServiceResult<CommercialUserReadDto>(OperationStatus.ValidationError, false);

            var commercialUser = Mapper.Map<Models.CommercialUser>(createDto);
            commercialUser.Active = true;

            await Context.CommercialUsers.AddAsync(commercialUser);
            await Context.SaveChangesAsync();

            var dto = Mapper.Map<CommercialUserReadDto>(commercialUser);
            return new DataServiceResult<CommercialUserReadDto>(OperationStatus.Created, true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<CommercialUserReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<CommercialUserReadDto>> UpdateAsync(CommercialUserUpdateDto updateDto)
    {
        if (updateDto == null || updateDto.Id <= 0)
            return new DataServiceResult<CommercialUserReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var commercialUser = await Context.CommercialUsers.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

            if (commercialUser == null)
                return new DataServiceResult<CommercialUserReadDto>(OperationStatus.NotFound, false);

            Mapper.Map(updateDto, commercialUser);
            Context.CommercialUsers.Update(commercialUser);
            await Context.SaveChangesAsync();

            var dto = Mapper.Map<CommercialUserReadDto>(commercialUser);
            return new DataServiceResult<CommercialUserReadDto>(OperationStatus.Updated, true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<CommercialUserReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        try
        {
            var commercialUser = await Context.CommercialUsers.FirstOrDefaultAsync(x => x.Id == id);

            if (commercialUser == null)
                return new DataServiceResult<bool>(OperationStatus.NotFound, false);

            Context.CommercialUsers.Remove(commercialUser);
            await Context.SaveChangesAsync();

            return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
        }
        catch (Exception)
        {
            return new DataServiceResult<bool>(OperationStatus.InternalServerError, false);
        }
    }
}
