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
    public class PositionService
    {
        private readonly IMapper mapper;
        private readonly AppDbContext context;

        public PositionService(IMapper map, AppDbContext context)
        {
            mapper = map;
            this.context = context;
        }
        public async Task<ServiceResult<PositionCreateDto>> CreateAsync(ModelStateDictionary modelstate, PositionCreateDto positionCreateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<PositionCreateDto>.Failure(modelstate.GetErrors());

            var positionMapped = mapper.Map<PositionCreateDto,Position>(positionCreateDto);

            await context.Positions.AddAsync(positionMapped);
            await context.SaveChangesAsync();

            return ServiceResult<PositionCreateDto>.Success(positionCreateDto);
        }

        public async Task<ServiceResult<IEnumerable<PositionCreateDto>>> CreateRangeAsync(ModelStateDictionary modelstate, IEnumerable<PositionCreateDto> positionCreateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<PositionCreateDto>>.Failure(modelstate.GetErrors());

            var positionMapped = mapper.Map<IEnumerable<PositionCreateDto>,IEnumerable<Position>>(positionCreateDtos);

            await context.Positions.AddRangeAsync(positionMapped);
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<PositionCreateDto>>.Success(positionCreateDtos);
        }

        public async Task<ServiceResult<PositionUpdateDto>> UpdateAsync(ModelStateDictionary modelstate, PositionUpdateDto positionUpdateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<PositionUpdateDto>.Failure(modelstate.GetErrors());
            
            var positionFromDb = await context.Positions.FirstOrDefaultAsync(x => x.Id == positionUpdateDto.Id);

            if (positionFromDb is null)
                return ServiceResult<PositionUpdateDto>.Failure(OperationStatus.NotFound);

            var mapped = mapper.Map(positionUpdateDto, positionFromDb);

            var returnedPosition = mapper.Map(mapped, new PositionUpdateDto());
            await context.SaveChangesAsync();

            return ServiceResult<PositionUpdateDto>.Success(returnedPosition, OperationStatus.Updated);
        }
        public async Task<ServiceResult<IEnumerable<PositionUpdateDto>>> UpdateRangeAsync
            (ModelStateDictionary modelstate, IEnumerable<PositionUpdateDto> positionUpdateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<PositionUpdateDto>>.Failure(modelstate.GetErrors());
            
            var positionsList = positionUpdateDtos.ToList();
            List<int> positionIds = new(positionsList.Select(c => c.Id));
            var positionsFromDb = await context.Positions.Where(x => positionIds.Contains(x.Id)).ToListAsync();

            if (positionsList.Count > positionsFromDb.Count)
                return ServiceResult<IEnumerable<PositionUpdateDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more positions wasn't found"));

            List<Position> mappedPositionsList = new();

            for (int i = 0; i < positionsFromDb.Count; i++)
                 mappedPositionsList.Add(mapper.Map(positionsList[i], positionsFromDb[i]));

            var returnedPositions = mapper.Map(mappedPositionsList, new List<PositionUpdateDto>());

            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<PositionUpdateDto>>.Success(returnedPositions, OperationStatus.Updated);
        }

        public async Task<ServiceResult<PositionReadDto>> GetByIdAsync(int id)
        {
            var position = await context.Positions.FirstOrDefaultAsync(pos => pos.Id == id);

            if (position is null)
                return ServiceResult<PositionReadDto>.Failure(OperationStatus.NotFound);

            return ServiceResult<PositionReadDto>
                .Success(mapper.Map<Position,PositionReadDto>(position), OperationStatus.Ok);
        }

        public async Task<ServiceResult<IEnumerable<PositionReadDto>>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            var positionsFromDb = await context.Positions.Where(pos => idsList.Contains(pos.Id)).ToListAsync();

            if (idsList.Count > positionsFromDb.Count)
                return ServiceResult<IEnumerable<PositionReadDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more positions wasn't found"));

            var positionsDtos = mapper.Map<IEnumerable<Position>, IEnumerable<PositionReadDto>>(positionsFromDb);
            return ServiceResult<IEnumerable<PositionReadDto>>.Success(positionsDtos);
        }

        public async Task<ServiceResult<IEnumerable<PositionReadDto>>> GetRangeAsync(int skip, int take)
        {
            if (skip < 0 || take < 0)
                return ServiceResult<IEnumerable<PositionReadDto>>.Failure(OperationStatus.InvalidInput);

            var positionsFromDb = await context.Positions.Skip(skip).Take(take).ToListAsync();
            var positionsDtos = mapper.Map<IEnumerable<Position>, IEnumerable<PositionReadDto>>(positionsFromDb);
            return ServiceResult<IEnumerable<PositionReadDto>>.Success(positionsDtos);
        }

        public async Task<ServiceResult<object?>> RemoveAsync(int id)
        {
            Position? position = await context.Positions.FirstOrDefaultAsync(pos => pos.Id == id);
            if (position is null)
                return ServiceResult<object?>.Failure(OperationStatus.NotFound);
            context.Positions.Remove(position);
            await context.SaveChangesAsync();
            return ServiceResult<object?>.Success(null);
        }
        public async Task<ServiceResult<object?>> RemoveRangeAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            IEnumerable<Position> positions = await context.Positions.Where(pos => idsList.Contains(pos.Id)).ToListAsync();
            if (!positions.Any())
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't positions with these ids."));

            if (positions.Count() < idsList.Count)
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't positions with one or more of these ids."));

            context.Positions.RemoveRange(positions);
            await context.SaveChangesAsync();
            return ServiceResult<object?>.Success(null);
        }
    }
}
