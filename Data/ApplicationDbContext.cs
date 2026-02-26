using HotelWeb.Enums;
using HotelWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelWeb.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<RoomType> RoomTypes => Set<RoomType>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<HousekeepingTask> HousekeepingTasks => Set<HousekeepingTask>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RoomType>().HasData(
            new RoomType { Id = 1, Name = "Single", Description = "Single room", MaxCapacity = 1 },
            new RoomType { Id = 2, Name = "Double", Description = "Double room", MaxCapacity = 2 },
            new RoomType { Id = 3, Name = "Suite", Description = "Suite room", MaxCapacity = 4 }
            );

            builder.Entity<Room>().HasData(
                new Room { Id = 1, RoomNumber = "101", RoomTypeId = 1, Capacity = 1, BasePrice = 2200, Status = RoomStatus.Available },
                new Room { Id = 2, RoomNumber = "102", RoomTypeId = 1, Capacity = 1, BasePrice = 2300, Status = RoomStatus.Available },
                new Room { Id = 3, RoomNumber = "103", RoomTypeId = 1, Capacity = 1, BasePrice = 2100, Status = RoomStatus.Maintenance },

                new Room { Id = 4, RoomNumber = "201", RoomTypeId = 2, Capacity = 2, BasePrice = 3200, Status = RoomStatus.Available },
                new Room { Id = 5, RoomNumber = "202", RoomTypeId = 2, Capacity = 2, BasePrice = 3400, Status = RoomStatus.Cleaning },
                new Room { Id = 6, RoomNumber = "203", RoomTypeId = 2, Capacity = 2, BasePrice = 3600, Status = RoomStatus.Available },
                new Room { Id = 7, RoomNumber = "204", RoomTypeId = 2, Capacity = 2, BasePrice = 3500, Status = RoomStatus.Occupied },

                new Room { Id = 8, RoomNumber = "301", RoomTypeId = 3, Capacity = 4, BasePrice = 5200, Status = RoomStatus.Available },
                new Room { Id = 9, RoomNumber = "302", RoomTypeId = 3, Capacity = 4, BasePrice = 5900, Status = RoomStatus.Available },
                new Room { Id = 10, RoomNumber = "303", RoomTypeId = 3, Capacity = 4, BasePrice = 6100, Status = RoomStatus.Cleaning }
            );

            builder.Entity<Reservation>().HasData(
                new Reservation
                {
                    Id = 1,
                    RoomId = 4,
                    CheckIn = new DateOnly(2026, 2, 26),
                    CheckOut = new DateOnly(2026, 3, 1),
                    GuestCount = 2,
                    Status = ReservationStatus.Confirmed,
                    TotalPrice = 0m,
                    CreatedAt = new DateTime(2026, 2, 20, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}

