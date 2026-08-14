using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Extensions;
using Unstore.Models;

namespace Unstore.Services.Account;

public partial class AccountService : BaseService
{
    private readonly ITokenService _tokenService;
    private readonly IServiceResultFactory _serviceResultFactory;
    // Returns the user token
    public AccountService(AppDbContext dbContext, IMapper mapper, 
        ITokenService tokenService, IServiceResultFactory serviceProviderFactoryProvider)
        : base(dbContext, mapper)
    {
        _tokenService = tokenService;
        _serviceResultFactory = serviceProviderFactoryProvider;
    }

    public async Task<IServiceResult<string>> TryLoginAsync(UserLoginDto userLogin)
    {
        var user = await Context.Users
            .Include(user => user.Roles).AsNoTracking().FirstOrDefaultAsync(x => x.Email == userLogin.Email);

        if (user == null)
            return _serviceResultFactory.Failure<string>(OperationStatus.NotFound);
        
        bool match = BCrypt.Net.BCrypt.Verify(userLogin.Password, user.PasswordHash);

        if (!match)
            return _serviceResultFactory.Failure<string>(OperationStatus.InvalidLogin);

        var token = _tokenService.GenerateToken(user);
        
        return _serviceResultFactory.Success(token);
    }

    public async Task<IServiceResult<bool>> TryRegisterAsync(UserCreateDtos userRegister)
    {
        bool exists = Context.Users.Any(x => userRegister.Username == x.Username || userRegister.Email == x.Email);

        if (exists)
            return _serviceResultFactory.Failure<bool>(OperationStatus.UserAlreadyExists);

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegister.Password);
        var userToCreate = Mapper.Map<UserCreateDtos, Models.User>(userRegister);
        var normalUserRole =
            await Context.Roles.FirstAsync(role => role.Name == Configuration.RoleName(RolesNames.Normal));

        userToCreate.PasswordHash = hashedPassword;
        userToCreate.Active = true;
        userToCreate.Roles = new List<Models.Role> {
            normalUserRole
        };

    await Context.Users.AddAsync(userToCreate);
        await Context.SaveChangesAsync();
        
        return _serviceResultFactory.Success(OperationStatus.Created, true);
    }

    public async Task<IServiceResult<bool>> ChangePasswordAsync(string password, string username)
    {
        var user = await Context.Users
            .FirstOrDefaultAsync(x => x.Username == username);
        
        if (user == null)
            return  _serviceResultFactory.Failure<bool>(OperationStatus.NotFound);
        
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        
        await Context.SaveChangesAsync();
        
        return _serviceResultFactory.Success(true);
    }

    public async Task<IServiceResult<CommercialUserCreateDto>> AddCommercialUserAsync(CommercialUserCreateDto dto, string username)
    {
        var user = await Context.Users.FirstOrDefaultAsync(x => x.Username == username);
        if (user == null)
            return _serviceResultFactory.Failure<CommercialUserCreateDto>(OperationStatus.NotFound);
        dto.OriginalUserId = user.Id;
        
        var commercialUser = Mapper.Map<Models.CommercialUser>(dto);
        commercialUser.OriginalUserId = user.Id;
        
        await Context.CommercialUsers.AddAsync(commercialUser);
        await Context.SaveChangesAsync();
        return _serviceResultFactory.Success(dto);
    }

    public async Task<IServiceResult<UserUpdateDto>> UpdateUserAsync(UserUpdateDto dto,
        string username)
    {
        var user = await Context.Users.FirstOrDefaultAsync(x => x.Username == username);
        if (user == null)
            return _serviceResultFactory.Failure<UserUpdateDto>(OperationStatus.NotFound);
        
        bool emailExists = await Context.Users.AnyAsync(x => x.Email == dto.Email);
        if (emailExists)
            return _serviceResultFactory.Failure<UserUpdateDto>(OperationStatus.UserAlreadyExists);

        user.Name = dto.Name;
        user.Email = dto.Email;

        return _serviceResultFactory.Success(dto);
    }
}