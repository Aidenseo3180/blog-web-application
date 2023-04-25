using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Models.ViewModels
{
    public class RegisterViewModel
    {
        //validate these 3 properties 

        [Required]  //tell View (or user) that we want these properties to be filled
        [MaxLength(10, ErrorMessage = "Username has to be shorter than 10 characters")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]  //validates if this is an email address
        public string Email { get; set; }
  
        [Required]
        [MinLength(6, ErrorMessage = "Password has to be at least 6 characters")]  //min length must be 6
        public string Password { get; set; }
    }
}
