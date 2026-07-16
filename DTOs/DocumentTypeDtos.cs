using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record DocumentTypeReadDto
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Key { get; set; }
    [Required]
    [MaxLength(200)]
    public string Description { get; set; }
}

public record DocumentTypeCreateDto
{
    [Required(ErrorMessage = "Document type key is required")]
    [MaxLength(100, ErrorMessage = "Document type key must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Document type key must be at least 2 characters long")]
    public string Key { get; set; }
    
    [Required(ErrorMessage = "Document type description is required")]
    [MaxLength(200, ErrorMessage = "Document type description must not exceed 200 characters")]
    [MinLength(3, ErrorMessage = "Document type description must be at least 3 characters long")]
    public string Description { get; set; }
}

public record DocumentTypeUpdateDto
{
    [Required(ErrorMessage = "Document type ID is required")]
    public int Id { get; set; }

    [MaxLength(100, ErrorMessage = "Document type key must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Document type key must be at least 2 characters long")]
    public string? Key { get; set; }
    
    [MaxLength(200, ErrorMessage = "Document type description must not exceed 200 characters")]
    [MinLength(3, ErrorMessage = "Document type description must be at least 3 characters long")]
    public string? Description { get; set; }
}
