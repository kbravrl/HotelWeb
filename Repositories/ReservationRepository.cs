using HotelWeb.Data;
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

    public Task<Reservation?> GetByIdWithRoomAsync(int id)
        => db.Reservations
            .Include(r => r.Room)
            .FirstOrDefaultAsync(r => r.Id == id);

    public Task SaveChangesAsync() => db.SaveChangesAsync();
}