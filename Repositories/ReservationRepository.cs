using HotelWeb.Data;
using HotelWeb.Enums;
using HotelWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelWeb.Repositories;

public class ReservationRepository(ApplicationDbContext db) : IReservationRepository
{
    public Task<List<Reservation>> GetAllAsync()
        => db.Reservations
            .Include(r => r.Room)
            .OrderByDescending(r => r.Id)
            .ToListAsync();
    public Task<Reservation?> GetByIdAsync(int id)
    => db.Reservations
        .Include(r => r.Room)
            .ThenInclude(room => room.RoomType)
        .FirstOrDefaultAsync(r => r.Id == id);

    public Task<Reservation?> GetByIdWithRoomAsync(int id)
        => db.Reservations
            .Include(r => r.Room)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task AddAsync(Reservation reservation)
        => await db.Reservations.AddAsync(reservation);

    public Task<bool> HasOverlapAsync(int roomId, DateOnly checkIn, DateOnly checkOut)
        => db.Reservations.AnyAsync(r =>
            r.RoomId == roomId
            && r.Status != ReservationStatus.Cancelled
            && r.Status != ReservationStatus.NoShow
            && checkIn < r.CheckOut
            && checkOut > r.CheckIn
        );

    public Task SaveChangesAsync() => db.SaveChangesAsync();
}