using HotelWeb.Auth;
using HotelWeb.Data;
using HotelWeb.Enums;
using HotelWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelWeb.Repositories;

public class EmployeeRepository(ApplicationDbContext db) : IEmployeeRepository
{
    public async Task<List<Employee>> GetAllAsync()
        => await db.Employees
            .Include(e => e.AssignedTasks)
            .AsNoTracking()
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();

    public async Task<Employee?> GetByIdAsync(int id)
        => await db.Employees
            .Include(e => e.AssignedTasks)
                .ThenInclude(t => t.Room)
            .FirstOrDefaultAsync(e => e.Id == id);


    public async Task<List<Employee>> GetCleanersAsync()
    {
        return await db.Employees
            .Where(e => e.Role == EmployeeRole.Housekeeping && e.IsActive)
            .AsNoTracking()
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .ToListAsync();
    }

    public async Task AddAsync(Employee employee)
    {
        await db.Employees.AddAsync(employee);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        var existingEmployee = await db.Employees
            .Include(e => e.AssignedTasks)
            .FirstOrDefaultAsync(e => e.Id == employee.Id);

        if (existingEmployee != null)
        {
            db.Entry(existingEmployee).CurrentValues.SetValues(employee);
            await SaveChangesAsync();
        }
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
