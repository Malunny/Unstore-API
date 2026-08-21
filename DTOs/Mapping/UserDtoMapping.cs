namespace Unstore.DTOs.Mapping;

public static partial class DtoMapping
{
    public static UserReadDto MapToDto(this Models.User user)
    {
        var rolesDto = user.Roles.Select(role => role.MapToString()).ToList();
        var addressesDto = user.Addresses.Select(address => address.MapToDto()).ToList();
        
        return new UserReadDto
        {
            Name = user.Name,
            Username =  user.Username,
            Email = user.Email,
            Roles = rolesDto,
            Addresses = addressesDto
        };
    }

    public static Models.User MapToModel(this UserCreateDtos dto)
    {
        return new Models.User
        {
            Name = dto.Name,
            Username = dto.Username,
            Email = dto.Email,
            Active = true
        };
    }
}