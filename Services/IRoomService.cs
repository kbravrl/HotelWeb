using HotelWeb.Models;

namespace HotelWeb.Services;

public interface IRoomService
{
    Task<List<Room>> GetAllAsync();
    Task<Room?> GetRoomAsync(int id);
    Task<List<Room>> SearchRoomsAsync(RoomSearchFilter filter);
    Task<List<RoomType>> GetRoomTypesAsync();
}