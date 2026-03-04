using HotelWeb.Models;

namespace HotelWeb.Services;

public interface ICustomerService
{
    Task<List<Customer>> GetAllCustomersAsync();
    Task<Customer?> GetCustomerAsync(int id);
    Task<Customer?> GetCustomerByEmailAsync(string email);
    Task CreateCustomerAsync(Customer customer);
    Task UpdateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(int id);
}
