using HotelWeb.Auth;
using HotelWeb.Data;
using HotelWeb.Models;
using HotelWeb.Repositories;
using Microsoft.AspNetCore.Identity;

namespace HotelWeb.Services;

public class CustomerService(
    ICustomerRepository repo,
    UserManager<ApplicationUser> userManager) : ICustomerService
{
    public Task<List<Customer>> GetAllCustomersAsync()
        => repo.GetAllAsync();

    public Task<Customer?> GetCustomerAsync(int id)
        => repo.GetByIdAsync(id);

    public async Task<Customer?> GetByApplicationUserIdAsync(string applicationUserId)
        => await repo.GetByApplicationUserIdAsync(applicationUserId);

    public async Task CreateCustomerAsync(Customer customer, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new Exception("Password is required.");
        }
      
        var user = new ApplicationUser
        {
            UserName = customer.Email,
            Email = customer.Email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        await userManager.AddToRoleAsync(user, AppRoles.Customer);

        customer.ApplicationUserId = user.Id;

        await repo.AddAsync(customer);
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        var existingCustomer = await repo.GetByIdAsync(customer.Id);
        if (existingCustomer == null)
        {
            throw new Exception("Customer not found.");
        }

        var user = await userManager.FindByIdAsync(existingCustomer.ApplicationUserId);
        if (user == null)
        {
            throw new Exception("Customer user account not found.");
        }

        var emailChanged = !string.Equals(user.Email, customer.Email, StringComparison.OrdinalIgnoreCase);

        if (emailChanged)
        {
            user.Email = customer.Email;
            user.UserName = customer.Email;

            var emailResult = await userManager.UpdateAsync(user);

            if (!emailResult.Succeeded)
            {
                throw new Exception(string.Join(", ", emailResult.Errors.Select(e => e.Description)));
            }
        }
        await repo.UpdateAsync(existingCustomer);
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await repo.GetByIdAsync(id);
        if (customer == null)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(customer.ApplicationUserId))
        {
            var user = await userManager.FindByIdAsync(customer.ApplicationUserId);

            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }

        await repo.DeleteAsync(id);
    }
}