using HotelWeb.Models;
using HotelWeb.Repositories;

namespace HotelWeb.Services;

public class EmployeeService(IEmployeeRepository repo) : IEmployeeService
{
    public Task<List<Employee>> GetAllEmployeesAsync()
        => repo.GetAllAsync();

    public Task<List<Employee>> GetAllEmployeesWithTasksAsync()
        => repo.GetAllWithTasksAsync();

    public Task<Employee?> GetEmployeeAsync(int id)
        => repo.GetByIdAsync(id);

    public Task<Employee?> GetEmployeeByEmailAsync(string email)
        => repo.GetByEmailAsync(email);

    public Task<List<Employee>> GetActiveEmployeesAsync()
        => repo.GetActiveEmployeesAsync();

    public Task CreateEmployeeAsync(Employee employee)
        => repo.AddAsync(employee);

    public Task UpdateEmployeeAsync(Employee employee)
        => repo.UpdateAsync(employee);

    public Task DeleteEmployeeAsync(int id)
        => repo.DeleteAsync(id);
}
