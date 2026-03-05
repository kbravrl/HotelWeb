using HotelWeb.Models;

namespace HotelWeb.Services;

public interface IRoomService
{
    Task<List<Room>> GetAllAsync();
    Task<Room?> GetRoomAsync(int id);
    Task<Room> CreateAsync(Room room);
    Task UpdateAsync(Room room);
    Task DeleteAsync(int id);
}