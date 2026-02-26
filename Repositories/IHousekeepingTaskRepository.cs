using HotelWeb.Models;

namespace HotelWeb.Repositories;

public interface IHousekeepingTaskRepository
{
    Task AddAsync(HousekeepingTask task);
    Task<HousekeepingTask?> GetByIdAsync(int id);
    Task<List<HousekeepingTask>> GetOpenTasksAsync();
    Task SaveChangesAsync();
}