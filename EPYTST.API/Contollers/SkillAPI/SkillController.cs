using EPYTST.API.Contollers.ReportAPI;
using EPYTST.Application.Entities;
using EPYTST.Application.Interfaces;
using EPYTST.Infrastructure.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace EPYTST.API.Contollers.SkillAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {

        private readonly ISkillService _SkillService;
        private readonly ILogger<ReportAPIController> _logger;
        private readonly IMemoryCache _cache;
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1); //// For restric same cache key access multiple user at a time


        public SkillController(ISkillService SkillService, ILogger<ReportAPIController> logger, IMemoryCache cache)
        {
            this._SkillService = SkillService;
            this._logger = logger;
            this._cache = cache;
        }


        [HttpPost("Add")]
        [ProducesResponseType(typeof(Skill), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddSkill([FromBody] Skill Skill)
        {
            var lst = await _SkillService.AddAsync(Skill);
            _cache.Remove(InMemoryCacheKeys.APIReports); //// Remove cache key
            return Ok(lst);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var lst = await _SkillService.GetAllAsync();
                if (lst != null)
                {
                    return Ok(lst);
                }
                return Ok(new List<Skill>());
            }
            catch (Exception ex)
            {
               throw ex.GetBaseException();
            }
        }

        [HttpGet("GetById/{id}")]
        [ProducesResponseType(typeof(Skill), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(string id)
        {
           var data = await _SkillService.GetByIdAsync(id);

            return Ok(data);
        }


        [HttpPut("Update/{id}")]
        [ProducesResponseType(typeof(Skill), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSkill(int id, [FromBody] Skill Skill)
        {
            var lst = await _SkillService.UpdateAsync(id,Skill);
            _cache.Remove(InMemoryCacheKeys.APIReports); //// Remove cache key
            return Ok(lst);
        }

        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(typeof(Skill), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSkill(string Id)
        {
            var lst = await _SkillService.DeleteAsync(Id);
            return Ok(lst);
        }



    }
}
