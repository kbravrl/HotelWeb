using HotelWeb.Data;
using HotelWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelWeb.Repositories;

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

    public async Task<List<RoomType>> GetRoomTypesAsync()
        => await db.RoomTypes.AsNoTracking().OrderBy(x => x.Name).ToListAsync();

    public Task SaveChangesAsync() => db.SaveChangesAsync();

    public async Task<List<Room>> SearchAsync(RoomSearchFilter filter)
    {
        var q = db.Rooms.AsNoTracking().Include(r => r.RoomType).AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.RoomNo))
            q = q.Where(r => r.RoomNumber.Contains(filter.RoomNo));

        if (filter.RoomTypeId.HasValue)
            q = q.Where(r => r.RoomTypeId == filter.RoomTypeId.Value);

        if (filter.Status.HasValue)
            q = q.Where(r => r.Status == filter.Status.Value);

        if (filter.Capacity.HasValue)
            q = q.Where(r => r.Capacity >= filter.Capacity.Value);

        if (filter.MinPrice.HasValue)
            q = q.Where(r => r.BasePrice >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            q = q.Where(r => r.BasePrice <= filter.MaxPrice.Value);

        return await q.OrderBy(r => r.RoomNumber).ToListAsync();
    }
}