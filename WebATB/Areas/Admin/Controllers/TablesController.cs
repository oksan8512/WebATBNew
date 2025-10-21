using Microsoft.AspNetCore.Mvc;

namespace WebATB.Areas.Admin.Controllers;

[Area("Admin")]
public class TablesController : Controller
{
    public IActionResult Basic() => View();
}