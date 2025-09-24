using EPYTST.API.Contollers.ReportAPI;
using EPYTST.Application.Entities;
using EPYTST.Application.Interfaces;
using EPYTST.Infrastructure.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace EPYTST.API.Contollers.UserInformationSkillMapAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInformationSkillMapController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IUserInformationSkillMapService _UserInformationSkillMapService;
        private readonly ISkillLevelService _skillLevelService;
        private readonly ILogger<ReportAPIController> _logger;
        private readonly IMemoryCache _cache;
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1); //// For restric same cache key access multiple user at a time


        public UserInformationSkillMapController(IUserInformationSkillMapService UserInformationSkillMapService, ILogger<ReportAPIController> logger, IMemoryCache cache, ISkillLevelService SkillLevelService, HttpClient httpClient)
        {
            this._UserInformationSkillMapService = UserInformationSkillMapService;
            this._logger = logger;
            this._cache = cache;
            this._skillLevelService = SkillLevelService;
            _httpClient = httpClient;
        }


        [HttpPost("Add")]
        [ProducesResponseType(typeof(UserInformationSkillMap), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUserInformationSkillMap([FromBody] UserInformationSkillMap UserInformationSkillMap)
        {
            UserInformationSkillMap.DateAdded = DateTime.UtcNow;
            UserInformationSkillMap.IsActive = true;
           var lst = await _UserInformationSkillMapService.AddAsync(UserInformationSkillMap);
            _cache.Remove(InMemoryCacheKeys.APIReports); //// Remove cache key
            return Ok(lst);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var lst = await _UserInformationSkillMapService.GetAllAsync();
                if (lst != null)
                {
                    return Ok(lst);
                }
                return Ok(new List<UserInformationSkillMap>());
            }
            catch (Exception ex)
            {
               throw ex.GetBaseException();
            }
        }

        [HttpGet("GetById")]
        [ProducesResponseType(typeof(UserInformationSkillMap), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(string id)
        {
           var data = await _UserInformationSkillMapService.GetByIdAsync(id);

           return Ok(data);
        }


        [HttpPut("Update")]
        [ProducesResponseType(typeof(UserInformationSkillMap), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserInformationSkillMap(int id, [FromBody] UserInformationSkillMap UserInformationSkillMap)
        {
            var lst = await _UserInformationSkillMapService.UpdateAsync(id,UserInformationSkillMap);
            _cache.Remove(InMemoryCacheKeys.APIReports); //// Remove cache key
            return Ok(lst);
        }

        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(typeof(UserInformationSkillMap), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUserInformationSkillMap(string Id)
        {
            var lst = await _UserInformationSkillMapService.DeleteAsync(Id);
            return Ok(lst);
        }


        [HttpGet("GetByUserCode")]
        [ProducesResponseType(typeof(UserInformationSkillMap), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByUserCode(int userCode)
        {
            var data = await _skillLevelService.GetByUserCode(userCode);

            return Ok(data);
        }


        [HttpGet("GetUserList")]
        [ProducesResponseType(typeof(List<UserIntormetionVM>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<UserIntormetionVM>>> GetUserList()
        {
            try
            {
                string externalUrl = "http://172.16.2.14:8042/api/Weather-Forecast/GetQueryResult/Eip4325";

                var response = await _httpClient.GetAsync(externalUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "External API error: " + response.ReasonPhrase);
                }

                var content = await response.Content.ReadAsStringAsync();

                var users = JsonSerializer.Deserialize<List<UserIntormetionVM>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (users == null)
                {
                    return Ok(new List<UserIntormetionVM>());
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error: " + ex.Message);
            }
        }





        [HttpGet("GetUserBySkillId")]
        [ProducesResponseType(typeof(UserInformationSkillMap), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserBySkillId(int skillId)
        {
            var data = await _skillLevelService.GetBySkillId(skillId);

            return Ok(data);
        }



    }
}
