using Microsoft.AspNetCore.Mvc;

namespace WebATB.Areas.Admin.Controllers;

[Area("Admin")]
public class FormsController : Controller
{
    public IActionResult BasicInputs() => View();
    public IActionResult InputGroups() => View();
}