using EPYTST.API.Contollers.ReportAPI;
using EPYTST.Application.Entities;
using EPYTST.Application.Interfaces;
using EPYTST.Infrastructure.CustomException;
using EPYTST.Infrastructure.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace EPYTST.API.Contollers.LoginUserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginUserController : ControllerBase
    {

        private readonly ILoginUserService _LoginUserService;
        private readonly ILogger<ReportAPIController> _logger;
        private readonly IMemoryCache _cache;
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1); //// For restric same cache key access multiple user at a time
        private readonly HttpClient _httpClient;

        public LoginUserController(ILoginUserService LoginUserService, ILogger<ReportAPIController> logger, IMemoryCache cache, IHttpClientFactory httpClientFactory)
        {
            this._LoginUserService = LoginUserService;
            this._logger = logger;
            this._cache = cache;
            _httpClient = httpClientFactory.CreateClient();
        }



        [HttpPost("LogIn")]
        [ProducesResponseType(typeof(LoginUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<LoginUser> LogIn(LoginRequest login)
        {
            // Combine username and password as required by the API
            string combined = $"{login.Username}_Te_yW_2_4_mO{login.Password}";

            // Build the full URL
            string url = $"http://172.16.2.14:8042/api/Weather-Forecast/GetLogin/{combined}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new ItemNotFoundException(ErrorKeys.NoRecord);

                throw new Exception("Error calling login API: " + response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsStringAsync();

            var user = JsonSerializer.Deserialize<LoginUser>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (user == null)
                throw new ItemNotFoundException(ErrorKeys.NoRecord);

            return user;
        }




        [HttpPost("Add")]
        [ProducesResponseType(typeof(LoginUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddLoginUser([FromBody] LoginUser LoginUser)
        {
            var lst = await _LoginUserService.AddAsync(LoginUser);
            _cache.Remove(InMemoryCacheKeys.APIReports); //// Remove cache key
            return Ok(lst);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var lst = await _LoginUserService.GetAllAsync();
                if (lst != null)
                {
                    return Ok(lst);
                }
                return Ok(new List<LoginUser>());
            }
            catch (Exception ex)
            {
               throw ex.GetBaseException();
            }
        }

        [HttpGet("GetById")]
        [ProducesResponseType(typeof(LoginUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(string id)
        {
           var data = await _LoginUserService.GetByIdAsync(id);

            return Ok(data);
        }


        [HttpPut("Update")]
        [ProducesResponseType(typeof(LoginUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLoginUser(int id, [FromBody] LoginUser LoginUser)
        {
            var lst = await _LoginUserService.UpdateAsync(id,LoginUser);
            _cache.Remove(InMemoryCacheKeys.APIReports); //// Remove cache key
            return Ok(lst);
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(typeof(LoginUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLoginUser(string Id)
        {
            var lst = await _LoginUserService.DeleteAsync(Id);
            return Ok(lst);
        }



    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
