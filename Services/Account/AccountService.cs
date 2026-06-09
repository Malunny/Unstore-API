using AutoMapper;
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
    private readonly IServiceResultFactoryProvider _serviceResultFactoryProvider;
    // Returns the user token
    public AccountService(AppDbContext dbContext, IMapper mapper, 
        ITokenService tokenService, IServiceResultFactoryProvider serviceProviderFactoryProvider)
        : base(dbContext, mapper)
    {
        _tokenService = tokenService;
        _serviceResultFactoryProvider = serviceProviderFactoryProvider;
    }

    public async Task<IServiceResult<string>> TryLoginAsync(UserLoginDto userLogin, ModelStateDictionary modelState)
    {
        var serviceResultFactory = _serviceResultFactoryProvider.Create<string>();
        if (!modelState.IsValid)
            return serviceResultFactory.Failure(OperationStatus.InvalidLogin);
        
        var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == userLogin.Email);
        
        if (user == null)
            return serviceResultFactory.Failure(OperationStatus.NotFound);
        
        bool match = BCrypt.Net.BCrypt.Verify(userLogin.Password, user.PasswordHash);

        if (!match)
            return serviceResultFactory.Failure(OperationStatus.InvalidLogin);
        
        var token = _tokenService.GenerateToken(user);
        
        return serviceResultFactory.Success(token);
    }

    public async Task<IServiceResult<bool>> TryRegisterAsync(UserCreationDto userRegister, ModelStateDictionary modelState)
    {
        var serviceResultFactory = _serviceResultFactoryProvider.Create<bool>();
        
        if (!modelState.IsValid)
            return serviceResultFactory.Failure(OperationStatus.InvalidInput);

        bool exists = Context.Users.Any(x => userRegister.Username == x.Username || userRegister.Email == x.Email);

        if (exists)
            return serviceResultFactory.Failure(OperationStatus.UserAlreadyExists);
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegister.Password);
        var userToCreate = Mapper.Map<UserCreationDto, User>(userRegister);
        userToCreate.PasswordHash = hashedPassword;

        await Context.Users.AddAsync(userToCreate);
        await Context.SaveChangesAsync();
        
        return serviceResultFactory.Success(OperationStatus.Created, true);
    }
}