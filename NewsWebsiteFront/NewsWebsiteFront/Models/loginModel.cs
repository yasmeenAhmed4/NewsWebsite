using System.ComponentModel.DataAnnotations;

namespace NewsWebsiteFront.Models
{
    public class loginModel
    {
        [Required(ErrorMessage = "Email is required")]      
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]     
        public string Password { get; set; }
    }
}
