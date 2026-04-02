using System.ComponentModel.DataAnnotations;

namespace Unstore.ViewModels;

public class EditorClientViewModel
{
    [Required]
    [MinLength(3)]
    public string Name { get; init; }
    [Required]
    [MinLength(3)]
    public string Address { get; init; }
    [Required]
    [EmailAddress]
    public string Email { get; init; }
    [Required]
    [MinLength(8)]
    public string ContactNumber { get; init; }
}