using HotelWeb.Enums;
using HotelWeb.Models;
using HotelWeb.Repositories;

namespace HotelWeb.Services;

public class HousekeepingTaskService(
    IHousekeepingTaskRepository taskRepo,
    IRoomRepository roomRepo
) : IHousekeepingTaskService
{
    public Task<List<HousekeepingTask>> GetAllTasksAsync()
        => taskRepo.GetAllAsync();

    public Task<List<HousekeepingTask>> GetOpenTasksAsync()
        => taskRepo.GetOpenTasksAsync();
    public Task<HousekeepingTask?> GetByIdAsync(int id)
        => taskRepo.GetByIdAsync(id);

    public async Task CreateTaskAsync(HousekeepingTask task)
    {
        task.CreatedAt = DateTime.UtcNow;
        task.Status = HousekeepingTaskStatus.Pending;

        var room = await roomRepo.GetByIdAsync(task.RoomId)
                   ?? throw new InvalidOperationException("Room not found.");

        if (room.Status == RoomStatus.Available)
            room.Status = RoomStatus.Cleaning;

        await taskRepo.AddAsync(task);
        await taskRepo.SaveChangesAsync();
    }

    public async Task UpdateTaskAsync(HousekeepingTask task)
    {
        var existingTask = await taskRepo.GetByIdAsync(task.Id)
                           ?? throw new InvalidOperationException("Task not found.");

        existingTask.RoomId = task.RoomId;
        existingTask.AssignedToEmployeeId = task.AssignedToEmployeeId;
        existingTask.Status = task.Status;

        if (task.CompletedAt.HasValue)
            existingTask.CompletedAt = task.CompletedAt;

        taskRepo.Update(existingTask);
        await taskRepo.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(int id)
    {
        var task = await taskRepo.GetByIdAsync(id)
                   ?? throw new InvalidOperationException("Task not found.");

        var room = await roomRepo.GetByIdAsync(task.RoomId)
                   ?? throw new InvalidOperationException("Room not found.");

        if (room.Status == RoomStatus.Cleaning)
            room.Status = RoomStatus.Available;

        taskRepo.Delete(task);
        await taskRepo.SaveChangesAsync();
    }

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
        task.AssignedToEmployee = null;

        var room = await roomRepo.GetByIdAsync(task.RoomId)
                   ?? throw new InvalidOperationException("Room not found.");

        if (room.Status == RoomStatus.Cleaning)
            room.Status = RoomStatus.Available;

        await roomRepo.SaveChangesAsync();
    }
}