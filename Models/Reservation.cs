using HotelWeb.Enums;

namespace HotelWeb.Models;

public class Reservation
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public Room? Room { get; set; }
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public int GuestCount { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}