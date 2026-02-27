using HotelWeb.Data;
using Microsoft.AspNetCore.Identity;

namespace HotelWeb.Auth;

public static class IdentitySeed
{
    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new[] { AppRoles.Customer, AppRoles.Employee };

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

        await CreateTestUserAsync(userManager, "customer@test.com", AppRoles.Customer);
        await CreateTestUserAsync(userManager, "employee@test.com", AppRoles.Employee);
    }

    private static async Task CreateTestUserAsync(UserManager<ApplicationUser> userManager, string email, string role)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is not null)
            return;

        user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, "Test123!");
        if (!result.Succeeded)
            throw new Exception($"{role} user create failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(user, role);
    }
}