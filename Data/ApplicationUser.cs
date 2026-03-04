using HotelWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelWeb.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public Customer? Customer { get; set; }
        public Employee? Employee { get; set; }
    }

}
