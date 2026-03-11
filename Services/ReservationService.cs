using HotelWeb.Enums;
using HotelWeb.Models;
using HotelWeb.Repositories;

namespace HotelWeb.Services;

public class ReservationService(
    IReservationRepository reservationRepo,
    IHousekeepingTaskRepository taskRepo,
    IRoomRepository roomRepo,
    ICustomerRepository customerRepo
) : IReservationService
{
    public Task<List<Reservation>> GetAllAsync()
        => reservationRepo.GetAllAsync();

    public Task<Reservation?> GetByIdAsync(int id)
        => reservationRepo.GetByIdAsync(id);

    public Task<List<Reservation>> GetVisibleAsync(bool isEmployee, int? customerId)
    {
        if (isEmployee)
            return reservationRepo.GetAllAsync();

        if (customerId == null || customerId == 0)
            return Task.FromResult(new List<Reservation>());

        return reservationRepo.GetAllForCustomerAsync(customerId.Value);
    }

    public async Task CreateAsync(int roomId, DateOnly checkIn, DateOnly checkOut, int customerId, int guestCount)
    {
        if (checkOut <= checkIn)
            throw new InvalidOperationException("Check-out must be after check-in.");

        if (guestCount <= 0)
            throw new InvalidOperationException("Guest count must be at least 1.");

        var room = await roomRepo.GetByIdAsync(roomId);
        if (room is null)
            throw new InvalidOperationException("Room not found.");

        if (guestCount > room.Capacity)
            throw new InvalidOperationException($"Guest count exceeds room capacity ({room.Capacity}).");

        var hasOverlap = await reservationRepo.HasOverlapAsync(roomId, checkIn, checkOut);
        if (hasOverlap)
            throw new InvalidOperationException("This room is not available for selected dates.");

        var numberOfNights = checkOut.DayNumber - checkIn.DayNumber;
        var totalPrice = numberOfNights * room.BasePrice;

        await reservationRepo.AddAsync(new Reservation
        {
            RoomId = roomId,
            CheckIn = checkIn,
            CheckOut = checkOut,
            Status = ReservationStatus.Pending,
            CustomerId = customerId,
            GuestCount = guestCount,
            TotalPrice = totalPrice
        });

        await reservationRepo.SaveChangesAsync();
    }

    public async Task UpdateAsync(int reservationId, DateOnly checkIn, DateOnly checkOut, int guestCount)
    {
        if (checkOut <= checkIn)
            throw new InvalidOperationException("Check-out must be after check-in.");

        if (guestCount <= 0)
            throw new InvalidOperationException("Guest count must be at least 1.");

        var res = await reservationRepo.GetByIdAsync(reservationId)
                  ?? throw new InvalidOperationException("Reservation not found.");

        if (res.Status != ReservationStatus.Pending && res.Status != ReservationStatus.Confirmed)
            throw new InvalidOperationException("Only Pending or Confirmed reservations can be updated.");

        if (res.Room is null)
            throw new InvalidOperationException("Room not loaded.");

        if (guestCount > res.Room.Capacity)
            throw new InvalidOperationException($"Guest count exceeds room capacity ({res.Room.Capacity}).");

        if (res.CheckIn != checkIn || res.CheckOut != checkOut)
        {
            var hasOverlap = await reservationRepo.HasOverlapAsync(res.RoomId, checkIn, checkOut, excludeReservationId: reservationId);
            if (hasOverlap)
                throw new InvalidOperationException("This room is not available for selected dates.");
        }

        res.CheckIn = checkIn;
        res.CheckOut = checkOut;
        res.GuestCount = guestCount;

        var numberOfNights = checkOut.DayNumber - checkIn.DayNumber;
        res.TotalPrice = numberOfNights * res.Room.BasePrice;

        await reservationRepo.SaveChangesAsync();
    }

    public async Task ConfirmAsync(int reservationId)
    {
        var res = await reservationRepo.GetByIdAsync(reservationId)
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
        var res = await reservationRepo.GetByIdAsync(reservationId)
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
        var res = await reservationRepo.GetByIdAsync(reservationId)
                  ?? throw new InvalidOperationException("Reservation not found.");

        if (res.Status != ReservationStatus.Confirmed)
            throw new InvalidOperationException("Only Confirmed reservations can be marked as NoShow.");

        res.Status = ReservationStatus.NoShow;

        await reservationRepo.SaveChangesAsync();
    }

    public async Task CheckInAsync(int reservationId)
    {
        var res = await reservationRepo.GetByIdAsync(reservationId)
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
        var res = await reservationRepo.GetByIdAsync(reservationId)
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

        if (res.CustomerId != 0)
        {
            var customer = await customerRepo.GetByIdAsync(res.CustomerId);

            if (customer != null)
            {
                customer.TotalStays += 1;
                customer.LastStayDate = DateTime.UtcNow;
                customer.LoyaltyLevel = CalculateLoyaltyLevel(customer.TotalStays);
            }
        }

        await reservationRepo.SaveChangesAsync();
    }

    private static LoyaltyLevel CalculateLoyaltyLevel(int totalStays)
    {
        return totalStays switch
        {
            >= 20 => LoyaltyLevel.Platinum,
            >= 10 => LoyaltyLevel.Gold,
            >= 5 => LoyaltyLevel.Silver,
            _ => LoyaltyLevel.Standard
        };
    }
}