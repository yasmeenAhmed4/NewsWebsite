
using NewsWebsite.DAL.Data.Models;

namespace CubeGame.DAL.Repo.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(LoginModel model);

    }
}
