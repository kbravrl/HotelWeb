using HotelWeb.Data;
using HotelWeb.Models;
using HotelWeb.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelWeb.Repository;

public class RoomRepository(ApplicationDbContext db) : IRoomRepository
{
    public Task<List<Room>> GetAllAsync()
        => db.Rooms.Include(x => x.RoomType).ToListAsync();

    public Task<Room?> GetByIdAsync(int id)
        => db.Rooms.Include(x => x.RoomType).FirstOrDefaultAsync(x => x.Id == id);
}