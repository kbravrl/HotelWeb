using HotelWeb.Enums;

namespace HotelWeb.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public RoomType RoomType { get; set; }
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Available;
    }
}
