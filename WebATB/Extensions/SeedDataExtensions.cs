using Microsoft.AspNetCore.Identity;
using WebATB.Data.Entities.Idenity;

namespace WebATB.Extensions;

public static class SeedDataExtensions
{
    public static async Task SeedDataAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();

        // Створюємо ролі якщо їх немає
        string[] roles = { "Admin", "User", "Moderator" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new RoleEntity { Name = role });
                Console.WriteLine($"✅ Створено роль: {role}");
            }
        }

        // Створюємо адміна якщо його немає
        var adminEmail = "admin@atb.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new UserEntity
            {
                Email = adminEmail,
                UserName = adminEmail,
                FirstName = "Адмін",
                LastName = "Система",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine("✅ Створено адміна:");
                Console.WriteLine($"   Email: {adminEmail}");
                Console.WriteLine($"   Пароль: Admin123!");
            }
            else
            {
                Console.WriteLine("❌ Помилка створення адміна:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"   - {error.Description}");
                }
            }
        }
        else
        {
            Console.WriteLine($"ℹ️ Адмін вже існує: {adminEmail}");
        }
    }
}