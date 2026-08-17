using Unstore.Models;

namespace Unstore.DTOs.Mapping;

public static partial class DtoMapping
{
    public static RoleReadDto MapToDto(this Role role)
        => new RoleReadDto
        {
            Name = role.Name,
            Description = role.Description,
            Id = role.Id
        };
    public static string MapToString(this Role role) => role.Name;

    public static Role MapToModel(this RoleCreateDto dto)
        => new Role
        {
            Name = dto.Name,
            Description = dto.Description
        };

    public static void MapFromUpdateDto(this Role role, RoleUpdateDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Name)) role.Name = dto.Name;
        if (!string.IsNullOrWhiteSpace(dto.Description)) role.Description = dto.Description;
    }
}