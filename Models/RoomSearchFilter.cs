using HotelWeb.Enums;

namespace HotelWeb.Models;

public class RoomSearchFilter
{
    public string? RoomNo { get; set; }
    public int? RoomTypeId { get; set; }
    public int? Capacity { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public RoomStatus? Status { get; set; }
}