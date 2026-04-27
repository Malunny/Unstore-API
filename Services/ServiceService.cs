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
    public class ServiceService
    {
        private readonly IMapper mapper;
        private readonly AppDbContext context;

        public ServiceService(IMapper map, AppDbContext context)
        {
            mapper = map;
            this.context = context;
        }
        public async Task<ServiceResult<ServiceCreateDto>> CreateAsync(ModelStateDictionary modelstate, ServiceCreateDto serviceCreateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<ServiceCreateDto>.Failure(modelstate.GetErrors());

            var serviceMapped = mapper.Map<ServiceCreateDto,Unstore.Models.Service>(serviceCreateDto);

            await context.Services.AddAsync(serviceMapped);
            await context.SaveChangesAsync();

            return ServiceResult<ServiceCreateDto>.Success(serviceCreateDto);
        }

        public async Task<ServiceResult<IEnumerable<ServiceCreateDto>>> CreateRangeAsync(ModelStateDictionary modelstate, IEnumerable<ServiceCreateDto> serviceCreateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<ServiceCreateDto>>.Failure(modelstate.GetErrors());

            var serviceMapped = mapper.Map<IEnumerable<ServiceCreateDto>,IEnumerable<Unstore.Models.Service>>(serviceCreateDtos);

            await context.Services.AddRangeAsync(serviceMapped);
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<ServiceCreateDto>>.Success(serviceCreateDtos);
        }

        public async Task<ServiceResult<ServiceUpdateDto>> UpdateAsync(ModelStateDictionary modelstate, ServiceUpdateDto serviceUpdateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<ServiceUpdateDto>.Failure(modelstate.GetErrors());
            
            var serviceFromDb = await context.Services.FirstOrDefaultAsync(x => x.Id == serviceUpdateDto.Id);

            if (serviceFromDb is null)
                return ServiceResult<ServiceUpdateDto>.Failure(OperationStatus.NotFound);

            var mapped = mapper.Map<ServiceUpdateDto, Unstore.Models.Service>(serviceUpdateDto, serviceFromDb);
            var returnedService = mapper.Map(mapped, new ServiceUpdateDto());
            await context.SaveChangesAsync();

            return ServiceResult<ServiceUpdateDto>.Success(returnedService, OperationStatus.Updated);
        }
        public async Task<ServiceResult<IEnumerable<ServiceUpdateDto>>> UpdateRangeAsync
            (ModelStateDictionary modelstate, IEnumerable<ServiceUpdateDto> serviceUpdateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<ServiceUpdateDto>>.Failure(modelstate.GetErrors());
            
            var servicesList = serviceUpdateDtos.ToList();
            List<int> serviceIds = new(servicesList.Select(c => c.Id));
            var servicesFromDb = await context.Services.Where(x => serviceIds.Contains(x.Id)).ToListAsync();

            if (servicesList.Count > servicesFromDb.Count)
                return ServiceResult<IEnumerable<ServiceUpdateDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more services wasn't found"));

            List<Unstore.Models.Service> mappedServicesList = new();

            for (int i = 0; i < servicesFromDb.Count; i++)
                 mappedServicesList.Add(mapper.Map(servicesList[i], servicesFromDb[i]));

            var returnedServices = mapper.Map(mappedServicesList, new List<ServiceUpdateDto>());
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<ServiceUpdateDto>>.Success(returnedServices, OperationStatus.Updated);
        }

        public async Task<ServiceResult<ServiceReadDto>> GetByIdAsync(int id)
        {
            var service = await context.Services.FirstOrDefaultAsync(serv => serv.Id == id);

            if (service is null)
                return ServiceResult<ServiceReadDto>.Failure(OperationStatus.NotFound);

            return ServiceResult<ServiceReadDto>
                .Success(mapper.Map<Unstore.Models.Service,ServiceReadDto>(service), OperationStatus.Ok);
        }

        public async Task<ServiceResult<IEnumerable<ServiceReadDto>>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            var servicesFromDb = await context.Services.Where(serv => idsList.Contains(serv.Id)).ToListAsync();

            if (idsList.Count > servicesFromDb.Count)
                return ServiceResult<IEnumerable<ServiceReadDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more services wasn't found"));

            var servicesDtos = mapper.Map<IEnumerable<Unstore.Models.Service>, IEnumerable<ServiceReadDto>>(servicesFromDb);
            return ServiceResult<IEnumerable<ServiceReadDto>>.Success(servicesDtos);
        }

        public async Task<ServiceResult<IEnumerable<ServiceReadDto>>> GetRangeAsync(int skip, int take)
        {
            if (skip < 0 || take < 0)
                return ServiceResult<IEnumerable<ServiceReadDto>>.Failure(OperationStatus.InvalidInput);

            var servicesFromDb = await context.Services.Skip(skip).Take(take).ToListAsync();
            var servicesDtos = mapper.Map<IEnumerable<Unstore.Models.Service>, IEnumerable<ServiceReadDto>>(servicesFromDb);
            return ServiceResult<IEnumerable<ServiceReadDto>>.Success(servicesDtos);
        }

        public async Task<ServiceResult<object?>> RemoveAsync(int id)
        {
            Unstore.Models.Service? service = await context.Services.FirstOrDefaultAsync(serv => serv.Id == id);
            if (service is null)
                return ServiceResult<object?>.Failure(OperationStatus.NotFound);
            context.Services.Remove(service);
            await context.SaveChangesAsync();
            return ServiceResult<object?>.Success(null);
        }
        public async Task<ServiceResult<object?>> RemoveRangeAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            IEnumerable<Unstore.Models.Service> services = await context.Services.Where(serv => idsList.Contains(serv.Id)).ToListAsync();
            if (!services.Any())
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't services with these ids."));
            if (services.Count() < idsList.Count)
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't services with one or more of these ids."));
            context.Services.RemoveRange(services);
            await context.SaveChangesAsync();
            return ServiceResult<object?>.Success(null);
        }
    }
}
