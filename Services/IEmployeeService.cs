using HotelWeb.Models;

namespace HotelWeb.Services;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllEmployeesAsync();
    Task<Employee?> GetEmployeeAsync(int id);
    Task<Employee?> GetByApplicationUserIdAsync(string applicationUserId);
    Task<List<Employee>> GetCleanersAsync();
    Task CreateEmployeeAsync(Employee employee);
    Task CreateEmployeeWithUserAsync(Employee employee, string password, string role);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(int id);
}
