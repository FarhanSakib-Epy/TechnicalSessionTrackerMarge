using EPYTST.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace EPYTST.API.Contollers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;


        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _baseApiUrl = configuration["ApiSettings:LoginUrl"];
            ViewBag.logInUrl = _baseApiUrl;
        }

        
        public IActionResult Index()
        {
            TempData["User"] = "null";
            return View();
        }
        public IActionResult MyProfile()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(LogIn logIn)
        {
            //string url = "https://localhost:44302/api/LoginUser/LogIn";
            var url = $"{_baseApiUrl}api/LoginUser/LogIn?username={logIn.UserName}&password={logIn.Password}";


            var loginRequest = new LogIn
            {
                UserName = logIn.UserName,
                Password = logIn.Password
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(loginRequest),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(url, jsonContent);

            var content = await response.Content.ReadAsStringAsync();

            var user = JsonSerializer.Deserialize<LoginUser>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (user.UserCode != 0)
            {
                TempData["User"] = JsonSerializer.Serialize(user);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View(logIn);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
