using System.ComponentModel.DataAnnotations;

namespace HotelWeb.Models.InputModels;

public class InputModel
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(500)]
    public string? Preferences { get; set; }
}