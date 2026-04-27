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
    public class ToolTagService
    {
        private readonly IMapper mapper;
        private readonly AppDbContext context;

        public ToolTagService(IMapper map, AppDbContext context)
        {
            mapper = map;
            this.context = context;
        }
        public async Task<ServiceResult<ToolTagCreateDto>> CreateAsync(ModelStateDictionary modelstate, ToolTagCreateDto toolTagCreateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<ToolTagCreateDto>.Failure(modelstate.GetErrors());

            var toolTagMapped = mapper.Map<ToolTagCreateDto,ToolTag>(toolTagCreateDto);

            await context.ToolTags.AddAsync(toolTagMapped);
            await context.SaveChangesAsync();

            return ServiceResult<ToolTagCreateDto>.Success(toolTagCreateDto);
        }

        public async Task<ServiceResult<IEnumerable<ToolTagCreateDto>>> CreateRangeAsync(ModelStateDictionary modelstate, IEnumerable<ToolTagCreateDto> toolTagCreateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<ToolTagCreateDto>>.Failure(modelstate.GetErrors());

            var toolTagMapped = mapper.Map<IEnumerable<ToolTagCreateDto>,IEnumerable<ToolTag>>(toolTagCreateDtos);

            await context.ToolTags.AddRangeAsync(toolTagMapped);
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<ToolTagCreateDto>>.Success(toolTagCreateDtos);
        }

        public async Task<ServiceResult<ToolTagUpdateDto>> UpdateAsync(ModelStateDictionary modelstate, ToolTagUpdateDto toolTagUpdateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<ToolTagUpdateDto>.Failure(modelstate.GetErrors());
            
            var toolTagFromDb = await context.ToolTags.FirstOrDefaultAsync(x => x.Id == toolTagUpdateDto.Id);

            if (toolTagFromDb is null)
                return ServiceResult<ToolTagUpdateDto>.Failure(OperationStatus.NotFound);

            var mapped = mapper.Map<ToolTagUpdateDto, ToolTag>(toolTagUpdateDto, toolTagFromDb);
            var returnedToolTag = mapper.Map(mapped, new ToolTagUpdateDto());
            await context.SaveChangesAsync();

            return ServiceResult<ToolTagUpdateDto>.Success(returnedToolTag, OperationStatus.Updated);
        }
        public async Task<ServiceResult<IEnumerable<ToolTagUpdateDto>>> UpdateRangeAsync
            (ModelStateDictionary modelstate, IEnumerable<ToolTagUpdateDto> toolTagUpdateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<ToolTagUpdateDto>>.Failure(modelstate.GetErrors());
            
            var toolTagsList = toolTagUpdateDtos.ToList();
            List<int> toolTagIds = new(toolTagsList.Select(c => c.Id));
            var toolTagsFromDb = await context.ToolTags.Where(x => toolTagIds.Contains(x.Id)).ToListAsync();

            if (toolTagsList.Count > toolTagsFromDb.Count)
                return ServiceResult<IEnumerable<ToolTagUpdateDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more tool tags wasn't found"));

            List<ToolTag> mappedToolTagsList = new();

            for (int i = 0; i < toolTagsFromDb.Count; i++)
                 mappedToolTagsList.Add(mapper.Map(toolTagsList[i], toolTagsFromDb[i]));

            var returnedToolTags = mapper.Map(mappedToolTagsList, new List<ToolTagUpdateDto>());
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<ToolTagUpdateDto>>.Success(returnedToolTags, OperationStatus.Updated);
        }

        public async Task<ServiceResult<ToolTagReadDto>> GetByIdAsync(int id)
        {
            var toolTag = await context.ToolTags.FirstOrDefaultAsync(tt => tt.Id == id);

            if (toolTag is null)
                return ServiceResult<ToolTagReadDto>.Failure(OperationStatus.NotFound);

            return ServiceResult<ToolTagReadDto>
                .Success(mapper.Map<ToolTag,ToolTagReadDto>(toolTag), OperationStatus.Ok);
        }

        public async Task<ServiceResult<IEnumerable<ToolTagReadDto>>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            var toolTagsFromDb = await context.ToolTags.Where(tt => idsList.Contains(tt.Id)).ToListAsync();

            if (idsList.Count > toolTagsFromDb.Count)
                return ServiceResult<IEnumerable<ToolTagReadDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more tool tags wasn't found"));

            var toolTagsDtos = mapper.Map<IEnumerable<ToolTag>, IEnumerable<ToolTagReadDto>>(toolTagsFromDb);
            return ServiceResult<IEnumerable<ToolTagReadDto>>.Success(toolTagsDtos);
        }

        public async Task<ServiceResult<IEnumerable<ToolTagReadDto>>> GetRangeAsync(int skip, int take)
        {
            if (skip < 0 || take < 0)
                return ServiceResult<IEnumerable<ToolTagReadDto>>.Failure(OperationStatus.InvalidInput);

            var toolTagsFromDb = await context.ToolTags.Skip(skip).Take(take).ToListAsync();
            var toolTagsDtos = mapper.Map<IEnumerable<ToolTag>, IEnumerable<ToolTagReadDto>>(toolTagsFromDb);
            return ServiceResult<IEnumerable<ToolTagReadDto>>.Success(toolTagsDtos);
        }

        public async Task<ServiceResult<object?>> RemoveAsync(int id)
        {
            ToolTag? toolTag = await context.ToolTags.FirstOrDefaultAsync(tt => tt.Id == id);
            if (toolTag is null)
                return ServiceResult<object?>.Failure(OperationStatus.NotFound);
            context.ToolTags.Remove(toolTag);
            await context.SaveChangesAsync();
            return ServiceResult<object?>.Success(null);
        }
        public async Task<ServiceResult<object?>> RemoveRangeAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            IEnumerable<ToolTag> toolTags = await context.ToolTags.Where(tt => idsList.Contains(tt.Id)).ToListAsync();
            if (!toolTags.Any())
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't tool tags with these ids."));
            if (toolTags.Count() < idsList.Count)
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't tool tags with one or more of these ids."));
            context.ToolTags.RemoveRange(toolTags);
            await context.SaveChangesAsync();
            return ServiceResult<object?>.Success(null);
        }
    }
}
