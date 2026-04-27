using System.ComponentModel.DataAnnotations;

namespace Unstore.DTO;

public record EmployeeUpdateDto
{
    [Required(ErrorMessage = "Employee ID is required.")] public int Id { get; set; }

    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")] public string? Name { get; set; }

    [Phone(ErrorMessage = "Phone number not valid.")] public string? ContactNumber { get; set; }

    public int? PositionId { get; set; }

    public DateTime? StartedAt { get; set; }

    public bool? Active { get; set; }
}