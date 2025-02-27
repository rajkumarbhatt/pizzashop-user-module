using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DAL.ViewModels
{
    public class CreateUserViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string? Username { get; set; }
        public string? ProfileImageURL { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string UsernameRequestedUser { get; set; }
        public IFormFile? ProfileImage { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public int RoleIdRequestedUser { get; set; }
        
    }
}