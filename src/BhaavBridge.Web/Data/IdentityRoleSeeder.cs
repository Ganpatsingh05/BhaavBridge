
using Microsoft.AspNetCore.Identity;

namespace BhaavBridge.Web.Data;

public static class IdentityRoleSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>(); /// It Manages the application role

        string[] roles =
        {
            "Farmer",
            "Buyer",
            "Administrator"
        };

        foreach (var role in roles)  ///Processes all three roles.
        {
            bool roleExists = await roleManager.RoleExistsAsync(role); ///// checks whether a role already exists. 

            if (!roleExists)
            {
                var result = await roleManager.CreateAsync( /// Creates a role if it doesn't exist.
                    new IdentityRole(role));

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        result.Errors.Select(error => error.Description));
                    throw new InvalidOperationException(
                        $"Failed to create role '{role}': {errors}");
                }
            }
        }
    }
}
