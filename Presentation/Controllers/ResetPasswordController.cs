using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using DAL.ViewModels;
using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using BLL.Interfaces;
using DAL.DBContext;

namespace Presentaion.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly PizzaShopContext _context;
        private readonly IJwtService _jwtService;
        private readonly IResetPasswordService _resetPasswordService;

        public ResetPasswordController(PizzaShopContext context, IJwtService jwtService, IResetPasswordService resetPasswordService)
        {
            _context = context;
            _jwtService = jwtService;
            _resetPasswordService = resetPasswordService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("resetpassword")]
        public IActionResult ResetPassword(string token)
        {
            var id = int.Parse(token.Split("?id=")[1]);
            token = token.Split("?id=")[0];
            var storedToken = HttpContext.Session.GetString("ResetToken");
            var storedExpiry = HttpContext.Session.GetString("ResetTokenExpiry");

            if (storedToken == token && storedExpiry != null && DateTime.Parse(storedExpiry) > DateTime.UtcNow)
            {
                return View(new ResetPasswordViewModel { Token = token, UserId = id });
            }
            else
            {
                return View("TokenExpired");
            }
        }

        [HttpPost]
        [Route("/api/resetpassword")]
        public JsonResult ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            try
            {
                var userId = model.UserId;
                var user = _resetPasswordService.GetUserDataById(userId);
                if (user != null)
                {
                    var storedToken = HttpContext.Session.GetString("ResetToken");
                    var storedExpiry = HttpContext.Session.GetString("ResetTokenExpiry");

                    if (storedToken == model.Token && storedExpiry != null && DateTime.Parse(storedExpiry) > DateTime.UtcNow)
                    {

                        HttpContext.Session.Remove("ResetToken");
                        HttpContext.Session.Remove("ResetTokenExpiry");
                        // reset password
                        return _resetPasswordService.ResetPassword(userId, model.NewPassword);
                    } else
                    {
                        return new JsonResult(new { success = false, message = "Invalid or expired token" });
                    }
                }
                else
                {
                    return new JsonResult(new { success = false, message = "User not found" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public IActionResult NewPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("/ResetPassword/NewPassword")]
        public async Task<IActionResult> NewPassword([FromBody] ResetPasswordFirstTimeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new JsonResult(new { success = false, message = "Validation errors" });
                }
                var userId = _jwtService.GetUserIdFromJwtToken(Request.Cookies["token"]);
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                    user.HasLoggedInBefore = true;
                    await _context.SaveChangesAsync();
                    return new JsonResult(new { success = true, message = "Password reset successfully" });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "User not found" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

    }
}