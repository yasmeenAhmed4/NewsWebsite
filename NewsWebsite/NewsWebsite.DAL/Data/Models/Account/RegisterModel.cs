using System.ComponentModel.DataAnnotations;

namespace NewsWebsite.DAL.Data.Models
{
    public class RegisterModel
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(128)]
        public string Email { get; set; }

        [Required, StringLength(256)]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }
        public string Role { get; set; } 
    }
}
