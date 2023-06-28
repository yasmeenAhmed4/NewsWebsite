using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWebsiteFront.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;

namespace NewsWebsiteFront.Controllers
{
    public class DashboardController : Controller
    {
        private readonly MyHttpClientFactory _httpClientFactory;
        //private readonly HttpClient _httpClient;
        public DashboardController(MyHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            //_httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("https://localhost:7102");
        }

        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["auth_token"];

            if (await GetAuthorsAsync() == null)
            {
                ViewData["Authors"] = null;
            }
            else
            {
                ViewData["Authors"] = await GetAuthorsAsync();
            }
            var httpClient = _httpClientFactory.GetHttpClient(token);
            var response = await httpClient.GetAsync("https://localhost:7102/api/News");

            if (!response.IsSuccessStatusCode)
            {
                // Handle error here
                return View("Error");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                // Handle empty response here
                return View("Empty" , "Dashboard");
            }

            try
            {
                var values = JsonConvert.DeserializeObject<IEnumerable<NewsDTO>>(content);
                if (values.Count() > 0)
                {
                    return View(values);
                }
                else
                {
                    // Handle empty values here
                    return View("Empty" , "Dashboard");
                }
            }
            catch (System.Text.Json.JsonException ex)
            {
                // Handle invalid JSON here
                return View("Error");
            }

        }

        // GET: DashController/Details/5
        public async Task<IActionResult> Details(int id)
        {      
            return View();     
        }


        [HttpGet]
        // GET: DashController/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Authors"] = await GetAuthorsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,newsDescription,imageURL,publicationDate,AuthorId")] NewsDTO newsmodel)
        {
            var token = Request.Cookies["auth_token"];
            try
            {
                if (ModelState.IsValid)
                {
                    // Save the image file to the server
                    if (newsmodel.imageURL != null)
                    {
                        var fileName = Path.GetFileName(newsmodel.imageURL.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            newsmodel.imageURL.CopyTo(fileStream);
                        }

                        // Set the image path in the NewsDTO object
                        var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
                        newsmodel.image = baseUrl + "/images/" + fileName;
                        //newsmodel.image = filePath;
                    }

                    var httpClient = _httpClientFactory.GetHttpClient(token);
                    var postjob = httpClient.PostAsJsonAsync<NewsDTO>("https://localhost:7102/api/News/AddNews", newsmodel);
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

                            ViewData["Authors"] = await GetAuthorsAsync();

                            return View();
                        }
                    }
                
                ViewData["Authors"] = await GetAuthorsAsync();

                return View();
            }
            catch
            {
                ViewData["Authors"] = await GetAuthorsAsync();

                return View();
            }
        }

        private async Task<IEnumerable<Authors>> GetAuthorsAsync()
        {
            var token = Request.Cookies["auth_token"];
            var httpClient = _httpClientFactory.GetHttpClient(token);

            var response = await httpClient.GetAsync("https://localhost:7102/api/Author");

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            var values = JsonConvert.DeserializeObject<IEnumerable<Authors>>(content);
            if (values.Count() > 0)
            {
                return values;
            }
            else
            {
                // Handle empty values here
                return null;
            }
           
        }

        public async Task<IActionResult> GetAuthors()
        {
            var token = Request.Cookies["auth_token"];
            var httpClient = _httpClientFactory.GetHttpClient(token);
            var response = await httpClient.GetAsync("https://localhost:7102/api/Author");
            
            if (!response.IsSuccessStatusCode)
            {
                // Handle error here
                return View("Error");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                // Handle empty response here
                return View("~/Views/AuthorDashboard/Empty.cshtml");
            }

            try
            {
                var values = JsonConvert.DeserializeObject<IEnumerable<Authors>>(content);
                if (values.Count() > 0)
                {
                    return View(values);
                }
                else
                {
                    // Handle empty values here
                    return View("~/Views/AuthorDashboard/Empty.cshtml");
                }
            }
            catch (System.Text.Json.JsonException ex)
            {
                // Handle invalid JSON here
                return View("Error");
            }

        }

        // GET: DashboardController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var token = Request.Cookies["auth_token"];
            ViewData["Authors"] = await GetAuthorsAsync();
            NewsDTO News = null;
            var httpClient = _httpClientFactory.GetHttpClient(token);
          
                using (var response = await httpClient.GetAsync("https://localhost:7102/api/News/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    News = JsonConvert.DeserializeObject<NewsDTO>(apiResponse);
                }
            

            return View(News);
        }

        // POST: DashController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsDTO newsmodel)
        {
            var token = Request.Cookies["auth_token"];
            try
            {
                if (ModelState.IsValid)
                {
                    // Save the image file to the server
                    if (newsmodel.imageURL != null)
                    {
                        var fileName = Path.GetFileName(newsmodel.imageURL.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            newsmodel.imageURL.CopyTo(fileStream);
                        }

                        // Set the image path in the NewsDTO object
                        var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
                        newsmodel.image = baseUrl + "/images/" + fileName;
                        //newsmodel.image = filePath;
                    }

                    var httpClient = _httpClientFactory.GetHttpClient(token);
                    var postjob = httpClient.PutAsJsonAsync<NewsDTO>("https://localhost:7102/api/News/EditNews", newsmodel);
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

                            ViewData["Authors"] = await GetAuthorsAsync();

                            return View();
                        }
                    
                }
                ViewData["Authors"] = await GetAuthorsAsync();
                return View();
            }
            catch
            {
                ViewData["Authors"] = await GetAuthorsAsync();

                return View();
            }
        }

        // GET: DashController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var token = Request.Cookies["auth_token"];
            try
            {
                var httpClient = _httpClientFactory.GetHttpClient(token);
                var response = await httpClient.DeleteAsync($"https://localhost:7102/api/News/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Dashboard");
                }

                return RedirectToAction("Index", "Dashboard");
            }
            catch
            {
                return View();
            }
        }

        // POST: DashController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
