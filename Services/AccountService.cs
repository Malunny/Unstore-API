using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Extensions;
using Unstore.Models;
using BCrypt.Net;

namespace Unstore.Services
{
    public class AccountService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public AccountService(AppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ServiceResult<string>> ValidateLoginAsync
        (ModelStateDictionary modelState, UserLoginDto userLogin)
        {
            if (!modelState.IsValid)
                return ServiceResult<string>.Failure(modelState.GetErrors());

            User? trackedUser = await _context.Users
                            .AsNoTracking()
                            .Include(u => u.Role)
                            .FirstOrDefaultAsync(u => u.Email == userLogin.Email);
    
            if (trackedUser is null)
                return ServiceResult<string>.Failure(OperationStatus.NotFound);

            if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, trackedUser.Password))
                return ServiceResult<string>.Failure
                (new ResultStatusMessage(OperationStatus.InvalidInput, "Password is incorrect"));

            var tokenService = new TokenService();
            string token = tokenService.GenerateToken(trackedUser);

            return ServiceResult<string>.Success(token);
        }

        private void CreateHashedPassword(string password, UserCreationDto userCreation)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            userCreation.Password = hashedPassword;
        }

        public async Task<ServiceResult<bool>> RegisterAsync(ModelStateDictionary modelState, UserCreationDto user)
        {
            if (!modelState.IsValid)
                return ServiceResult<bool>.Failure(modelState.GetErrors());
            
            var hasTrackedUser = await _context.Users
                                .AsNoTracking()
                                .AnyAsync(u => u.Email == user.Email || u.Username == user.Username);
            if (hasTrackedUser)
                return ServiceResult<bool>.Failure
                (new ResultStatusMessage(OperationStatus.UserAlreadyExists, "this is email is already being used."));
            
            CreateHashedPassword(user.Password, user);

            var mappedUser = _mapper.Map<UserCreationDto, User>(user);

            mappedUser.Role = await _context.Roles.FirstAsync(r => r.Id == 1);

            await _context.Users.AddAsync(mappedUser);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        } 
    }
}