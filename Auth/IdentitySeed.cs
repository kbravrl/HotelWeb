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
}