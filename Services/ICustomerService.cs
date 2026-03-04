using HotelWeb.Models;

namespace HotelWeb.Services;

public interface ICustomerService
{
    Task<List<Customer>> GetAllCustomersAsync();
    Task<List<Customer>> GetAllCustomersWithReservationsAsync();
    Task<Customer?> GetCustomerAsync(int id);
    Task<Customer?> GetCustomerByEmailAsync(string email);
    Task<Customer?> GetCustomerByApplicationUserIdAsync(string applicationUserId);
    Task CreateCustomerAsync(Customer customer);
    Task UpdateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(int id);
}
