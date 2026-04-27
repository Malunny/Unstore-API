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
    public class EmployeeService
    {
        private readonly IMapper mapper;
        private readonly AppDbContext context;

        public EmployeeService(IMapper map, AppDbContext context)
        {
            mapper = map;
            this.context = context;
        }
        public async Task<ServiceResult<EmployeeCreateDto>> CreateAsync(ModelStateDictionary modelstate, EmployeeCreateDto employeeCreateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<EmployeeCreateDto>.Failure(modelstate.GetErrors());

            bool emailExists = await context.Employees.AnyAsync(e => e.Email == employeeCreateDto.Email);

            if (emailExists)
                return ServiceResult<EmployeeCreateDto>.Failure
                (new ResultStatusMessage(OperationStatus.UserAlreadyExists, "There is already a user with this e-mail"));

            var position = await context.Positions.FirstOrDefaultAsync(r => r.Id == employeeCreateDto.PositionId);
            if (position is null)
                return ServiceResult<EmployeeCreateDto>.Failure(new ResultStatusMessage(OperationStatus.NotFound, "Position not found on database"));
            var employeeMapped = mapper.Map<EmployeeCreateDto,Employee>(employeeCreateDto);
            employeeMapped.Position = position;
            await context.Employees.AddAsync(employeeMapped);
            await context.SaveChangesAsync();

            return ServiceResult<EmployeeCreateDto>.Success(employeeCreateDto);
        }

        public async Task<ServiceResult<IEnumerable<EmployeeCreateDto>>> CreateRangeAsync(ModelStateDictionary modelstate, IEnumerable<EmployeeCreateDto> employeeCreateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<EmployeeCreateDto>>.Failure(modelstate.GetErrors());
            

            var employeePositionsIds = employeeCreateDtos.Select(e => e.PositionId).ToList();
            var positionsIdsDb = await context.Positions.ToDictionaryAsync(p => p.Id);
            
            var employeeMapped = mapper.Map<IEnumerable<EmployeeCreateDto>,List<Employee>>(employeeCreateDtos);
            int counter = 0;

            foreach (int employeePositionId in employeePositionsIds)
            {
                if (!positionsIdsDb.TryGetValue(employeePositionId, out var position))
                    return ServiceResult<IEnumerable<EmployeeCreateDto>>.Failure(new ResultStatusMessage(OperationStatus.NotFound, "Database don't have one (or more) positions that you've put."));   
                var employee = employeeMapped[counter];
                employee.Position = position;
            }

            await context.Employees.AddRangeAsync(employeeMapped);
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<EmployeeCreateDto>>.Success(employeeCreateDtos);
        }

        public async Task<ServiceResult<EmployeeUpdateDto>> UpdateAsync(ModelStateDictionary modelstate, EmployeeUpdateDto employeeUpdateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<EmployeeUpdateDto>.Failure(modelstate.GetErrors());
            
            var employeeFromDb = await context.Employees
                            .Include(e => e.Position)
                            .FirstOrDefaultAsync(x => x.Id == employeeUpdateDto.Id);

            if (employeeFromDb is null)
                return ServiceResult<EmployeeUpdateDto>.Failure(OperationStatus.NotFound);

            var mapped = mapper.Map(employeeUpdateDto, employeeFromDb);

            var returnedEmployee = mapper.Map(mapped, new EmployeeUpdateDto());
            
            await context.SaveChangesAsync();

            return ServiceResult<EmployeeUpdateDto>.Success(returnedEmployee, OperationStatus.Updated);
        }
        public async Task<ServiceResult<IEnumerable<EmployeeUpdateDto>>> UpdateRangeAsync
            (ModelStateDictionary modelstate, IEnumerable<EmployeeUpdateDto> employeeUpdateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<EmployeeUpdateDto>>.Failure(modelstate.GetErrors());
            
            var employeesList = employeeUpdateDtos.ToList();
            List<int> employeeIds = new(employeesList.Select(c => c.Id));

            var employeesFromDb = await context.Employees.Where(x => employeeIds.Contains(x.Id)).ToListAsync();
            
            if (employeesList.Count > employeesFromDb.Count)
                return ServiceResult<IEnumerable<EmployeeUpdateDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more employees wasn't found"));

            List<Employee> mappedEmployeesList = new();

            for (int i = 0; i < employeesFromDb.Count; i++)
                 mappedEmployeesList.Add(mapper.Map(employeesList[i], employeesFromDb[i]));

            
            var returnedEmployees = mapper.Map(mappedEmployeesList, new List<EmployeeUpdateDto>());
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<EmployeeUpdateDto>>.Success(returnedEmployees, OperationStatus.Updated);
        }

        public async Task<ServiceResult<EmployeeReadDto>> GetByIdAsync(int id)
        {
            var employee = await context.Employees
                        .AsNoTracking()
                        .Include(e => e.Position)
                        .FirstOrDefaultAsync(emp => emp.Id == id);

            if (employee is null)
                return ServiceResult<EmployeeReadDto>.Failure(OperationStatus.NotFound);

            var readDtoEmployee = mapper.Map<Employee,EmployeeReadDto>(employee);
            readDtoEmployee = readDtoEmployee with {Position = employee.Position.Name};

            return ServiceResult<EmployeeReadDto>
                .Success(readDtoEmployee, OperationStatus.Ok);
        }

        public async Task<ServiceResult<IEnumerable<EmployeeReadDto>>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            List<Employee> employeesFromDb = await context.Employees
                            .AsNoTracking()
                            .Include(e => e.Position)
                            .Where(emp => idsList.Contains(emp.Id)).ToListAsync();

            if (idsList.Count > employeesFromDb.Count)
                return ServiceResult<IEnumerable<EmployeeReadDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more employees wasn't found"));

            var employeesDtos = mapper.Map<IEnumerable<Employee>, List<EmployeeReadDto>>(employeesFromDb);

            for (int i = 0; i < employeesFromDb.Count; i++)
            {
                employeesDtos[i] = employeesDtos[i] with {Position = employeesFromDb[i].Position.Name};
                System.Console.WriteLine(employeesDtos[i].Position + " Position");
            }

            return ServiceResult<IEnumerable<EmployeeReadDto>>.Success(employeesDtos);
        }

        public async Task<ServiceResult<IEnumerable<EmployeeReadDto>>> GetRangeAsync(int skip, int take)
        {
            if (skip < 0 || take < 0)
                return ServiceResult<IEnumerable<EmployeeReadDto>>.Failure(OperationStatus.InvalidInput);

            var employeesFromDb = await context.Employees
                            .AsNoTracking()
                            .Include(e => e.Position)
                            .Skip(skip)
                            .Take(take)
                            .ToListAsync();

            List<EmployeeReadDto> mappedEmployees = mapper.Map<IEnumerable<Employee>, List<EmployeeReadDto>>(employeesFromDb);

            for (int i = 0; i < employeesFromDb.Count; i++)
            {
                mappedEmployees[i] = mappedEmployees[i] with {Position = employeesFromDb[i].Position.Name};
            }

            return ServiceResult<IEnumerable<EmployeeReadDto>>.Success(mappedEmployees);
        }

        public async Task<ServiceResult<object?>> RemoveAsync(int id)
        {
            Employee? employee = await context.Employees.FirstOrDefaultAsync(emp => emp.Id == id);

            if (employee is null)
                return ServiceResult<object?>.Failure(OperationStatus.NotFound);

            context.Employees.Remove(employee);
            await context.SaveChangesAsync();

            return ServiceResult<object?>.Success(null);
        }
        public async Task<ServiceResult<object?>> RemoveRangeAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            IEnumerable<Employee> employees = await context.Employees.Where(emp => idsList.Contains(emp.Id)).ToListAsync();

            if (!employees.Any())
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't employees with these ids."));

            if (employees.Count() < idsList.Count)
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't employees with one or more of these ids."));

            context.Employees.RemoveRange(employees);
            await context.SaveChangesAsync();

            return ServiceResult<object?>.Success(null);
        }
    }
}
