using DAL.Models;
using BLL.Interfaces;

namespace BLL.Services
{
    public class NavBarService : INavBarService
    {
        private readonly PizzashopContext _context;
        public NavBarService(PizzashopContext context)
        {
            _context = context;
        }
                public string GetUsernameFromUserId(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                return user.Username;
            }
            return "";
        }

        public string GetProfileImageUrlFromUserId(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                return user.ProfileImage ?? "";
            }
            return "";
        }

        public int GetRoleIdFromUserId(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                return user.RoleId;
            }
            return 0;
        }

        public bool IsFirstTimeLogin(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user.HasLoggedInBefore == true)
            {
                return true;
            }
            return false;
        }
    }
}

