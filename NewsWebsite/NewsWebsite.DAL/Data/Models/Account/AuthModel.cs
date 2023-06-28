using System.Text.Json.Serialization;

namespace NewsWebsite.DAL.Data.Models
{
    public class AuthModel
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
