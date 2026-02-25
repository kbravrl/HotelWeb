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

    public async Task<List<RoomType>> GetRoomTypesAsync()
        => await db.RoomTypes.AsNoTracking().OrderBy(x => x.Name).ToListAsync();

    public async Task<List<Room>> SearchAsync(RoomSearchFilter filter)
    {
        var q = db.Rooms.AsNoTracking().Include(r => r.RoomType).AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.RoomNumberQuery))
            q = q.Where(r => r.RoomNumber.Contains(filter.RoomNumberQuery.Trim()));

        if (filter.RoomTypeId.HasValue)
            q = q.Where(r => r.RoomTypeId == filter.RoomTypeId.Value);

        if (filter.Status.HasValue)
            q = q.Where(r => r.Status == filter.Status.Value);

        if (filter.GuestCount.HasValue)
            q = q.Where(r => r.Capacity >= filter.GuestCount.Value);

        if (filter.CheckIn.HasValue && filter.CheckOut.HasValue)
        {
            var checkIn = filter.CheckIn.Value;
            var checkOut = filter.CheckOut.Value;

            if (checkOut <= checkIn)
                return new List<Room>();

            q = q.Where(room =>
                !db.Reservations.Any(res =>
                    res.RoomId == room.Id &&
                    res.Status != HotelWeb.Enums.ReservationStatus.Cancelled &&
                    checkIn < res.CheckOut &&
                    checkOut > res.CheckIn
                )
            );
        }


        return await q.OrderBy(r => r.RoomNumber).ToListAsync();
    }
}