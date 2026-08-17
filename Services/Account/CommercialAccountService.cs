using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.DTOs.Mapping;

namespace Unstore.Services.Account;

public class CommercialAccountService([FromServices] AppDbContext context, [FromServices] IServiceResultFactory serviceResultFactory)
{
    public async Task<IServiceResult<CommercialUserReadDto>> GetOwnCommercialAccountAsync(string username)
    {
        var userIdUsername = await context.Users
            .AsNoTracking()
            .Select(x => new {x.Id, x.Username})
            .FirstOrDefaultAsync(u => u.Username == username);
        
        if (userIdUsername is null)
            return serviceResultFactory.Failure<CommercialUserReadDto>(OperationStatus.NotFound);
        
        var data = await context.CommercialUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.OriginalUserId == userIdUsername.Id);

        if (data is null)
            return serviceResultFactory.Failure<CommercialUserReadDto>(OperationStatus.NotFound);

        var dto = data.MapToDto();

        return serviceResultFactory.Success(dto);
    }
    public async Task<IServiceResult<CommercialUserCreateDto>> TryRegisterCommercialAccountAsync(CommercialUserCreateDto dto, 
        string username)
    {
        var user = await context.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username);
        
        if (user is null) 
            return serviceResultFactory.Failure<CommercialUserCreateDto>(OperationStatus.NotFound);
        
        if (await context.CommercialUsers.AnyAsync(commercialUser => commercialUser.OriginalUserId == user.Id))
            return  serviceResultFactory.Failure<CommercialUserCreateDto>(OperationStatus.Unauthorized);
        
        var sellerRole = await context.Roles.FirstAsync(x => x.Name == Configuration.RoleName(RolesNames.Seller));
        user.Roles.Add(sellerRole);
        
        var commercialUser = dto.MapToModel();
        
        commercialUser.OriginalUserId = user.Id;
        dto.OriginalUserId = commercialUser.Id;

        await context.CommercialUsers.AddAsync(commercialUser);
        await context.SaveChangesAsync();

        return serviceResultFactory.Success(dto);
    }
}