using EPYTST.Application.Entities;
using EPYTST.Application.Entities.ReportAPI;
using EPYTST.Application.Entities.UserInformation;
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
    public class UserInformationSkillMapService : IUserInformationSkillMapService
    {
        //private readonly DapperDBContext _dbContext;
        //public LoginUserSkillMapService(DapperDBContext dapperDBContext)
        //{
        //    _dbContext = dapperDBContext;
        //}

        private readonly IDapperCRUDService<UserInformationSkillMap> _dbService;
        private readonly IDapperCRUDService<ReportAPIUserHistory> _dbServiceUH;
        //private readonly SqlConnection _connection;
        public UserInformationSkillMapService(IDapperCRUDService<UserInformationSkillMap> dbService, IDapperCRUDService<ReportAPIUserHistory> dbServiceUH)
        {
            _dbService = dbService;
            _dbServiceUH = dbServiceUH;
            //_connection = _dbService.Connection;
        }

        public async Task<UserInformationSkillMap> AddAsync(UserInformationSkillMap item)
        {
            var nextId = await GetNextLoginUserSkillMapId();
            item.UserInformationSkillMapId = nextId;

            var savedEntity = await _dbService.SaveEntityAsync(item) ??
                    throw new Exception(ErrorKeys.UnsuccesfullInsertUpdate);
            return savedEntity;
        }

        public async Task<string> DeleteAsync(string Id)
        {
            try
            {
                string response = string.Empty;
   
                bool isDeleted = await _dbService.DeleteEntityAsync(new UserInformationSkillMap(), Id);
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

        public async Task<IEnumerable<UserInformationSkillMap>> GetAllAsync()
        {
            string query = "Select * From UserInformationSkillMap where IsActive = 1";
            return await _dbService.GetDataAsync<UserInformationSkillMap>(query);
        }

        public async Task<IEnumerable<UserInformationSkillMap>> GetAsync()
        {
            ////No need to write try catch. Exception will handle by GlobalExceptionHandler middleware
            //try
            //{
                string query = "Select * From LoginUserSkillMap";
                //using (var conn = this._dbContext.CreateConnection())
                //{ 
                //    var lst = await conn.QueryAsync<LoginUserSkillMap>(query);
                //    return lst.ToList();
                //}

                return await _dbService.GetDataAsync<UserInformationSkillMap>(query);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public async Task<UserInformationSkillMap> GetByIdAsync(string Id)
        {
            string query = "Select * From LoginUserSkillMap Where LoginUserSkillMapId=@Id";
            var item = await _dbService.GetFirstOrDefaultAsync<UserInformationSkillMap>(query, new { Id }) ??
                throw new ItemNotFoundException(ErrorKeys.NoRecord);

            return item;
        }


        public async Task<UserInformationSkillMap> GetUserInformationSkillMapByIdAsync(string Id)
        {
            string query = "Select * From UserInformationSkillMap Where UserInformationSkillMapId=@Id";
            var item = await _dbService.GetFirstOrDefaultAsync<UserInformationSkillMap>(query, new { Id }) ??
                throw new ItemNotFoundException(ErrorKeys.NoRecord);

            return item;
        }


        public async Task<bool> UpdateAsync(long Id, UserInformationSkillMap item)
        {
            //item.LoginUserSkillMapId = (int)Id;
            var savedEntity = await _dbService.SaveEntityAsync(item) ??
                 throw new Exception(ErrorKeys.UnsuccesfullInsertUpdate);
            return true;
        }

        public async Task<int> GetNextLoginUserSkillMapId()
        {
            string query = "SELECT ISNULL(MAX(UserInformationSkillMapId), 0) + 1 FROM UserInformationSkillMap";
            int nextId = await _dbService.GetScalarAsync<int>(query);
            return nextId;
        }



        //Check User
        public async Task<UserInformation> GetUserByUserNameAsync(int userName)
        {
            string query = "Select * From UserInformation Where HumanSpiritNo=@userName";
            var item = await _dbService.GetFirstOrDefaultAsync<UserInformation>(query, new { userName }) ??
                throw new ItemNotFoundException(ErrorKeys.NoRecord);

            return item;
        }


        public async Task<bool> UpdateUserInformationSkillMapAsync(int Id, UserInformationSkillMap item)
        {

            ////item.LoginUserSkillMapId = (int)Id;
            //var savedEntity = await _dbService.SaveEntityAsync(item) ??
            //     throw new Exception(ErrorKeys.UnsuccesfullInsertUpdate);

            string query = @"
                    UPDATE UserInformationSkillMap
                    SET
                        SkillId = @SkillId,
                        SkillLevelId = @SkillLevelId,
                        CertificateName = @CertificateName,
                        CertificatePath = @CertificatePath,
                        HandsOnExperience = @HandsOnExperience,
                        HandsOnExperienceFromDate = @HandsOnExperienceFromDate,
                        HandsOnExperienceToDate = @HandsOnExperienceToDate,
                        DateUpdated = GETDATE(),
                        UpdatedBy = @UpdatedBy
                    WHERE UserInformationSkillMapId = @Id";

            var parameters = new
            {
                Id,
                item.SkillId,
                item.SkillLevelId,
                item.CertificateName,
                item.CertificatePath,
                item.HandsOnExperience,
                item.HandsOnExperienceFromDate,
                item.HandsOnExperienceToDate,
                item.UpdatedBy
            };

            var result = await _dbService.ExecuteAsync(query, parameters);

            if (result <= 0)
                throw new Exception(ErrorKeys.UnsuccesfullInsertUpdate);

            return true;
            //return true;
        }

    }
}
