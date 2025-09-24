using Microsoft.AspNetCore.Mvc;

namespace EPYTST.API.Contollers
{
    [Route("Skill")]
    public class SkillController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;

        public SkillController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _baseApiUrl = configuration["ApiSettings:LoginUrl"];
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {

            return View();
        }

    }
}
