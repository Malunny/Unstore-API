using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record UserDocumentReadDto
{
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public string Information { get; set; }
    [Required]
    public int DocumentTypeId { get; set; }
}

public record UserDocumentCreateDto
{
    [Required(ErrorMessage = "User ID is required")]
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "Information is required")]
    public string Information { get; set; }
    
    [Required(ErrorMessage = "Document Type ID is required")]
    public int DocumentTypeId { get; set; }
}

public record UserDocumentUpdateDto
{
    [Required(ErrorMessage = "User Document ID is required")]
    public int Id { get; set; }

    public string? Information { get; set; }
    
    public int? DocumentTypeId { get; set; }
}