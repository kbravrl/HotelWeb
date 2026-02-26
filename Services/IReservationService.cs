using HotelWeb.Models;

namespace HotelWeb.Services;

public interface IReservationService
{
    Task<List<Reservation>> GetAllAsync();
    Task CheckInAsync(int reservationId);
    Task CheckOutAsync(int reservationId);
}