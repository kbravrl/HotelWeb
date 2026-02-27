using HotelWeb.Data;
using HotelWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelWeb.Repositories;

public class RoomRepository(ApplicationDbContext db) : IRoomRepository
{
    public async Task<List<Room>> GetAllAsync()
        => await db.Rooms
            .AsNoTracking()
            .OrderBy(r => r.RoomNumber)
            .ToListAsync();

    public async Task<Room?> GetByIdAsync(int id)
        => await db.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

    public Task SaveChangesAsync() => db.SaveChangesAsync();
}