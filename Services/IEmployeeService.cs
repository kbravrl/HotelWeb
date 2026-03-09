using HotelWeb.Models;

namespace HotelWeb.Services;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllEmployeesAsync();
    Task<List<Employee>> GetAllEmployeesWithTasksAsync();
    Task<Employee?> GetEmployeeAsync(int id);
    Task<Employee?> GetEmployeeByEmailAsync(string email);
    Task<List<Employee>> GetActiveEmployeesAsync();
    Task<List<Employee>> GetCleanersAsync();
    Task CreateEmployeeAsync(Employee employee);
    Task CreateEmployeeWithUserAsync(Employee employee, string password, string role);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(int id);
}
