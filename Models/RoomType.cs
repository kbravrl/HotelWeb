using System.ComponentModel.DataAnnotations;

namespace HotelWeb.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        [MaxLength(80)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Description { get; set; }
        public int MaxCapacity { get; set; }
        public List<Room> Rooms { get; set; } = new();
    }
}
