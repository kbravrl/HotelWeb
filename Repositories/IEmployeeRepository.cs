using HotelWeb.Models;

namespace HotelWeb.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();
    Task<List<Employee>> GetAllWithTasksAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee?> GetByEmailAsync(string email);
    Task<List<Employee>> GetActiveEmployeesAsync();
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
