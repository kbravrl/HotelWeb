using HotelWeb.Models;

namespace HotelWeb.Repositories;

public interface IReservationRepository
{
    Task<List<Reservation>> GetAllAsync();
    Task<Reservation?> GetByIdWithRoomAsync(int id);
    Task SaveChangesAsync();
}