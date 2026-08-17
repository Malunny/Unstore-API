using Unstore.Models;

namespace Unstore.DTOs.Mapping;

public static partial class DtoMapping
{
    public static DocumentType MapToModel(this DocumentTypeCreateDto dto)
        => new DocumentType
        {
            Key = dto.Key,
            Description = dto.Description
        };

    public static DocumentTypeReadDto MapToDto(this DocumentType model)
        => new DocumentTypeReadDto
        {
            Id = model.Id,
            Key = model.Key,
            Description = model.Description
        };

    public static void MapFromUpdateDto(this DocumentType model, DocumentTypeUpdateDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Key)) model.Key = dto.Key;
        if (!string.IsNullOrWhiteSpace(dto.Description)) model.Description = dto.Description;
    }
}
