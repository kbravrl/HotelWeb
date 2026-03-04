using HotelWeb.Data;
using HotelWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelWeb.Repositories;

public class EmployeeRepository(ApplicationDbContext db) : IEmployeeRepository
{
    public async Task<List<Employee>> GetAllAsync()
        => await db.Employees
            .AsNoTracking()
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();

    public async Task<List<Employee>> GetAllWithTasksAsync()
        => await db.Employees
            .Include(e => e.AssignedTasks)
            .AsNoTracking()
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();

    public async Task<Employee?> GetByIdAsync(int id)
        => await db.Employees
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<Employee?> GetByEmailAsync(string email)
        => await db.Employees
            .FirstOrDefaultAsync(e => e.Email == email);

    public async Task<List<Employee>> GetActiveEmployeesAsync()
        => await db.Employees
            .Where(e => e.IsActive)
            .AsNoTracking()
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .ToListAsync();

    public async Task AddAsync(Employee employee)
    {
        await db.Employees.AddAsync(employee);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        db.Employees.Update(employee);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await GetByIdAsync(id);
        if (employee != null)
        {
            db.Employees.Remove(employee);
            await SaveChangesAsync();
        }
    }

    public Task SaveChangesAsync() => db.SaveChangesAsync();
}
