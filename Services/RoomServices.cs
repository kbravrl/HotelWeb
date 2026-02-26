using HotelWeb.Models;
using HotelWeb.Repositories;

namespace HotelWeb.Services;

public class RoomService(IRoomRepository repo) : IRoomService
{
    public Task<List<Room>> GetAllAsync()
        => repo.GetAllAsync();

    public Task<Room?> GetRoomAsync(int id)
        => repo.GetByIdAsync(id);

    public Task<List<Room>> SearchRoomsAsync(RoomSearchFilter filter)
        => repo.SearchAsync(filter);

    public Task<List<RoomType>> GetRoomTypesAsync()
        => repo.GetRoomTypesAsync();
}