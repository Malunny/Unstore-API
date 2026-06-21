using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.DocumentType;

public class DocumentTypeService : BaseService
{
    public DocumentTypeService(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IServiceResult<List<DocumentTypeReadDto>>> GetAllAsync()
    {
        try
        {
            var documentTypes = await Context.DocumentTypes.AsNoTracking().ToListAsync();
            var dtos = Mapper.Map<List<DocumentTypeReadDto>>(documentTypes);
            return new DataServiceResult<List<DocumentTypeReadDto>>(true, dtos);
        }
        catch (Exception)
        {
            return new DataServiceResult<List<DocumentTypeReadDto>>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<DocumentTypeReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<DocumentTypeReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var documentType = await Context.DocumentTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (documentType == null)
                return new DataServiceResult<DocumentTypeReadDto>(OperationStatus.NotFound, false);

            var dto = Mapper.Map<DocumentTypeReadDto>(documentType);
            return new DataServiceResult<DocumentTypeReadDto>(true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<DocumentTypeReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<DocumentTypeReadDto>> CreateAsync(DocumentTypeCreateDto createDto)
    {
        if (createDto == null)
            return new DataServiceResult<DocumentTypeReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var keyExists = await Context.DocumentTypes.AnyAsync(x => x.Key == createDto.Key);
            if (keyExists)
                return new DataServiceResult<DocumentTypeReadDto>(OperationStatus.ValidationError, false);

            var documentType = Mapper.Map<Models.DocumentType>(createDto);

            await Context.DocumentTypes.AddAsync(documentType);
            await Context.SaveChangesAsync();

            var dto = Mapper.Map<DocumentTypeReadDto>(documentType);
            return new DataServiceResult<DocumentTypeReadDto>(OperationStatus.Created, true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<DocumentTypeReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<DocumentTypeReadDto>> UpdateAsync(DocumentTypeUpdateDto updateDto)
    {
        if (updateDto == null || updateDto.Id <= 0)
            return new DataServiceResult<DocumentTypeReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var documentType = await Context.DocumentTypes.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

            if (documentType == null)
                return new DataServiceResult<DocumentTypeReadDto>(OperationStatus.NotFound, false);

            if (updateDto.Key != null && updateDto.Key != documentType.Key)
            {
                var keyExists = await Context.DocumentTypes.AnyAsync(x => x.Key == updateDto.Key && x.Id != updateDto.Id);
                if (keyExists)
                    return new DataServiceResult<DocumentTypeReadDto>(OperationStatus.ValidationError, false);
            }

            Mapper.Map(updateDto, documentType);
            Context.DocumentTypes.Update(documentType);
            await Context.SaveChangesAsync();

            var dto = Mapper.Map<DocumentTypeReadDto>(documentType);
            return new DataServiceResult<DocumentTypeReadDto>(OperationStatus.Updated, true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<DocumentTypeReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        try
        {
            var documentType = await Context.DocumentTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (documentType == null)
                return new DataServiceResult<bool>(OperationStatus.NotFound, false);

            Context.DocumentTypes.Remove(documentType);
            await Context.SaveChangesAsync();

            return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
        }
        catch (Exception)
        {
            return new DataServiceResult<bool>(OperationStatus.InternalServerError, false);
        }
    }
}
