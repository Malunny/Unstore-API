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
    private readonly IServiceResultFactory _serviceResultFactory;
    // Returns the user token
    public AccountService(AppDbContext dbContext, IMapper mapper, 
        ITokenService tokenService, IServiceResultFactory serviceProviderFactoryProvider)
        : base(dbContext, mapper)
    {
        _tokenService = tokenService;
        _serviceResultFactory = serviceProviderFactoryProvider;
    }

    public async Task<IServiceResult<string>> TryLoginAsync(UserLoginDto userLogin, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return _serviceResultFactory.Failure<string>(OperationStatus.InvalidLogin);
        
        var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == userLogin.Email);
        
        if (user == null)
            return _serviceResultFactory.Failure<string>(OperationStatus.NotFound);
        
        bool match = BCrypt.Net.BCrypt.Verify(userLogin.Password, user.PasswordHash);

        if (!match)
            return _serviceResultFactory.Failure<string>(OperationStatus.InvalidLogin);
        
        var token = _tokenService.GenerateToken(user);
        
        return _serviceResultFactory.Success(token);
    }

    public async Task<IServiceResult<bool>> TryRegisterAsync(UserCreateDtos userRegister, ModelStateDictionary modelState)
    {
        Console.WriteLine("--------------------------------------------------- 0");
        if (!modelState.IsValid)
        {
            Console.WriteLine(modelState.Values);
            return _serviceResultFactory.Failure<bool>(OperationStatus.InvalidInput);
        }
            
        Console.WriteLine("--------------------------------------------------- 1");
        bool exists = Context.Users.Any(x => userRegister.Username == x.Username || userRegister.Email == x.Email);
        
        if (exists)
            return _serviceResultFactory.Failure<bool>(OperationStatus.UserAlreadyExists);
        Console.WriteLine("--------------------------------------------------- 3");
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegister.Password);
        var userToCreate = Mapper.Map<UserCreateDtos, Models.User>(userRegister);
        userToCreate.PasswordHash = hashedPassword;

        await Context.Users.AddAsync(userToCreate);
        await Context.SaveChangesAsync();
        
        return _serviceResultFactory.Success(OperationStatus.Created, true);
    }

    public async Task<IServiceResult<CommercialUserCreateDto>> AddCommercialUserAsync(CommercialUserCreateDto dto, string username)
    {
        Console.WriteLine("----------------------------------------------------------");
        var user = await Context.Users.FirstOrDefaultAsync(x => x.Username == username);
        Console.WriteLine("---------------------------------------------------------");
        if (user == null)
            return _serviceResultFactory.Failure<CommercialUserCreateDto>(OperationStatus.NotFound);
        Console.WriteLine("---------------------------------------------------------");
        dto.OriginalUserId = user.Id;
        Console.WriteLine("-----------------------------------------------------------");
        
        var commercialUser = Mapper.Map<Models.CommercialUser>(dto);
        commercialUser.OriginalUserId = user.Id;

        Console.WriteLine("AAAAAAAAAAAAAAAAAAAA");
        
        await Context.CommercialUsers.AddAsync(commercialUser);
        Console.WriteLine("AAAAAAAAAAAAAAAAAAAA");
        await Context.SaveChangesAsync();
        return _serviceResultFactory.Success(dto);
    }
}