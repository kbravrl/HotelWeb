using HotelWeb.Models;

namespace HotelWeb.Services;

public interface IReservationService
{
    Task<List<Reservation>> GetAllAsync();
    Task<Reservation?> GetByIdAsync(int id);
    Task CreateAsync(int roomId, DateOnly checkIn, DateOnly checkOut);
    Task CheckInAsync(int reservationId);
    Task CheckOutAsync(int reservationId);
}