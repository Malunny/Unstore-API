using System.ComponentModel.DataAnnotations;

namespace Unstore.ViewModels;

public class UpdateClientViewModel
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }
    public string? Name { get; init; }
    public string? Address { get; init; }
    public string? Email { get; init; }
    public string? ContactNumber { get; init; }
}