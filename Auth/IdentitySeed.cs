using HotelWeb.Data;
using HotelWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelWeb.Auth;

public static class IdentitySeed
{
    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new[] { AppRoles.Admin, AppRoles.Customer, AppRoles.Receptionist, AppRoles.Housekeeping };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    public static async Task SeedTestUsersAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await CreateAdminUserAsync(userManager);
        await CreateTestCustomerAsync(userManager, dbContext);
    }

    private static async Task CreateAdminUserAsync(UserManager<ApplicationUser> userManager)
    {
        var email = "admin@hotelweb.com";
        var user = await userManager.FindByEmailAsync(email);

        if (user is not null)
        {
            Console.WriteLine($"Admin user already exists: {email}");
            return;
        }

        Console.WriteLine($"Creating new admin user: {email}");
        user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, "Test123!");
        if (!result.Succeeded)
            throw new Exception("Admin user create failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(user, AppRoles.Admin);
        Console.WriteLine("Admin user created successfully!");
    }

    private static async Task CreateTestCustomerAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
    {
        var email = "customer@hotelweb.com";
        var user = await userManager.FindByEmailAsync(email);

        if (user is not null)
        {
            Console.WriteLine($"Customer user already exists: {email}");

            var existingCustomer = dbContext.Customers.FirstOrDefault(c => c.ApplicationUserId == user.Id);
            if (existingCustomer is null)
            {
                Console.WriteLine("Customer record not found, creating...");
                var customer = new Customer
                {
                    ApplicationUserId = user.Id,
                    FirstName = "Ali",
                    LastName = "Yılmaz",
                    Email = email,
                    Phone = "+90 555 111 2233",
                    DateOfBirth = new DateOnly(1990, 5, 15),
                    IdentityNumber = "12345678901"
                };
                dbContext.Customers.Add(customer);
                await dbContext.SaveChangesAsync();
                Console.WriteLine("Customer record created successfully!");
            }
            return;
        }

        Console.WriteLine($"Creating new customer user: {email}");
        user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, "Test123!");
        if (!result.Succeeded)
            throw new Exception("Customer user create failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(user, AppRoles.Customer);

        var newCustomer = new Customer
        {
            ApplicationUserId = user.Id,
            FirstName = "Ali",
            LastName = "Yılmaz",
            Email = email,
            Phone = "+90 555 111 2233",
            DateOfBirth = new DateOnly(1990, 5, 15),
            IdentityNumber = "12345678901"
        };

        dbContext.Customers.Add(newCustomer);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("Customer user and record created successfully!");
    }
}