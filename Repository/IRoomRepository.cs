using HotelWeb.Models;

namespace HotelWeb.Repository.IRepository;

public interface IRoomRepository
{
    Task<List<Room>> GetAllAsync();
    Task<Room?> GetByIdAsync(int id);
    Task<List<Room>> SearchAsync(RoomSearchFilter filter);
    Task<List<RoomType>> GetRoomTypesAsync();
}