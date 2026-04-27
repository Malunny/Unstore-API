using System.ComponentModel.DataAnnotations;

namespace Unstore.DTO;

public record ToolTagUpdateDto
{
    [Required(ErrorMessage = "Tool tag ID is required.")] public int Id { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Tag name must be between 2 and 50 characters")]
     public string? TagName { get; set; }

    [StringLength(200, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 200 characters")]
     public string? Description { get; set; }
}