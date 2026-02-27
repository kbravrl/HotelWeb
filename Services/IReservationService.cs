using HotelWeb.Models;

namespace HotelWeb.Services;

public interface IReservationService
{
    Task<List<Reservation>> GetAllAsync();
    Task<Reservation?> GetByIdAsync(int id);
    Task<List<Reservation>> GetVisibleAsync(bool isEmployee, string? customerId);
    Task CreateAsync(int roomId, DateOnly checkIn, DateOnly checkOut, string customerId);

    Task ConfirmAsync(int reservationId);
    Task CancelAsync(int reservationId);
    Task MarkNoShowAsync(int reservationId);

    Task CheckInAsync(int reservationId);
    Task CheckOutAsync(int reservationId);
}