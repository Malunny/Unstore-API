using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record ProductAvaliationReadDto
{
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string Description { get; set; }
    
    [Required]
    [Range(1, 5, ErrorMessage = "Stars must be between 1 and 5")]
    public short Stars { get; set; }
}

public record ProductAvaliationCreateDto
{
    [Required(ErrorMessage = "User ID is required")]
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "Product ID is required")]
    public int ProductId { get; set; }
    
    [Required(ErrorMessage = "Description is required")]
    [MaxLength(500, ErrorMessage = "Description must not exceed 500 characters")]
    [MinLength(3, ErrorMessage = "Description must be at least 3 characters long")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Stars are required")]
    [Range(1, 5, ErrorMessage = "Stars must be between 1 and 5")]
    public short Stars { get; set; }
}
