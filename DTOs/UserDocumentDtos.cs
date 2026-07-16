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
    [MaxLength(50)]
    public string Information { get; set; }
    
    [Required(ErrorMessage = "Document Type ID is required")]
    public int DocumentTypeId { get; set; }
}

public record UserDocumentUpdateDto
{
    [Required]
    [MaxLength(50)]
    public string Information { get; set; }
    [Required]
    public int DocumentTypeId { get; set; }
}