using EPYTST.Application.Entities;
using EPYTST.Application.Entities.ReportAPI;
using EPYTST.Application.Interfaces;
using EPYTST.Infrastructure.CustomException;
using EPYTST.Infrastructure.Data;
using EPYTST.Infrastructure.Static;
using System.Data;
using System.Data.Entity;
using System.Text.RegularExpressions;
using static Dapper.SqlMapper;

namespace EPYTST.Application.Services
{
    public class SkillService : ISkillService
    {
        //private readonly DapperDBContext _dbContext;
        //public SkillService(DapperDBContext dapperDBContext)
        //{
        //    _dbContext = dapperDBContext;
        //}

        private readonly IDapperCRUDService<Skill> _dbService;
        private readonly IDapperCRUDService<ReportAPIUserHistory> _dbServiceUH;
        //private readonly SqlConnection _connection;
        public SkillService(IDapperCRUDService<Skill> dbService, IDapperCRUDService<ReportAPIUserHistory> dbServiceUH)
        {
            _dbService = dbService;
            _dbServiceUH = dbServiceUH;
            //_connection = _dbService.Connection;
        }

        public async Task<Skill> AddAsync(Skill item)
        {

            var nextId = await GetNextSkillId();
            item.SkillId = nextId;

            var savedEntity = await _dbService.SaveEntityAsync(item) ??
                    throw new Exception(ErrorKeys.UnsuccesfullInsertUpdate);
            return savedEntity;
        }

        public async Task<string> DeleteAsync(string Id)
        {
            try
            {
                string response = string.Empty;
   
                bool isDeleted = await _dbService.DeleteEntityAsync(new Skill(), Id);
                if (isDeleted)
                {
                    response = "Deleted successfully.";
                }
                else
                {
                    response = "Could not be deleted.";
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Skill>> GetAllAsync()
        {
            string query = "Select s.SkillId, s.SkillName, s.Description, s.DateAdded, s.AddedBy,s.DateUpdated, s.UpdatedBy, s.IsActive,  lu.Name as AddedByName From Skill as s Inner Join EPYHRMS..LoginUser as lu on s.AddedBy = lu.UserCode";
            return await _dbService.GetDataAsync<Skill>(query);
        }

        public async Task<IEnumerable<Skill>> GetAsync()
        {
            ////No need to write try catch. Exception will handle by GlobalExceptionHandler middleware
            //try
            //{
                string query = "Select * From Skill where IsActive = 1";
                //using (var conn = this._dbContext.CreateConnection())
                //{ 
                //    var lst = await conn.QueryAsync<Skill>(query);
                //    return lst.ToList();
                //}

                return await _dbService.GetDataAsync<Skill>(query);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public async Task<Skill> GetByIdAsync(string Id)
        {
            string query = "Select * From Skill Where SkillId=@Id";
            var item = await _dbService.GetFirstOrDefaultAsync<Skill>(query, new { Id }) ??
                throw new ItemNotFoundException(ErrorKeys.NoRecord);

            return item;
        }

        public async Task<bool> UpdateAsync(long Id, Skill item)
        {
            var savedEntity = await _dbService.SaveEntityAsync(item) ??
                 throw new Exception(ErrorKeys.UnsuccesfullInsertUpdate);
            return true;
        }



        public async Task<int> GetNextSkillId()
        {
            string query = "SELECT ISNULL(MAX(SkillId), 0) + 1 FROM Skill";
            int nextId = await _dbService.GetScalarAsync<int>(query);
            return nextId;
        }

        
    }
}
