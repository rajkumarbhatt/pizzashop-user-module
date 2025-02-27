using DAL.Models;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BLL.Services
{
    public class LoginService : ILoginService
    {
        private readonly PizzashopContext _context;
        private readonly IJwtService _jwtService;

        public LoginService(PizzashopContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public IActionResult Validate(string email, string password)
        {
            
            User? user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                string? role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId).Name;
                string token = _jwtService.GenerateJwtToken(user, role);
                return new JsonResult(new { token = token, success = true, message = "Login successful" });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Invalid email or password" });
            }
        }
    }
}