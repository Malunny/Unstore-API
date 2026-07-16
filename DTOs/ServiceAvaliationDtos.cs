using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record ServiceAvaliationReadDto
{
    [Required]
    public int ClientId { get; set; }
    
    [Required]
    public int ServiceId { get; set; }
    
    [Required]
    [Range(1, 5, ErrorMessage = "Stars must be between 1 and 5")]
    public short Stars { get; set; }
    
    [MaxLength(100)]
    public string? Title { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string Description { get; set; }
}

public record ServiceAvaliationCreateDto
{
    [Required(ErrorMessage = "Client ID is required")]
    public int ClientId { get; set; }
    
    [Required(ErrorMessage = "Service ID is required")]
    public int ServiceId { get; set; }
    
    [Required(ErrorMessage = "Stars are required")]
    [Range(1, 5, ErrorMessage = "Stars must be between 1 and 5")]
    public short Stars { get; set; }
    
    [MaxLength(100, ErrorMessage = "Title must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters long")]
    public string? Title { get; set; }
    
    [Required(ErrorMessage = "Description is required")]
    [MaxLength(500, ErrorMessage = "Description must not exceed 500 characters")]
    [MinLength(3, ErrorMessage = "Description must be at least 3 characters long")]
    public string Description { get; set; }
}
