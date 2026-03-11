using System.ComponentModel.DataAnnotations;
using HotelWeb.Data;
using HotelWeb.Enums;
namespace HotelWeb.Models;

public class Customer
{
    public int Id { get; set; }

    public string ApplicationUserId { get; set; } = default!;
    public ApplicationUser ApplicationUser { get; set; } = default!;

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = string.Empty;

    public DateOnly? DateOfBirth { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public int TotalStays { get; set; } = 0;

    public DateTime? LastStayDate { get; set; }

    [StringLength(500)]
    public string? Preferences { get; set; }

    public LoyaltyLevel LoyaltyLevel { get; set; } = LoyaltyLevel.Standard;
}