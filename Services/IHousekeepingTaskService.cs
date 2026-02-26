using HotelWeb.Models;

namespace HotelWeb.Services;

public interface IHousekeepingTaskService
{
    Task<List<HousekeepingTask>> GetOpenTasksAsync();
    Task StartAsync(int taskId);
    Task CompleteAsync(int taskId);
}