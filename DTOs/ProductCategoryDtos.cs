using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record ProductCategoryReadDto
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Key { get; set; }
    
    [Required]
    [MaxLength(250)]
    public string Description { get; set; }
}

public record ProductCategoryCreateDto
{
    [Required(ErrorMessage = "Category key is required")]
    [MaxLength(100, ErrorMessage = "Category key must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Category key must be at least 2 characters long")]
    public string Key { get; set; }
    
    [Required(ErrorMessage = "Category description is required")]
    [MaxLength(250, ErrorMessage = "Category description must not exceed 250 characters")]
    [MinLength(3, ErrorMessage = "Category description must be at least 3 characters long")]
    public string Description { get; set; }
}

public record ProductCategoryUpdateDto
{
    [Required(ErrorMessage = "Category ID is required")]
    public int Id { get; set; }

    [MaxLength(100, ErrorMessage = "Category key must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Category key must be at least 2 characters long")]
    public string? Key { get; set; }
    
    [MaxLength(250, ErrorMessage = "Category description must not exceed 250 characters")]
    [MinLength(3, ErrorMessage = "Category description must be at least 3 characters long")]
    public string? Description { get; set; }
}
