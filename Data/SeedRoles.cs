using Microsoft.AspNetCore.Identity;
using OnlineBookStore.Models;

namespace OnlineBookStore.Data
{
    public static class SeedRoles
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Seeding started...");

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Customer" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    Console.WriteLine($"Role '{roleName}' created.");
                }
            }

            var adminEmail = "admin@bookstore.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                Console.WriteLine("Admin user does not exist, creating now...");
                var user = new ApplicationUser { UserName = adminEmail, Email = adminEmail };

                var createUser = await userManager.CreateAsync(user, "Admin@12345"); // Ensure a strong password

                if (createUser.Succeeded)
                {
                    Console.WriteLine("Admin user created successfully.");
                    await userManager.AddToRoleAsync(user, "Admin");
                    Console.WriteLine("Admin user assigned to 'Admin' role.");
                }
                else
                {
                    Console.WriteLine("Failed to create admin user:");
                    foreach (var error in createUser.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Admin user already exists.");
            }

            Console.WriteLine("Seeding completed.");
        }
    }
}
