using System.ComponentModel.DataAnnotations;

namespace DAL.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Current Password is required")]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }
        public string? Username { get; set; }
        public string? ProfileImageURL { get; set; }
        public int? RoleId { get; set; }
    }
}
