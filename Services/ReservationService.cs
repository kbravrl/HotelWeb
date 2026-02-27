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

    public Task<List<Reservation>> GetVisibleAsync(bool isEmployee, string? customerId)
    {
        if (isEmployee)
            return reservationRepo.GetAllAsync();

        if (string.IsNullOrWhiteSpace(customerId))
            return Task.FromResult(new List<Reservation>());

        return reservationRepo.GetAllForCustomerAsync(customerId);
    }

    public async Task CreateAsync(int roomId, DateOnly checkIn, DateOnly checkOut, string customerId)
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
            Status = ReservationStatus.Pending,
            CustomerId = customerId
        });

        await reservationRepo.SaveChangesAsync();
    }

    public async Task ConfirmAsync(int reservationId)
    {
        var res = await reservationRepo.GetByIdWithRoomAsync(reservationId)
                  ?? throw new InvalidOperationException("Reservation not found.");

        if (res.Status != ReservationStatus.Pending)
            throw new InvalidOperationException("Only Pending reservations can be confirmed.");

        if (res.Room is null)
            throw new InvalidOperationException("Room not loaded.");

        if (res.Room.Status != RoomStatus.Available)
            throw new InvalidOperationException("Room is not available.");

        res.Status = ReservationStatus.Confirmed;

        await reservationRepo.SaveChangesAsync();
    }

    public async Task CancelAsync(int reservationId)
    {
        var res = await reservationRepo.GetByIdWithRoomAsync(reservationId)
                  ?? throw new InvalidOperationException("Reservation not found.");

        if (res.Status == ReservationStatus.CheckedIn || res.Status == ReservationStatus.CheckedOut)
            throw new InvalidOperationException("This reservation can no longer be cancelled.");


        if (res.Status == ReservationStatus.Cancelled || res.Status == ReservationStatus.NoShow)
            return;

        res.Status = ReservationStatus.Cancelled;

        await reservationRepo.SaveChangesAsync();
    }

    public async Task MarkNoShowAsync(int reservationId)
    {
        var res = await reservationRepo.GetByIdWithRoomAsync(reservationId)
                  ?? throw new InvalidOperationException("Reservation not found.");

        if (res.Status != ReservationStatus.Confirmed)
            throw new InvalidOperationException("Only Confirmed reservations can be marked as NoShow.");

        res.Status = ReservationStatus.NoShow;

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