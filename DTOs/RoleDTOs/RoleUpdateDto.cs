using System.ComponentModel.DataAnnotations;

namespace Unstore.DTO;

public record RoleUpdateDto
{
    [Required(ErrorMessage = "Role ID is required.")] public int Id { get; set; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")] public string? Name { get; set; }

    [StringLength(200, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 200 characters")] public string? Description { get; set; }
}