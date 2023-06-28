using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWebsiteFront.Models;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace NewsWebsiteFront.Controllers
{
    public class AuthorDashboardController : Controller
    {
       
        private readonly MyHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public AuthorDashboardController(MyHttpClientFactory httpClientFactory )
        {
            _httpClientFactory = httpClientFactory;
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: AuthorDashboardController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        [HttpGet]
        public ActionResult CreateAuthor()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAuthor(Authors model)
        {
            // Get the authentication token from the authentication cookie
            var token = Request.Cookies["auth_token"];

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        // Use the MyHttpClientFactory to get the authenticated HttpClient instance
                        var httpClient = _httpClientFactory.GetHttpClient(token);
                        var postjob = httpClient.PostAsJsonAsync<Authors>("https://localhost:7102/api/Author", model);
                        Console.WriteLine(postjob);
                        postjob.Wait();
                        var postRes = postjob.Result;

                        if (postRes.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Succuss");
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {

                            Console.WriteLine("Fail");
                            var errorMessage = await postRes.Content.ReadAsStringAsync();
                            Console.WriteLine(errorMessage);

                            return View();
                        }
                    }

                    Console.WriteLine("Fail2");
                    return View();
                }
                catch(Exception ex)
                {
                    // Console.WriteLine("Fail3");
                   // throw ex;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }



        // GET: AuthorDashboardController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var token = Request.Cookies["auth_token"];
            Authors author = null;

            var httpClient = _httpClientFactory.GetHttpClient(token);

            using (var response = await httpClient.GetAsync("https://localhost:7102/api/Author/"+id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    author = JsonConvert.DeserializeObject<Authors>(apiResponse);
                }          

            return View(author);
        }

        // POST: AuthorDashboardController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Authors model)
        {
            var token = Request.Cookies["auth_token"];
            try
            {
                if (ModelState.IsValid)
                {
                    var httpClient = _httpClientFactory.GetHttpClient(token);
                        var postjob = httpClient.PutAsJsonAsync<Authors>("https://localhost:7102/api/Author/EditAuthor", model);
                        Console.WriteLine(postjob);
                        postjob.Wait();
                        var postRes = postjob.Result;

                        if (postRes.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {

                            var errorMessage = await postRes.Content.ReadAsStringAsync();
                            Console.WriteLine(errorMessage);

                            return View();
                        }
                    }
                
                return View();
            }
            catch
            {

                return View();
            }
        }

        // GET: AuthorDashboardController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var token = Request.Cookies["auth_token"];
            try
            {
                var httpClient = _httpClientFactory.GetHttpClient(token);
                var response = await httpClient.DeleteAsync($"https://localhost:7102/api/Author/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAuthors", "Dashboard");
                }

                return RedirectToAction("GetAuthors", "Dashboard");
            }
            catch
            {
                return View();
            }
        }

        // POST: AuthorDashboardController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id , IFormCollection collection)
        {
           
            return View();
           
        }
    }
}
