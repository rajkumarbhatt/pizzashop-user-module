using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using DAL.ViewModels;
using Microsoft.CodeAnalysis.Differencing;

namespace Presentaion.Controllers
{
    [Authorize (Roles = "Super Admin")]
    public class RoleAndPermission : Controller
    {
        private readonly IJwtService _jwtService;
        private readonly INavBarService _navBarService;
        private readonly IUserListService _userListService;
        private readonly IRoleAndPermissionService _roleAndPermissionService;

        public RoleAndPermission(IJwtService jwtService, INavBarService navBarService, IUserListService userListService, IRoleAndPermissionService roleAndPermissionService)
        {
            _jwtService = jwtService;
            _navBarService = navBarService;
            _userListService = userListService;
            _roleAndPermissionService = roleAndPermissionService;
        }
        public IActionResult Index()
        {
            var userId = _jwtService.GetUserIdFromJwtToken(Request.Cookies["token"]);
            var username = _navBarService.GetUsernameFromUserId(userId);
            var profileImageURL = _navBarService.GetProfileImageUrlFromUserId(userId);
            var roleId = _navBarService.GetRoleIdFromUserId(userId);
            var roles = _roleAndPermissionService.GetRoles();
            RoleAndPermissionViewModel roleAndPermissionViewModel = new RoleAndPermissionViewModel
            {
                Username = username,
                ProfileImageURL = profileImageURL,
                RoleId = roleId,
                Roles = roles
            };
            return View(roleAndPermissionViewModel);
        }


        [HttpGet]
        [Route("/RoleAndPermission/EditPermissions/{roleIdRequested}")]
        public IActionResult EditPermissions(int roleIdRequested)
        {
            var userId = _jwtService.GetUserIdFromJwtToken(Request.Cookies["token"]);
            var username = _navBarService.GetUsernameFromUserId(userId);
            var profileImageURL = _navBarService.GetProfileImageUrlFromUserId(userId);
            var roleId = _navBarService.GetRoleIdFromUserId(userId);
            var permissions = _roleAndPermissionService.GetPermissions();
            var roleRequested = _roleAndPermissionService.GetRole(roleIdRequested);
            var rolePermissions = _roleAndPermissionService.GetRolePermissions(roleIdRequested);
            EditPermissionsViewModel editPermissionsViewModel = new EditPermissionsViewModel
            {
                Username = username,
                ProfileImageURL = profileImageURL,
                Permissions = permissions,
                RolePermissions = rolePermissions,
                RoleId = roleId,
                RequestedRole = roleRequested
            };
            return View(editPermissionsViewModel);
        }

        [HttpPost]
        public IActionResult UpdatePermissions ([FromBody] List<PermissionChangeModel> changedPermissions) {
            return _roleAndPermissionService.UpdateRolePermissions(changedPermissions);
        }
    }
}