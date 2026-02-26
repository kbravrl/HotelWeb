using HotelWeb.Enums;
using HotelWeb.Repositories;

namespace HotelWeb.Services;

public class HousekeepingTaskService(
    IHousekeepingTaskRepository taskRepo,
    IRoomRepository roomRepo
) : IHousekeepingTaskService
{
    public Task<List<HotelWeb.Models.HousekeepingTask>> GetOpenTasksAsync()
        => taskRepo.GetOpenTasksAsync();

    public async Task StartAsync(int taskId)
    {
        var task = await taskRepo.GetByIdAsync(taskId)
                   ?? throw new InvalidOperationException("Task not found.");

        if (task.Status != HousekeepingTaskStatus.Pending)
            throw new InvalidOperationException("Only Pending tasks can be started.");

        task.Status = HousekeepingTaskStatus.InProgress;
        await taskRepo.SaveChangesAsync();
    }

    public async Task CompleteAsync(int taskId)
    {
        var task = await taskRepo.GetByIdAsync(taskId)
                   ?? throw new InvalidOperationException("Task not found.");

        if (task.Status != HousekeepingTaskStatus.InProgress)
            throw new InvalidOperationException("Only InProgress tasks can be completed.");

        task.Status = HousekeepingTaskStatus.Done;
        task.CompletedAt = DateTime.UtcNow;

        var room = await roomRepo.GetByIdAsync(task.RoomId)
                   ?? throw new InvalidOperationException("Room not found.");

        if (room.Status == RoomStatus.Cleaning)
            room.Status = RoomStatus.Available;

        await taskRepo.SaveChangesAsync();
        await roomRepo.SaveChangesAsync();
    }
}