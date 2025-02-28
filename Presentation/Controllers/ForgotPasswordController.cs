using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using DAL.ViewModels;
using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using DAL.DBContext;


namespace Presentaion.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly PizzaShopContext _context;

        public ForgotPasswordController(IEmailService emailService, PizzaShopContext context)
        {
            _emailService = emailService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

    [HttpPost]
    [Route("api/forgotpassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel forgotPasswordModel)
        {
            try
            {
                var email = forgotPasswordModel.Email;
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user != null)
                {
                    // Generate a unique token
                    var token = Guid.NewGuid().ToString();
                    var expiryDate = DateTime.UtcNow.AddDays(1); // Token valid for 1 day
                    HttpContext.Session.SetString("ResetToken", token);
                    HttpContext.Session.SetString("ResetTokenExpiry", expiryDate.ToString());

                    var resetLink = $"http://localhost:5125/resetpassword?token={token}?id={user.Id}";
                    await _emailService.SendEmailAsync(user.Email, user, resetLink);

                    return new JsonResult(new { success = true, message = "Email Sent" });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "Email not registered" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
}
    }
}