using HotelWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelWeb.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RoomType>(e =>
            {
                e.Property(x => x.Name).HasMaxLength(80).IsRequired();
                e.Property(x => x.Description).HasMaxLength(500);
                e.Property(x => x.MaxCapacity).IsRequired();
            });

            builder.Entity<Room>(e =>
            {
                e.Property(x => x.RoomNumber).IsRequired();
                e.HasIndex(x => x.RoomNumber).IsUnique();

                e.Property(x => x.BasePrice).HasColumnType("numeric(10,2)");
                e.Property(x => x.Capacity).IsRequired();

                e.HasOne(x => x.RoomType)
                 .WithMany(x => x.Rooms)
                 .HasForeignKey(x => x.RoomTypeId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
