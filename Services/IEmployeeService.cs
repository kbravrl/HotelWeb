using HotelWeb.Models;

namespace HotelWeb.Services;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllEmployeesAsync();
    Task<List<Employee>> GetAllEmployeesWithTasksAsync();
    Task<Employee?> GetEmployeeAsync(int id);
    Task<Employee?> GetEmployeeByEmailAsync(string email);
    Task<List<Employee>> GetActiveEmployeesAsync();
    Task CreateEmployeeAsync(Employee employee);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(int id);
}
