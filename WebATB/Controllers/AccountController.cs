using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebATB.Data.Entities.Idenity;
using WebATB.Interfaces;
using WebATB.Models.Account;
using WebATB.Services;

namespace WebATB.Controllers;

public class AccountController(UserManager<UserEntity> userManager,
    SignInManager<UserEntity> signInManager,
    IImageService imageService,
    IMapper mapper) : Controller
{
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Main");
    }


    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var imageStr = model.Image is not null ? await imageService.SaveImageAsync(model.Image, "AvatarDir") : null;
            var user = mapper.Map<UserEntity>(model);
            user.Image = imageStr;
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Main");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        var model = new LoginViewModel { ReturnUrl = returnUrl };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Перевіряємо чи існує користувач з таким email
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Користувача з таким email не знайдено");
                return View(model);
            }

            // Спроба входу
            var result = await signInManager.PasswordSignInAsync(
                user.UserName!,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                // Якщо є returnUrl і він локальний - перенаправляємо туди
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Main");
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Обліковий запис заблоковано");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Вхід не дозволено");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Невірний пароль");
            }
        }

        return View(model);
    }

    // ================= РЕДАГУВАННЯ ПРОФІЛЮ =================
    [HttpGet]
    public async Task<IActionResult> EditProfileView()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            TempData["ErrorMessage"] = "Користувача не знайдено";
            return RedirectToAction("Login");
        }

        var model = new EditProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            CurrentImage = user.Image
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfileView(EditProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            TempData["ErrorMessage"] = "Користувача не знайдено";
            return RedirectToAction("Login");
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;

        if (model.Image != null && model.Image.Length > 0)
        {
            try
            {
                var imageResult = await imageService.SaveImageAsync(model.Image, "AvatarDir");
                if (!string.IsNullOrEmpty(imageResult))
                {
                    user.Image = imageResult;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Помилка завантаження фото: {ex.Message}";
                return View(model);
            }
        }

        var result = await userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Профіль успішно оновлено";
            return RedirectToAction("EditProfileView");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }


}