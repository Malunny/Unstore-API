using System.ComponentModel.DataAnnotations;

namespace Unstore.DTO;

public record ClientUpdateDto
{
    [Required(ErrorMessage = "Client ID is required.")]
    public int Id { get; set; }
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
    public string Name { get; set; }
    [StringLength(150, MinimumLength = 3, ErrorMessage = "Address must be between 3 and 150 characters")]
    public string Address { get; set; }
    [Phone(ErrorMessage = "Phone number not valid.")]
    public string ContactNumber { get; set; }
}