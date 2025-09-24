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
    public class DepartmentService : IDepartmentService
    {
        //private readonly DapperDBContext _dbContext;
        //public DepartmentService(DapperDBContext dapperDBContext)
        //{
        //    _dbContext = dapperDBContext;
        //}

        private readonly IDapperCRUDService<Department> _dbService;
        private readonly IDapperCRUDService<ReportAPIUserHistory> _dbServiceUH;
        //private readonly SqlConnection _connection;
        public DepartmentService(IDapperCRUDService<Department> dbService, IDapperCRUDService<ReportAPIUserHistory> dbServiceUH)
        {
            _dbService = dbService;
            _dbServiceUH = dbServiceUH;
            //_connection = _dbService.Connection;
        }

        public async Task<Department> AddAsync(Department item)
        {

            var nextId = await GetNextDepartmentId();
            item.DepartmentId = nextId;

            var savedEntity = await _dbService.SaveEntityAsync(item) ??
                    throw new Exception(ErrorKeys.UnsuccesfullInsertUpdate);
            return savedEntity;
        }

        public async Task<string> DeleteAsync(string Id)
        {
            try
            {
                string response = string.Empty;
   
                bool isDeleted = await _dbService.DeleteEntityAsync(new Department(), Id);
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

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            string query = "Select * From Department";
            return await _dbService.GetDataAsync<Department>(query);
        }

        public async Task<IEnumerable<Department>> GetAsync()
        {
            ////No need to write try catch. Exception will handle by GlobalExceptionHandler middleware
            //try
            //{
                string query = "Select * From Department";
                //using (var conn = this._dbContext.CreateConnection())
                //{ 
                //    var lst = await conn.QueryAsync<Department>(query);
                //    return lst.ToList();
                //}

                return await _dbService.GetDataAsync<Department>(query);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public async Task<Department> GetByIdAsync(string Id)
        {
            string query = "Select * From Department Where DepartmentId=@Id";
            var item = await _dbService.GetFirstOrDefaultAsync<Department>(query, new { Id }) ??
                throw new ItemNotFoundException(ErrorKeys.NoRecord);

            return item;
        }

        public async Task<bool> UpdateAsync(long Id, Department item)
        {
            try
            {
                item.DepartmentId = (int)Id;

                var savedEntity = await _dbService.SaveEntityAsync(item);
                    //?? throw new Exception(ErrorKeys.UnsuccesfullInsertUpdate);

                return true;
            }
            catch (Exception ex)
            {

                throw ex.GetBaseException();
            }

           
        }



        public async Task<int> GetNextDepartmentId()
        {
            string query = "SELECT ISNULL(MAX(DepartmentId), 0) + 1 FROM Department";
            int nextId = await _dbService.GetScalarAsync<int>(query);
            return nextId;
        }

        
    }
}
