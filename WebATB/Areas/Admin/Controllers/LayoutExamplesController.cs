using Microsoft.AspNetCore.Mvc;

namespace WebATB.Areas.Admin.Controllers;

[Area("Admin")]
public class LayoutExamplesController : Controller
{
    public IActionResult Blank() => View();
    public IActionResult Container() => View();
    public IActionResult Fluid() => View();
    public IActionResult HorizontalMenu() => View();
    public IActionResult WithoutMenu() => View();
    public IActionResult WithoutNavbar() => View();
}