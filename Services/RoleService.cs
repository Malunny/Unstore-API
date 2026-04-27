using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTO;
using Unstore.Extensions;
using Unstore.Models;

namespace Unstore.Services
{
    public class RoleService
    {
        private readonly IMapper mapper;
        private readonly AppDbContext context;

        public RoleService(IMapper map, AppDbContext context)
        {
            mapper = map;
            this.context = context;
        }
        public async Task<ServiceResult<RoleCreateDto>> CreateAsync(ModelStateDictionary modelstate, RoleCreateDto roleCreateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<RoleCreateDto>.Failure(modelstate.GetErrors());

            var roleMapped = mapper.Map<RoleCreateDto,Role>(roleCreateDto);

            await context.Roles.AddAsync(roleMapped);
            await context.SaveChangesAsync();

            return ServiceResult<RoleCreateDto>.Success(roleCreateDto);
        }

        public async Task<ServiceResult<IEnumerable<RoleCreateDto>>> CreateRangeAsync(ModelStateDictionary modelstate, IEnumerable<RoleCreateDto> roleCreateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<RoleCreateDto>>.Failure(modelstate.GetErrors());

            var roleMapped = mapper.Map<IEnumerable<RoleCreateDto>,IEnumerable<Role>>(roleCreateDtos);

            await context.Roles.AddRangeAsync(roleMapped);
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<RoleCreateDto>>.Success(roleCreateDtos);
        }

        public async Task<ServiceResult<RoleUpdateDto>> UpdateAsync(ModelStateDictionary modelstate, RoleUpdateDto roleUpdateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<RoleUpdateDto>.Failure(modelstate.GetErrors());
            
            var roleFromDb = await context.Roles.FirstOrDefaultAsync(x => x.Id == roleUpdateDto.Id);

            if (roleFromDb is null)
                return ServiceResult<RoleUpdateDto>.Failure(OperationStatus.NotFound);

            var mapped = mapper.Map<RoleUpdateDto, Role>(roleUpdateDto, roleFromDb);
            var returnedRole = mapper.Map(mapped, new RoleUpdateDto());
            await context.SaveChangesAsync();

            return ServiceResult<RoleUpdateDto>.Success(returnedRole, OperationStatus.Updated);
        }
        public async Task<ServiceResult<IEnumerable<RoleUpdateDto>>> UpdateRangeAsync
            (ModelStateDictionary modelstate, IEnumerable<RoleUpdateDto> roleUpdateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<RoleUpdateDto>>.Failure(modelstate.GetErrors());
            
            var rolesList = roleUpdateDtos.ToList();
            List<int> roleIds = new(rolesList.Select(c => c.Id));
            var rolesFromDb = await context.Roles.Where(x => roleIds.Contains(x.Id)).ToListAsync();

            if (rolesList.Count > rolesFromDb.Count)
                return ServiceResult<IEnumerable<RoleUpdateDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more roles wasn't found"));

            List<Role> mappedRolesList = new();

            for (int i = 0; i < rolesFromDb.Count; i++)
                 mappedRolesList.Add(mapper.Map(rolesList[i], rolesFromDb[i]));

            var returnedRoles = mapper.Map(mappedRolesList, new List<RoleUpdateDto>());
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<RoleUpdateDto>>.Success(returnedRoles, OperationStatus.Updated);
        }

        public async Task<ServiceResult<RoleReadDto>> GetByIdAsync(int id)
        {
            var role = await context.Roles.FirstOrDefaultAsync(r => r.Id == id);

            if (role is null)
                return ServiceResult<RoleReadDto>.Failure(OperationStatus.NotFound);

            return ServiceResult<RoleReadDto>
                .Success(mapper.Map<Role,RoleReadDto>(role), OperationStatus.Ok);
        }

        public async Task<ServiceResult<IEnumerable<RoleReadDto>>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            var rolesFromDb = await context.Roles.Where(r => idsList.Contains(r.Id)).ToListAsync();

            if (idsList.Count > rolesFromDb.Count)
                return ServiceResult<IEnumerable<RoleReadDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more roles wasn't found"));

            var rolesDtos = mapper.Map<IEnumerable<Role>, IEnumerable<RoleReadDto>>(rolesFromDb);
            return ServiceResult<IEnumerable<RoleReadDto>>.Success(rolesDtos);
        }

        public async Task<ServiceResult<IEnumerable<RoleReadDto>>> GetRangeAsync(int skip, int take)
        {
            if (skip < 0 || take < 0)
                return ServiceResult<IEnumerable<RoleReadDto>>.Failure(OperationStatus.InvalidInput);

            var rolesFromDb = await context.Roles.Skip(skip).Take(take).ToListAsync();
            var rolesDtos = mapper.Map<IEnumerable<Role>, IEnumerable<RoleReadDto>>(rolesFromDb);
            return ServiceResult<IEnumerable<RoleReadDto>>.Success(rolesDtos);
        }

        public async Task<ServiceResult<object?>> RemoveAsync(int id)
        {
            Role? role = await context.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role is null)
                return ServiceResult<object?>.Failure(OperationStatus.NotFound);
            context.Roles.Remove(role);
            await context.SaveChangesAsync();
            return ServiceResult<object?>.Success(null);
        }
        public async Task<ServiceResult<object?>> RemoveRangeAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            IEnumerable<Role> roles = await context.Roles.Where(r => idsList.Contains(r.Id)).ToListAsync();
            if (!roles.Any())
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't roles with these ids."));
            if (roles.Count() < idsList.Count)
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't roles with one or more of these ids."));
            context.Roles.RemoveRange(roles);
            await context.SaveChangesAsync();
            return ServiceResult<object?>.Success(null);
        }
    }
}