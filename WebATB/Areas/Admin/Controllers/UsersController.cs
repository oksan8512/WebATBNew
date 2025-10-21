using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebATB.Areas.Admin.Models.Users;
using WebATB.Data;

namespace WebATB.Areas.Admin.Controllers;

[Area("Admin")]
public class UsersController(AppATBDbContext dbContext, IMapper mapper) : Controller
{
    public async Task<IActionResult> Index()
    {
        var users = await dbContext.Users
            .ProjectTo<UserItemVM>(mapper.ConfigurationProvider)
            .ToListAsync();

        return View(users);
    }
}