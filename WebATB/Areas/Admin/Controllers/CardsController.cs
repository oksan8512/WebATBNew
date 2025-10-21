using Microsoft.AspNetCore.Mvc;

namespace WebATB.Areas.Admin.Controllers;

[Area("Admin")]
public class CardsController : Controller
{
    public IActionResult Basic() => View();
}