using System.ComponentModel.DataAnnotations;

namespace Unstore.DTO;

public record ProductUpdateDto
{
    [Required(ErrorMessage = "Product ID is required.")] public int Id { get; set; }

    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")] public string? Name { get; set; }

    [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")] public string? Description { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Value must be positive")] public decimal? Value { get; set; }

    [Url(ErrorMessage = "Invalid URL format")] public string? ImageUrl { get; set; }
}