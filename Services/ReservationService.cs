using HotelWeb.Enums;
using HotelWeb.Models;
using HotelWeb.Repositories;

namespace HotelWeb.Services;

public class ReservationService(
    IReservationRepository reservationRepo,
    IHousekeepingTaskRepository taskRepo
) : IReservationService
{
    public Task<List<Reservation>> GetAllAsync()
        => reservationRepo.GetAllAsync();

    public async Task CheckInAsync(int reservationId)
    {
        var res = await reservationRepo.GetByIdWithRoomAsync(reservationId)
                  ?? throw new InvalidOperationException("Reservation not found.");

        if (res.Status != ReservationStatus.Confirmed)
            throw new InvalidOperationException("Only Confirmed reservations can be checked-in.");

        if (res.Room is null)
            throw new InvalidOperationException("Room not loaded.");

        if (res.Room.Status != RoomStatus.Available)
            throw new InvalidOperationException("Room is not available.");

        res.Status = ReservationStatus.CheckedIn;
        res.Room.Status = RoomStatus.Occupied;

        await reservationRepo.SaveChangesAsync();
    }

    public async Task CheckOutAsync(int reservationId)
    {
        var res = await reservationRepo.GetByIdWithRoomAsync(reservationId)
                  ?? throw new InvalidOperationException("Reservation not found.");

        if (res.Status != ReservationStatus.CheckedIn)
            throw new InvalidOperationException("Only CheckedIn reservations can be checked-out.");

        if (res.Room is null)
            throw new InvalidOperationException("Room not loaded.");

        res.Status = ReservationStatus.CheckedOut;
        res.Room.Status = RoomStatus.Cleaning;

        await taskRepo.AddAsync(new HousekeepingTask
        {
            RoomId = res.RoomId
        });

        await reservationRepo.SaveChangesAsync();
        await taskRepo.SaveChangesAsync();
    }
}