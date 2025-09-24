using EPYTST.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace EPYTST.API.Contollers
{
    [Route("UserInformation")]
    public class UserInformationController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;

        public UserInformationController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
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

        [HttpGet("AddSkill")]
        public IActionResult AddSkill()
        {

            return View();
        }


        [HttpPost("Submit")]
        public async Task<IActionResult> Submit([FromForm] SkillUploadViewModel model)
        {

            string savedFilePath = string.Empty;
            string? uniqueFileName = null;

            if (model.CertificateFile != null && model.CertificateFile.Length > 0)
            {
                // Extract original file name and extension
                var originalFileName = Path.GetFileNameWithoutExtension(model.CertificateFile.FileName);
                var fileExtension = Path.GetExtension(model.CertificateFile.FileName);

                // Sanitize the file name (optional but recommended)
                originalFileName = originalFileName.Replace(" ", "_");

                // Generate unique name: e.g., SkillCert_20250717_abc123xyz.pdf
                uniqueFileName = $"{originalFileName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}_{Guid.NewGuid().ToString().Substring(0, 8)}{fileExtension}";

                // Save path
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder); // ensure it exists

                savedFilePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file
                using (var stream = new FileStream(savedFilePath, FileMode.Create))
                {
                    await model.CertificateFile.CopyToAsync(stream);
                }
            }

            // Optional: URL to access the file from client
            var relativePath = savedFilePath.Replace(Directory.GetCurrentDirectory(), "").Replace("\\", "/");
            var fileUrl = $"/{relativePath.TrimStart('/')}";



            //Api Call
            var url = $"{_baseApiUrl}api/UserInformationSkillMap/Add";


            var loginUserSkillMap = new LoginUserSkillMap
            {
                UserCode = model.UserCode,
                SkillId = model.SkillId,
                SkillLevelId = model.SkillLevelId,
                CertificateName = model.CertificateName,
                CertificatePath = fileUrl,
                HandsOnExperience = model.HandsOnExperience,
                HandsOnExperienceFromDate = model.HandsOnExperienceFromDate,
                HandsOnExperienceToDate = model.HandsOnExperienceToDate
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(loginUserSkillMap),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(url, jsonContent);

            var content = await response.Content.ReadAsStringAsync();

            var user = JsonSerializer.Deserialize<LoginUser>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });


            return Ok(new { message = "Skill data submitted successfully." });
        }




        [HttpGet("GetSkillByUserCode")]
        public IActionResult GetSkillByUserCode()
        {
            return View();
        }

        [HttpGet("GetUserList")]
        public IActionResult GetUserList()
        {
            return View();
        }


    }
}
