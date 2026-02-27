using HotelWeb.Models;

namespace HotelWeb.Services;

public interface IRoomService
{
    Task<List<Room>> GetAllAsync();
    Task<Room?> GetRoomAsync(int id);
}