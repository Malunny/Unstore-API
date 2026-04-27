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
    public class ToolService
    {
        private readonly IMapper mapper;
        private readonly AppDbContext context;

        public ToolService(IMapper map, AppDbContext context)
        {
            mapper = map;
            this.context = context;
        }
        public async Task<ServiceResult<ToolCreateDto>> CreateAsync(ModelStateDictionary modelstate, ToolCreateDto toolCreateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<ToolCreateDto>.Failure(modelstate.GetErrors());

            var toolMapped = mapper.Map<ToolCreateDto,Tool>(toolCreateDto);

            await context.Tools.AddAsync(toolMapped);
            await context.SaveChangesAsync();

            return ServiceResult<ToolCreateDto>.Success(toolCreateDto);
        }

        public async Task<ServiceResult<IEnumerable<ToolCreateDto>>> CreateRangeAsync(ModelStateDictionary modelstate, IEnumerable<ToolCreateDto> toolCreateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<ToolCreateDto>>.Failure(modelstate.GetErrors());

            var toolMapped = mapper.Map<IEnumerable<ToolCreateDto>,IEnumerable<Tool>>(toolCreateDtos);

            await context.Tools.AddRangeAsync(toolMapped);
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<ToolCreateDto>>.Success(toolCreateDtos);
        }

        public async Task<ServiceResult<ToolUpdateDto>> UpdateAsync(ModelStateDictionary modelstate, ToolUpdateDto toolUpdateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<ToolUpdateDto>.Failure(modelstate.GetErrors());
            
            var toolFromDb = await context.Tools.FirstOrDefaultAsync(x => x.Id == toolUpdateDto.Id);

            if (toolFromDb is null)
                return ServiceResult<ToolUpdateDto>.Failure(OperationStatus.NotFound);

            var mapped = mapper.Map<ToolUpdateDto, Tool>(toolUpdateDto, toolFromDb);
            var returnedTool = mapper.Map(mapped, new ToolUpdateDto());
            await context.SaveChangesAsync();

            return ServiceResult<ToolUpdateDto>.Success(returnedTool, OperationStatus.Updated);
        }
        public async Task<ServiceResult<IEnumerable<ToolUpdateDto>>> UpdateRangeAsync
            (ModelStateDictionary modelstate, IEnumerable<ToolUpdateDto> toolUpdateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<ToolUpdateDto>>.Failure(modelstate.GetErrors());
            
            var toolsList = toolUpdateDtos.ToList();
            List<int> toolIds = new(toolsList.Select(c => c.Id));
            var toolsFromDb = await context.Tools.Where(x => toolIds.Contains(x.Id)).ToListAsync();

            if (toolsList.Count > toolsFromDb.Count)
                return ServiceResult<IEnumerable<ToolUpdateDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more tools wasn't found"));

            List<Tool> mappedToolsList = new();

            for (int i = 0; i < toolsFromDb.Count; i++)
                 mappedToolsList.Add(mapper.Map(toolsList[i], toolsFromDb[i]));

            var returnedTools = mapper.Map(mappedToolsList, new List<ToolUpdateDto>());
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<ToolUpdateDto>>.Success(returnedTools, OperationStatus.Updated);
        }

        public async Task<ServiceResult<ToolReadDto>> GetByIdAsync(int id)
        {
            var tool = await context.Tools.FirstOrDefaultAsync(t => t.Id == id);

            if (tool is null)
                return ServiceResult<ToolReadDto>.Failure(OperationStatus.NotFound);

            return ServiceResult<ToolReadDto>
                .Success(mapper.Map<Tool,ToolReadDto>(tool), OperationStatus.Ok);
        }

        public async Task<ServiceResult<IEnumerable<ToolReadDto>>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            var toolsFromDb = await context.Tools.Where(t => idsList.Contains(t.Id)).ToListAsync();

            if (idsList.Count > toolsFromDb.Count)
                return ServiceResult<IEnumerable<ToolReadDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more tools wasn't found"));

            var toolsDtos = mapper.Map<IEnumerable<Tool>, IEnumerable<ToolReadDto>>(toolsFromDb);
            return ServiceResult<IEnumerable<ToolReadDto>>.Success(toolsDtos);
        }

        public async Task<ServiceResult<IEnumerable<ToolReadDto>>> GetRangeAsync(int skip, int take)
        {
            if (skip < 0 || take < 0)
                return ServiceResult<IEnumerable<ToolReadDto>>.Failure(OperationStatus.InvalidInput);

            var toolsFromDb = await context.Tools.Skip(skip).Take(take).ToListAsync();
            var toolsDtos = mapper.Map<IEnumerable<Tool>, IEnumerable<ToolReadDto>>(toolsFromDb);
            return ServiceResult<IEnumerable<ToolReadDto>>.Success(toolsDtos);
        }

        public async Task<ServiceResult<object?>> RemoveAsync(int id)
        {
            Tool? tool = await context.Tools.FirstOrDefaultAsync(t => t.Id == id);
            if (tool is null)
                return ServiceResult<object?>.Failure(OperationStatus.NotFound);
            context.Tools.Remove(tool);
            await context.SaveChangesAsync();
            return ServiceResult<object?>.Success(null);
        }
        public async Task<ServiceResult<object?>> RemoveRangeAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            IEnumerable<Tool> tools = await context.Tools.Where(t => idsList.Contains(t.Id)).ToListAsync();
            if (!tools.Any())
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't tools with these ids."));
            if (tools.Count() < idsList.Count)
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't tools with one or more of these ids."));
            context.Tools.RemoveRange(tools);
            await context.SaveChangesAsync();
            return ServiceResult<object?>.Success(null);
        }
    }
}
