using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebATB.Areas.Admin.Models.Users;
using WebATB.Data;
using WebATB.Data.Entities.Idenity;

namespace WebATB.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class UsersController(
    AppATBDbContext dbContext,
    IMapper mapper,
    UserManager<UserEntity> userManager) : Controller
{
    // Список всіх користувачів
    public async Task<IActionResult> Index()
    {
        var users = await dbContext.Users
            .OrderByDescending(u => u.Id)
            .ProjectTo<UserItemVM>(mapper.ConfigurationProvider)
            .ToListAsync();

        // Додаємо ролі для кожного користувача
        foreach (var user in users)
        {
            var userEntity = await userManager.FindByIdAsync(user.Id.ToString());
            if (userEntity != null)
            {
                user.Roles = (await userManager.GetRolesAsync(userEntity)).ToList();
                // Перевіряємо чи заблокований користувач
                user.IsLocked = await userManager.IsLockedOutAsync(userEntity);
            }
        }

        return View(users);
    }

    // Призначити/зняти роль Admin
    [HttpPost]
    public async Task<IActionResult> ToggleRole(int userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound();

        var isInRole = await userManager.IsInRoleAsync(user, roleName);

        if (isInRole)
        {
            await userManager.RemoveFromRoleAsync(user, roleName);
            TempData["Success"] = $"Роль '{roleName}' видалено у користувача {user.FirstName} {user.LastName}";
        }
        else
        {
            await userManager.AddToRoleAsync(user, roleName);
            TempData["Success"] = $"Роль '{roleName}' додано користувачу {user.FirstName} {user.LastName}";
        }

        return RedirectToAction(nameof(Index));
    }

    // Заблокувати користувача
    [HttpPost]
    public async Task<IActionResult> LockUser(int userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound();

        // Блокуємо на 100 років (практично назавжди)
        var lockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
        var result = await userManager.SetLockoutEndDateAsync(user, lockoutEnd);

        if (result.Succeeded)
        {
            TempData["Success"] = $"Користувача {user.FirstName} {user.LastName} заблоковано";
        }
        else
        {
            TempData["Error"] = "Помилка при блокуванні користувача";
        }

        return RedirectToAction(nameof(Index));
    }

    // Розблокувати користувача
    [HttpPost]
    public async Task<IActionResult> UnlockUser(int userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound();

        var result = await userManager.SetLockoutEndDateAsync(user, null);

        if (result.Succeeded)
        {
            // Скидаємо лічильник невдалих спроб входу
            await userManager.ResetAccessFailedCountAsync(user);
            TempData["Success"] = $"Користувача {user.FirstName} {user.LastName} розблоковано";
        }
        else
        {
            TempData["Error"] = "Помилка при розблокуванні користувача";
        }

        return RedirectToAction(nameof(Index));
    }

    // Видалити користувача
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return NotFound();

        var result = await userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            TempData["Success"] = $"Користувача {user.FirstName} {user.LastName} видалено";
        }
        else
        {
            TempData["Error"] = "Помилка при видаленні користувача";
        }

        return RedirectToAction(nameof(Index));
    }
}