using HotelWeb.Data;
using HotelWeb.Models;
using HotelWeb.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelWeb.Repository;

public class RoomRepository(ApplicationDbContext db) : IRoomRepository
{
    public async Task<List<Room>> GetAllAsync()
        => await db.Rooms
            .AsNoTracking()
            .Include(r => r.RoomType)
            .OrderBy(r => r.RoomNumber)
            .ToListAsync();

    public async Task<Room?> GetByIdAsync(int id)
        => await db.Rooms
            .AsNoTracking()
            .Include(r => r.RoomType)
            .FirstOrDefaultAsync(r => r.Id == id);
}