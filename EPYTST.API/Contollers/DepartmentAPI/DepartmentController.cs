using EPYTST.API.Contollers.ReportAPI;
using EPYTST.Application.Entities;
using EPYTST.Application.Interfaces;
using EPYTST.Infrastructure.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace EPYTST.API.Contollers.DepartmentAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly IDepartmentService _departmentService;
        private readonly ILogger<ReportAPIController> _logger;
        private readonly IMemoryCache _cache;
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1); //// For restric same cache key access multiple user at a time


        public DepartmentController(IDepartmentService departmentService, ILogger<ReportAPIController> logger, IMemoryCache cache)
        {
            this._departmentService = departmentService;
            this._logger = logger;
            this._cache = cache;
        }


        [HttpPost("Add")]
        [ProducesResponseType(typeof(Department), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddDepartment([FromBody] Department department)
        {
            var lst = await _departmentService.AddAsync(department);
            _cache.Remove(InMemoryCacheKeys.APIReports); //// Remove cache key
            return Ok(lst);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var lst = await _departmentService.GetAllAsync();
                if (lst != null)
                {
                    return Ok(lst);
                }
                return Ok(new List<Department>());
            }
            catch (Exception ex)
            {
               throw ex.GetBaseException();
            }
        }

        [HttpGet("GetById")]
        [ProducesResponseType(typeof(Department), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(string id)
        {
           var data = await _departmentService.GetByIdAsync(id);

            return Ok(data);
        }


        [HttpPut("Update")]
        [ProducesResponseType(typeof(Department), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department department)
        {
            var lst = await _departmentService.UpdateAsync(id,department);
            _cache.Remove(InMemoryCacheKeys.APIReports); //// Remove cache key
            return Ok(lst);
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(typeof(Department), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDepartment(string Id)
        {
            var lst = await _departmentService.DeleteAsync(Id);
            return Ok(lst);
        }



    }
}
