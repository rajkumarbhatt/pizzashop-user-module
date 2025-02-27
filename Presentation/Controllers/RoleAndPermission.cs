using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using DAL.ViewModels;

namespace Presentaion.Controllers
{
    [Authorize (Roles = "Super Admin")]
    public class RoleAndPermission : Controller
    {
        private readonly IJwtService _jwtService;
        private readonly INavBarService _navBarService;
        private readonly IUserListService _userListService;

        public RoleAndPermission(IJwtService jwtService, INavBarService navBarService, IUserListService userListService)
        {
            _jwtService = jwtService;
            _navBarService = navBarService;
            _userListService = userListService;
        }
        public IActionResult Index()
        {
            var userId = _jwtService.GetUserIdFromJwtToken(Request.Cookies["token"]);
            var username = _navBarService.GetUsernameFromUserId(userId);
            var profileImageURL = _navBarService.GetProfileImageUrlFromUserId(userId);
            var roleId = _navBarService.GetRoleIdFromUserId(userId);
            var roles = _userListService.GetRoles();
            RoleAndPermissionViewModel roleAndPermissionViewModel = new RoleAndPermissionViewModel
            {
                Username = username,
                ProfileImageURL = profileImageURL,
                RoleId = roleId,
                Roles = roles
            };
            return View(roleAndPermissionViewModel);
        }
    }
}