using HotelWeb.Models;

namespace HotelWeb.Services;

public interface IReservationService
{
    Task<List<Reservation>> GetAllAsync();
    Task<Reservation?> GetByIdAsync(int id);
    Task<List<Reservation>> GetVisibleAsync(bool isEmployee, int? customerId);
    Task CreateAsync(int roomId, DateOnly checkIn, DateOnly checkOut, int customerId, int guestCount);

    Task ConfirmAsync(int reservationId);
    Task CancelAsync(int reservationId);
    Task MarkNoShowAsync(int reservationId);

    Task CheckInAsync(int reservationId);
    Task CheckOutAsync(int reservationId);
}