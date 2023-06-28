using Microsoft.AspNetCore.Mvc;
using NewsWebsiteFront.Models;
using System.Net.Http.Headers;

namespace NewsWebsiteFront.Controllers
{
    public class MyHttpClientFactory
    {
        private static HttpClient _httpClient;

        public HttpClient GetHttpClient(string token)
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
            }

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return _httpClient;
        }
    }
    public class AccountController : Controller
    {
      
        private readonly MyHttpClientFactory _httpClientFactory;

        public AccountController(MyHttpClientFactory httpClientFactory)
        {
          
            _httpClientFactory = httpClientFactory;

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token");

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<ActionResult> Login(loginModel login)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7102/api/Auth/login");
                var postjob = client.PostAsJsonAsync<loginModel>("", login);
                postjob.Wait();
                var postRes = postjob.Result;

                if (postRes.IsSuccessStatusCode)
                {
                    var result = await postRes.Content.ReadFromJsonAsync<AuthModel>();
                    var Mytoken = result.Token;

                    // Add the authentication token to the HTTP headers of subsequent requests
                    var httpClient = _httpClientFactory.GetHttpClient(Mytoken);

                    // Set an authentication cookie with the authentication token
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                    };

                    Response.Cookies.Append("auth_token", Mytoken, cookieOptions);

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {                  
                    return View();
                }
            }
        }
    }
}
