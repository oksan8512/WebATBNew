using Microsoft.AspNetCore.Mvc;

namespace WebATB.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardsController : Controller
{
    public IActionResult Index() => View();
}