using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebATB.Data.Entities.Idenity;
using WebATB.Interfaces;
using WebATB.Models.Account;

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
                //if (imageStr is not null)
                //{
                //    await userManager.AddClaimAsync(user, new Claim("avatar", $"/avatars/50_{imageStr}"));
                //}

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
}