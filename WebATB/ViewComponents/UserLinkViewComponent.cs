using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebATB.Data.Entities.Idenity;
using WebATB.Models.Account;

namespace WebATB.ViewComponents
{
    public class UserLinkViewComponent : ViewComponent
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;

        public UserLinkViewComponent(UserManager<UserEntity> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return Content(string.Empty);
            }

            var model = _mapper.Map<UserLinkViewModel>(user);
            return View(model);
        }
    }
}