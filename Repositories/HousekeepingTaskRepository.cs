using HotelWeb.Data;
using HotelWeb.Enums;
using HotelWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelWeb.Repositories;
public class HousekeepingTaskRepository(ApplicationDbContext db)
    : IHousekeepingTaskRepository
{
    public async Task AddAsync(HousekeepingTask task)
        => await db.HousekeepingTasks.AddAsync(task);

    public Task<HousekeepingTask?> GetByIdAsync(int id)
        => db.HousekeepingTasks
            .Include(t => t.Room)
            .FirstOrDefaultAsync(t => t.Id == id);

    public Task<List<HousekeepingTask>> GetOpenTasksAsync()
        => db.HousekeepingTasks
            .Include(t => t.Room)
            .Where(t => t.Status != HousekeepingTaskStatus.Done)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();

    public Task SaveChangesAsync()
        => db.SaveChangesAsync();
}