using HotelWeb.Models;

namespace HotelWeb.Repositories;

public interface IRoomRepository
{
    Task<List<Room>> GetAllAsync();
    Task<Room?> GetByIdAsync(int id);
    Task<Room> CreateAsync(Room room);
    Task UpdateAsync(Room room);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}