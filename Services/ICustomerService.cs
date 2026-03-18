using HotelWeb.Models;

namespace HotelWeb.Services;

public interface ICustomerService
{
    Task<List<Customer>> GetAllCustomersAsync();
    Task<Customer?> GetCustomerAsync(int id);
    Task<Customer?> GetByApplicationUserIdAsync(string applicationUserId);
    Task CreateCustomerAsync(Customer customer, string password);
    Task UpdateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(int id);
}