using HotelWeb.Models;
using HotelWeb.Repositories;

namespace HotelWeb.Services;

public class CustomerService(ICustomerRepository repo) : ICustomerService
{
    public Task<List<Customer>> GetAllCustomersAsync()
        => repo.GetAllAsync();

    public Task<List<Customer>> GetAllCustomersWithReservationsAsync()
        => repo.GetAllWithReservationsAsync();

    public Task<Customer?> GetCustomerAsync(int id)
        => repo.GetByIdAsync(id);

    public Task<Customer?> GetCustomerByEmailAsync(string email)
        => repo.GetByEmailAsync(email);

    public async Task<Customer?> GetCustomerByApplicationUserIdAsync(string applicationUserId)
    {
        var customers = await repo.GetAllAsync();
        return customers.FirstOrDefault(c => c.ApplicationUserId == applicationUserId);
    }

    public Task CreateCustomerAsync(Customer customer)
        => repo.AddAsync(customer);

    public Task UpdateCustomerAsync(Customer customer)
        => repo.UpdateAsync(customer);

    public Task DeleteCustomerAsync(int id)
        => repo.DeleteAsync(id);
}
