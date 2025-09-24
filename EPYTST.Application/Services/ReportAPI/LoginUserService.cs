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
    public class LoginUserService : ILoginUserService
    {
        //private readonly DapperDBContext _dbContext;
        //public LoginUserService(DapperDBContext dapperDBContext)
        //{
        //    _dbContext = dapperDBContext;
        //}

        private readonly IDapperCRUDService<LoginUser> _dbService;
        private readonly IDapperCRUDService<ReportAPIUserHistory> _dbServiceUH;
        //private readonly SqlConnection _connection;
        public LoginUserService(IDapperCRUDService<LoginUser> dbService, IDapperCRUDService<ReportAPIUserHistory> dbServiceUH)
        {
            _dbService = dbService;
            _dbServiceUH = dbServiceUH;
            //_connection = _dbService.Connection;
        }

        public async Task<LoginUser> AddAsync(LoginUser item)
        {

            var nextId = await GetNextLoginUserId();
            item.UserCode = nextId;

            var savedEntity = await _dbService.SaveEntityAsync(item) ??
                    throw new Exception(ErrorKeys.UnsuccesfullInsertUpdate);
            return savedEntity;
        }

        public async Task<string> DeleteAsync(string Id)
        {
            try
            {
                string response = string.Empty;
   
                bool isDeleted = await _dbService.DeleteEntityAsync(new LoginUser(), Id);
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

        public async Task<IEnumerable<LoginUser>> GetAllAsync()
        {
            string query = "Select * From LoginUser";
            return await _dbService.GetDataAsync<LoginUser>(query);
        }

        public async Task<IEnumerable<LoginUser>> GetAsync()
        {
            ////No need to write try catch. Exception will handle by GlobalExceptionHandler middleware
            //try
            //{
                string query = "Select * From LoginUser";
                //using (var conn = this._dbContext.CreateConnection())
                //{ 
                //    var lst = await conn.QueryAsync<LoginUser>(query);
                //    return lst.ToList();
                //}

                return await _dbService.GetDataAsync<LoginUser>(query);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public async Task<LoginUser> GetByIdAsync(string Id)
        {
            string query = "Select * From LoginUser Where LoginUserId=@Id";
            var item = await _dbService.GetFirstOrDefaultAsync<LoginUser>(query, new { Id }) ??
                throw new ItemNotFoundException(ErrorKeys.NoRecord);

            return item;
        }

        public async Task<bool> UpdateAsync(long Id, LoginUser item)
        {
            item.UserCode = (int)Id;

            var savedEntity = await _dbService.SaveEntityAsync(item) ??
                 throw new Exception(ErrorKeys.UnsuccesfullInsertUpdate);
            return true;
        }



        public async Task<int> GetNextLoginUserId()
        {
            string query = "SELECT ISNULL(MAX(LoginUserId), 0) + 1 FROM LoginUser";
            int nextId = await _dbService.GetScalarAsync<int>(query);
            return nextId;
        }

        
    }
}
