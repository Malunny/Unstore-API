using System.ComponentModel.DataAnnotations;

namespace Unstore.DTO;

public record ServiceUpdateDto
{
    [Required(ErrorMessage = "Service ID is required.")]
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public int? EmployeeId { get; set; }

    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Details must be between 10 and 1000 characters")]
    public string? Details { get; set; }

    [StringLength(150, MinimumLength = 3, ErrorMessage = "Address must be between 3 and 150 characters")]
    public string? Address { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Service Cost can not be negative.")]

    public IEnumerable<int>? ToolsIds { get; set; }
    public IEnumerable<int>? ProductsIds { get; set; }
}