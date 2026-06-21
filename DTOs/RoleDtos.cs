using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record RoleReadDto
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(500)]
    public string Description { get; set; }
}

public record RoleCreateDto
{
    [Required(ErrorMessage = "Role name is required")]
    [MaxLength(100, ErrorMessage = "Role name must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Role name must be at least 2 characters long")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Role description is required")]
    [MaxLength(500, ErrorMessage = "Role description must not exceed 500 characters")]
    [MinLength(5, ErrorMessage = "Role description must be at least 5 characters long")]
    public string Description { get; set; }
}

public record RoleUpdateDto
{
    [Required(ErrorMessage = "Role ID is required")]
    public int Id { get; set; }

    [MaxLength(100, ErrorMessage = "Role name must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Role name must be at least 2 characters long")]
    public string? Name { get; set; }
    
    [MaxLength(500, ErrorMessage = "Role description must not exceed 500 characters")]
    [MinLength(5, ErrorMessage = "Role description must be at least 5 characters long")]
    public string? Description { get; set; }
}
