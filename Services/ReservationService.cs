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
    public Task<Reservation?> GetByIdAsync(int id)
        => reservationRepo.GetByIdAsync(id);
    public async Task CreateAsync(int roomId, DateOnly checkIn, DateOnly checkOut)
    {
        if (checkOut <= checkIn)
            throw new InvalidOperationException("Check-out must be after check-in.");

        var hasOverlap = await reservationRepo.HasOverlapAsync(roomId, checkIn, checkOut);
        if (hasOverlap)
            throw new InvalidOperationException("This room is not available for selected dates.");

        await reservationRepo.AddAsync(new Reservation
        {
            RoomId = roomId,
            CheckIn = checkIn,
            CheckOut = checkOut,
            Status = ReservationStatus.Pending
        });

        await reservationRepo.SaveChangesAsync();
    }
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
    }
}