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
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<Room> CreateAsync(Room room)
    {
        if (await IsRoomNumberExistsAsync(room.RoomNumber))
        {
            throw new InvalidOperationException($"Room number '{room.RoomNumber}' already exists.");
        }

        db.Rooms.Add(room);
        await db.SaveChangesAsync();
        return room;
    }

    public async Task UpdateAsync(Room room)
    {
        if (await IsRoomNumberExistsAsync(room.RoomNumber, room.Id))
        {
            throw new InvalidOperationException($"Room number '{room.RoomNumber}' already exists.");
        }

        db.Rooms.Update(room);
        await db.SaveChangesAsync();
    }

    private async Task<bool> IsRoomNumberExistsAsync(string roomNumber, int? excludeRoomId = null)
    {
        var query = db.Rooms.Where(r => r.RoomNumber == roomNumber);

        if (excludeRoomId.HasValue)
        {
            query = query.Where(r => r.Id != excludeRoomId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var room = await db.Rooms.FindAsync(id);
        if (room == null)
        {
            throw new InvalidOperationException($"Room with ID {id} not found.");
        }

        if (!await CanDeleteAsync(id))
        {
            throw new InvalidOperationException($"Room '{room.RoomNumber}' cannot be deleted because it has existing reservations or housekeeping tasks.");
        }

        db.Rooms.Remove(room);
        await db.SaveChangesAsync();
    }
    private async Task<bool> CanDeleteAsync(int id)
    {
        var hasReservations = await db.Reservations.AnyAsync(r => r.RoomId == id);
        var hasHousekeepingTasks = await db.HousekeepingTasks.AnyAsync(h => h.RoomId == id);

        return !hasReservations && !hasHousekeepingTasks;
    }

    public Task SaveChangesAsync() => db.SaveChangesAsync();

}