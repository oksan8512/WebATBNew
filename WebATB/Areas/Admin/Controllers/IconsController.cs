using Microsoft.AspNetCore.Mvc;

namespace WebATB.Areas.Admin.Controllers;

[Area("Admin")]
public class IconsController : Controller
{
    public IActionResult RiIcons() => View();
}