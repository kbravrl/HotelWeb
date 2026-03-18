using HotelWeb.Models;

namespace HotelWeb.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee?> GetByApplicationUserIdAsync(string applicationUserId);
    Task<List<Employee>> GetCleanersAsync();
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
