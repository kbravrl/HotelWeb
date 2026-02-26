using HotelWeb.Enums;

namespace HotelWeb.Models;

public class HousekeepingTask
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public Room? Room { get; set; }
    public HousekeepingTaskStatus Status { get; set; } = HousekeepingTaskStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}