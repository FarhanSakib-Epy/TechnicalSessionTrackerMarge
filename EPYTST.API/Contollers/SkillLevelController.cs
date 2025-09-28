using EPYTST.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPYTST.API.Contollers
{
    [Route("SkillLevel")]
    public class SkillLevelController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;
        private readonly ISkillLevelService _SkillLevelService;

        public SkillLevelController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, ISkillLevelService SkillLevelService, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _baseApiUrl = configuration["ApiSettings:LoginUrl"];
            this._SkillLevelService = SkillLevelService;
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
        public async Task<IActionResult> Details(int id)
        {
            var skillLevel = await _SkillLevelService.GetByIdAsync(id.ToString());

            return View(skillLevel);
        }
    }
}
