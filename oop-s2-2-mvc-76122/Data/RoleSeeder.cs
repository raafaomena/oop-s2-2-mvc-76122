using Microsoft.AspNetCore.Identity;

namespace oop_s2_2_mvc_76122.Data;

public static class RoleSeeder
{
    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roles = { "Admin", "Inspector", "Viewer" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        await CreateUser(userManager, "admin@test.com", "Admin123!", "Admin");
        await CreateUser(userManager, "inspector@test.com", "Inspector123!", "Inspector");
        await CreateUser(userManager, "viewer@test.com", "Viewer123!", "Viewer");
    }

    private static async Task CreateUser(UserManager<IdentityUser> userManager,
        string email, string password, string role)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new IdentityUser { UserName = email, Email = email };

            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, role);
        }
    }
}