using HotelWeb.Models;

namespace HotelWeb.Services;

public interface IHousekeepingTaskService
{
    Task<List<HousekeepingTask>> GetAllTasksAsync();
    Task<List<HousekeepingTask>> GetOpenTasksAsync();
    Task<HousekeepingTask?> GetTaskByIdAsync(int id);
    Task CreateTaskAsync(HousekeepingTask task);
    Task UpdateTaskAsync(HousekeepingTask task);
    Task DeleteTaskAsync(int id);
    Task StartAsync(int taskId);
    Task CompleteAsync(int taskId);
}