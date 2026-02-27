using HotelWeb.Models;

namespace HotelWeb.Repositories;

public interface IRoomRepository
{
    Task<List<Room>> GetAllAsync();
    Task<Room?> GetByIdAsync(int id);
    Task SaveChangesAsync();
}