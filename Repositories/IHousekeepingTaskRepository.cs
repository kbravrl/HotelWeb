using HotelWeb.Models;

namespace HotelWeb.Repositories;

public interface IHousekeepingTaskRepository
{
    Task<List<HousekeepingTask>> GetAllAsync();
    Task<HousekeepingTask?> GetByIdAsync(int id);
    Task<List<HousekeepingTask>> GetOpenTasksAsync();
    Task AddAsync(HousekeepingTask task);
    void Update(HousekeepingTask task);
    void Delete(HousekeepingTask task);
    Task SaveChangesAsync();
}