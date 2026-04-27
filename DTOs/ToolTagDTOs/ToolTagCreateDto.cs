using System.ComponentModel.DataAnnotations;

namespace Unstore.DTO;

public record ToolTagCreateDto
{
    [Required(ErrorMessage = "Tag name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Tag name must be between 2 and 50 characters")]
    public string TagName { get; set; }
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 200 characters")]
    public string Description { get; set; }
}