using HotelWeb.Enums;

namespace HotelWeb.Models;

public class RoomSearchFilter
{
    public DateOnly? CheckIn { get; set; }
    public DateOnly? CheckOut { get; set; }
    public int? GuestCount { get; set; }
    public int? RoomTypeId { get; set; }
    public RoomStatus? Status { get; set; }
    public string? RoomNumberQuery { get; set; }
}