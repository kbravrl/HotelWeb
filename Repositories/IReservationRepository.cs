using HotelWeb.Models;

namespace HotelWeb.Repositories;

public interface IReservationRepository
{
    Task<List<Reservation>> GetAllAsync();
    Task<Reservation?> GetByIdAsync(int id);
    Task<Reservation?> GetByIdWithRoomAsync(int id);
    Task<List<Reservation>> GetAllForCustomerAsync(string customerId);
    Task AddAsync(Reservation reservation); 
    Task<bool> HasOverlapAsync(int roomId, DateOnly checkIn, DateOnly checkOut); 
    Task SaveChangesAsync();
}