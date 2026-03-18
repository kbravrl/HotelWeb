using HotelWeb.Data;
using HotelWeb.Models;
using HotelWeb.Repositories;
using Microsoft.AspNetCore.Identity;

namespace HotelWeb.Services;

public class EmployeeService(IEmployeeRepository repo, UserManager<ApplicationUser> userManager) : IEmployeeService
{
    public Task<List<Employee>> GetAllEmployeesAsync()
        => repo.GetAllAsync();

    public Task<Employee?> GetEmployeeAsync(int id)
        => repo.GetByIdAsync(id);

    public Task<Employee?> GetByApplicationUserIdAsync(string applicationUserId)
        => repo.GetByApplicationUserIdAsync(applicationUserId);

    public Task<List<Employee>> GetCleanersAsync()
        => repo.GetCleanersAsync();

    public Task CreateEmployeeAsync(Employee employee)
        => repo.AddAsync(employee);

    public async Task CreateEmployeeWithUserAsync(Employee employee, string password, string role)
    {
        var user = new ApplicationUser
        {
            UserName = employee.Email,
            Email = employee.Email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        await userManager.AddToRoleAsync(user, role);
        employee.ApplicationUserId = user.Id;
        await repo.AddAsync(employee);
    }

    public Task UpdateEmployeeAsync(Employee employee)
        => repo.UpdateAsync(employee);

    public Task DeleteEmployeeAsync(int id)
        => repo.DeleteAsync(id);
}
