using HotelWeb.Models;
using HotelWeb.Repository.IRepository;

namespace HotelWeb.Services;

public class RoomService(IRoomRepository repo)
{
    public Task<List<Room>> GetRoomsAsync() => repo.GetAllAsync();
    public Task<Room?> GetRoomAsync(int id) => repo.GetByIdAsync(id);
}