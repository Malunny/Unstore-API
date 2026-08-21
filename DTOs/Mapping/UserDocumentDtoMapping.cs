using Unstore.Models;

namespace Unstore.DTOs.Mapping;

public static partial class DtoMapping
{
    public static UserDocument MapToModel(this UserDocumentCreateDto dto)
        => new UserDocument
        {
            UserId = dto.UserId,
            Information = dto.Information,
            DocumentTypeId = dto.DocumentTypeId
        };

    public static UserDocumentReadDto MapToDto(this UserDocument model)
        => new UserDocumentReadDto
        {
            Id = model.Id,
            UserId = model.UserId,
            Information = model.Information,
            DocumentTypeId = model.DocumentTypeId
        };
}
