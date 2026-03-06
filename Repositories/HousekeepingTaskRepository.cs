using HotelWeb.Data;
using HotelWeb.Enums;
using HotelWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelWeb.Repositories;
public class HousekeepingTaskRepository(ApplicationDbContext db)
    : IHousekeepingTaskRepository
{
    public Task<List<HousekeepingTask>> GetAllAsync()
        => db.HousekeepingTasks
            .Include(t => t.Room)
            .Include(t => t.AssignedToEmployee)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

    public Task<HousekeepingTask?> GetByIdAsync(int id)
        => db.HousekeepingTasks
            .Include(t => t.Room)
            .Include(t => t.AssignedToEmployee)
            .FirstOrDefaultAsync(t => t.Id == id);

    public Task<List<HousekeepingTask>> GetOpenTasksAsync()
        => db.HousekeepingTasks
            .Include(t => t.Room)
            .Include(t => t.AssignedToEmployee)
            .Where(t => t.Status != HousekeepingTaskStatus.Done)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();

    public async Task AddAsync(HousekeepingTask task)
        => await db.HousekeepingTasks.AddAsync(task);

    public void Update(HousekeepingTask task)
        => db.HousekeepingTasks.Update(task);

    public void Delete(HousekeepingTask task)
        => db.HousekeepingTasks.Remove(task);

    public Task SaveChangesAsync()
        => db.SaveChangesAsync();
}