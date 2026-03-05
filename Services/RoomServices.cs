using HotelWeb.Models;
using HotelWeb.Repositories;

namespace HotelWeb.Services;

public class RoomService(IRoomRepository repo) : IRoomService
{
    public Task<List<Room>> GetAllAsync()
        => repo.GetAllAsync();

    public Task<Room?> GetRoomAsync(int id)
        => repo.GetByIdAsync(id);

    public Task<Room> CreateAsync(Room room)
        => repo.CreateAsync(room);

    public Task UpdateAsync(Room room)
        => repo.UpdateAsync(room);

    public Task DeleteAsync(int id)
        => repo.DeleteAsync(id);
}