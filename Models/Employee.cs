using HotelWeb.Data;
using HotelWeb.Enums;

namespace HotelWeb.Models;

public class Employee
{
    public int Id { get; set; }
    public string? ApplicationUserId { get; set; } = default!;
    public ApplicationUser? ApplicationUser { get; set; }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public EmployeeRole Role { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public DateOnly? HireDate { get; set; }
    public decimal? Salary { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<HousekeepingTask> AssignedTasks { get; set; } = new List<HousekeepingTask>();
}
