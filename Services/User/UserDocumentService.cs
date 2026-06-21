using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services;

public class UserDocumentService : BaseService
{
    private readonly IServiceResultFactory _serviceResultFactory;
    public UserDocumentService(AppDbContext dbContext, IMapper mapper, IServiceResultFactory serviceResultFactory) : base(dbContext, mapper)
    {
        _serviceResultFactory = serviceResultFactory;
    }

    public async Task<IServiceResult<UserDocumentCreateDto>> CreateAsync(UserDocumentCreateDto createDto)
    {
        // Validate input
        if (createDto == null)
            return _serviceResultFactory.Failure<UserDocumentCreateDto>(OperationStatus.InvalidInput);

        if (createDto.UserId <= 0 || createDto.DocumentTypeId <= 0)
            return _serviceResultFactory.Failure<UserDocumentCreateDto>(OperationStatus.InvalidInput);

        // Check if user exists
        var userExists = await Context.Users.AnyAsync(x => x.Id == createDto.UserId);
        if (!userExists)
            return _serviceResultFactory.Failure<UserDocumentCreateDto>(OperationStatus.NotFound);

        // Check if document type exists
        var documentTypeExists = await Context.DocumentTypes.AnyAsync(x => x.Id == createDto.DocumentTypeId);
        if (!documentTypeExists)
            return _serviceResultFactory.Failure<UserDocumentCreateDto>(OperationStatus.NotFound);

        // Map and create
        var newDocument = Mapper.Map<UserDocumentCreateDto, UserDocument>(createDto);
        await Context.UserDocuments.AddAsync(newDocument);
        await Context.SaveChangesAsync();

        return _serviceResultFactory.Success(OperationStatus.Created, createDto);
    }

    /// <summary>
    /// Gets a specific user document by ID.
    /// </summary>
    public async Task<IServiceResult<UserDocumentReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return _serviceResultFactory.Failure<UserDocumentReadDto>(OperationStatus.InvalidInput);

        var document = await Context.UserDocuments
            .Include(x => x.DocumentType)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (document == null)
            return _serviceResultFactory.Failure<UserDocumentReadDto>(OperationStatus.NotFound);

        var dto = Mapper.Map<UserDocument, UserDocumentReadDto>(document);
        return _serviceResultFactory.Success(dto);
    }

    /// <summary>
    /// Gets all documents for a specific user.
    /// </summary>
    public async Task<IServiceResult<List<UserDocumentReadDto>>> GetUserDocuments(int userId)
    {
        if (userId <= 0)
            return _serviceResultFactory.Failure<List<UserDocumentReadDto>>(OperationStatus.InvalidInput);

        var documents = await Context.UserDocuments
            .Where(x => x.UserId == userId)
            .Include(x => x.DocumentType)
            .ToListAsync();

        if (documents.Count == 0)
            return _serviceResultFactory.Failure<List<UserDocumentReadDto>>(OperationStatus.NotFound);

        var dtos = Mapper.Map<List<UserDocument>, List<UserDocumentReadDto>>(documents);
        return _serviceResultFactory.Success(dtos);
    }

    /// <summary>
    /// Updates an existing user document.
    /// </summary>
    public async Task<IServiceResult<UserDocumentUpdateDto>> UpdateAsync(int id, UserDocumentUpdateDto updateDto)
    {
        if (id <= 0 || updateDto == null)
            return _serviceResultFactory.Failure<UserDocumentUpdateDto>(OperationStatus.InvalidInput);

        var document = await Context.UserDocuments.FirstOrDefaultAsync(x => x.Id == id);
        if (document == null)
            return _serviceResultFactory.Failure<UserDocumentUpdateDto>(OperationStatus.NotFound);
        
        var documentTypeExists = await Context.DocumentTypes.AnyAsync(x => x.Id == updateDto.DocumentTypeId);
        if (!documentTypeExists)
            return _serviceResultFactory.Failure<UserDocumentUpdateDto>(OperationStatus.NotFound);

        document.Information = updateDto.Information;

        Context.UserDocuments.Update(document);
        await Context.SaveChangesAsync();

        return _serviceResultFactory.Success(OperationStatus.Updated, updateDto);
    }

    /// <summary>
    /// Deletes a user document by ID.
    /// </summary>
    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return _serviceResultFactory.Failure<bool>(OperationStatus.InvalidInput);

        var document = await Context.UserDocuments.FirstOrDefaultAsync(x => x.Id == id);
        if (document == null)
            return _serviceResultFactory.Failure<bool>(OperationStatus.NotFound);

        Context.UserDocuments.Remove(document);
        await Context.SaveChangesAsync();

        return _serviceResultFactory.Success(OperationStatus.Deleted, true);
    }

    /// <summary>
    /// Deletes all documents for a specific user.
    /// </summary>
    public async Task<IServiceResult<bool>> DeleteUserDocumentsAsync(int userId)
    {
        if (userId <= 0)
            return _serviceResultFactory.Failure<bool>(OperationStatus.InvalidInput);

        var documents = await Context.UserDocuments
            .Where(x => x.UserId == userId)
            .ToListAsync();

        if (documents.Count == 0)
            return _serviceResultFactory.Failure<bool>(OperationStatus.NotFound);

        Context.UserDocuments.RemoveRange(documents);
        await Context.SaveChangesAsync();

        return _serviceResultFactory.Success(OperationStatus.Deleted, true);
    }
}