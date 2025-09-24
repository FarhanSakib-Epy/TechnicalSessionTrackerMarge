using EPYTST.API.Contollers.ReportAPI;
using EPYTST.Application.Entities;
using EPYTST.Application.Interfaces;
using EPYTST.Infrastructure.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace EPYTST.API.Contollers.SkillLevelAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillLevelController : ControllerBase
    {

        private readonly ISkillLevelService _SkillLevelService;
        private readonly ILogger<ReportAPIController> _logger;
        private readonly IMemoryCache _cache;
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1); //// For restric same cache key access multiple user at a time


        public SkillLevelController(ISkillLevelService SkillLevelService, ILogger<ReportAPIController> logger, IMemoryCache cache)
        {
            this._SkillLevelService = SkillLevelService;
            this._logger = logger;
            this._cache = cache;
        }


        [HttpPost("Add")]
        [ProducesResponseType(typeof(SkillLevel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddSkillLevel([FromBody] SkillLevel SkillLevel)
        {
            var lst = await _SkillLevelService.AddAsync(SkillLevel);
            _cache.Remove(InMemoryCacheKeys.APIReports); //// Remove cache key
            return Ok(lst);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var lst = await _SkillLevelService.GetAllAsync();
                if (lst != null)
                {
                    return Ok(lst);
                }
                return Ok(new List<SkillLevel>());
            }
            catch (Exception ex)
            {
               throw ex.GetBaseException();
            }
        }

        [HttpGet("GetById/{id}")]
        [ProducesResponseType(typeof(SkillLevel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(string id)
        {
           var data = await _SkillLevelService.GetByIdAsync(id);

            return Ok(data);
        }


        [HttpPut("Update")]
        [ProducesResponseType(typeof(SkillLevel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSkillLevel(int id, [FromBody] SkillLevel SkillLevel)
        {
            var lst = await _SkillLevelService.UpdateAsync(id,SkillLevel);
            _cache.Remove(InMemoryCacheKeys.APIReports); //// Remove cache key
            return Ok(lst);
        }

        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(typeof(SkillLevel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSkillLevel(string Id)
        {
            var lst = await _SkillLevelService.DeleteAsync(Id);
            return Ok(lst);
        }



    }
}
