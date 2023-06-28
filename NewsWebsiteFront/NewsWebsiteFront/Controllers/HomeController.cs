using Microsoft.AspNetCore.Mvc;
using NewsWebsiteFront.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace NewsWebsiteFront.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7102");
        }
        public async Task<IActionResult> Index(string status)
        {
           
            var response = await _httpClient.GetAsync("/api/News");
            if (!response.IsSuccessStatusCode)
            {
                // Handle error here
                return View("Error");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                // Handle empty response here
                return View("Empty");
            }

            try
            {
                var values = JsonConvert.DeserializeObject<IEnumerable<NewsDTO>>(content);
                if (values.Count() > 0)
                {

                    switch (status)
                    {
                        case "N":
                            values = values.OrderByDescending(s => s.Title);
                            break;
                        case "D":
                            values = values.OrderByDescending(s => s.creationDate);
                            break;
                        default:
                            values = values.OrderBy(s => s.Title);
                            break;

                    }

                    return View(values);

                }
                else
                {
                    // Handle empty values here
                    return View("Empty");
                }
            }
            catch (System.Text.Json.JsonException ex)
            {
                // Handle invalid JSON here
                return View("Error");
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync("/api/News/" + id);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(content))
            {
                var values = JsonConvert.DeserializeObject<NewsDTO>(content);

                var response2 = await _httpClient.GetAsync("/api/Author");
                var authorsContent = await response2.Content.ReadAsStringAsync();

                if (response2.IsSuccessStatusCode && !string.IsNullOrEmpty(authorsContent))
                {
                    ViewData["Authors"] = JsonConvert.DeserializeObject<IEnumerable<Authors>>(authorsContent);
                }

                return View(values);
            }
            else
            {
                // Handle unsuccessful response here
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}