using EPYTST.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPYTST.API.Contollers
{
    [Route("Skill")]
    public class SkillController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;
        private readonly ISkillService _SkillService;

        public SkillController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, ISkillService SkillService, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _baseApiUrl = configuration["ApiSettings:LoginUrl"];
            this._SkillService = SkillService;
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

        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            var skill = _SkillService.GetByIdAsync(id.ToString());

            return View(skill.Result);
        }
        


    }
}
