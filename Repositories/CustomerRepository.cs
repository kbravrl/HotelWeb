using HotelWeb.Data;
using HotelWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelWeb.Repositories;

public class CustomerRepository(ApplicationDbContext db) : ICustomerRepository
{
    public async Task<List<Customer>> GetAllAsync()
        => await db.Customers
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

    public async Task<Customer?> GetByIdAsync(int id)
        => await db.Customers
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Customer?> GetByEmailAsync(string email)
        => await db.Customers
            .FirstOrDefaultAsync(c => c.Email == email);

    public async Task AddAsync(Customer customer)
    {
        await db.Customers.AddAsync(customer);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        db.Customers.Update(customer);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await GetByIdAsync(id);
        if (customer != null)
        {
            db.Customers.Remove(customer);
            await SaveChangesAsync();
        }
    }

    public Task SaveChangesAsync() => db.SaveChangesAsync();
}
