
using BhaavBridge.Web.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace BhaavBridge.Web.Data;

public static class AdminBootstrapSeeder
{
    public static async Task SeedAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        var userManager =
            services.GetRequiredService<UserManager<ApplicationUser>>();

        var email = configuration["AdminBootstrap:Email"];
        var password = configuration["AdminBootstrap:Password"];

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Administrator email or password is missing from User Secrets.");
        }

        email = email.Trim();

        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                FullName = "BhaavBridge Administrator",
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult =
                await userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    createResult.Errors.Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Failed to create administrator: {errors}");
            }
		 } 
        var existingRoles = await userManager.GetRolesAsync(user);
        var hasNonAdminRole = existingRoles.Any(
            role => !string.Equals(role, "Administrator", StringComparison.OrdinalIgnoreCase));

        if (hasNonAdminRole)
        {
            throw new InvalidOperationException(
                "The configured Development administrator email already belongs to a non-administrator role. Use a dedicated administrator account.");
        }

        if (!await userManager.IsInRoleAsync(user, "Administrator"))
        {
            var roleResult =
                await userManager.AddToRoleAsync(user, "Administrator");

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    roleResult.Errors.Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Failed to assign Administrator role: {errors}");
            }
        }
    }
}
