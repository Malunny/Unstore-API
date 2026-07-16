using Unstore.Models;

namespace Unstore.DTOs.Mapping;

public static partial class DtoMapping
{
    public static CommercialUser MapToModel(this CommercialUserCreateDto dto)
    {
        var model = new CommercialUser
        {
            CommercialName = dto.CommercialName,
            About =  dto.About,
            OriginalUserId =  dto.OriginalUserId
        };
        
        return model;
    }

    public static CommercialUserReadDto MapToDto(this CommercialUser model)
    {
        var dto = new CommercialUserReadDto();
        dto.Id = model.Id;
        dto.About = model.About;
        dto.CommercialName = model.CommercialName;
        return dto;
    }
}