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
    public class SkillLevelService : ISkillLevelService
    {
        //private readonly DapperDBContext _dbContext;
        //public SkillLevelService(DapperDBContext dapperDBContext)
        //{
        //    _dbContext = dapperDBContext;
        //}

        private readonly IDapperCRUDService<SkillLevel> _dbService;
        private readonly IDapperCRUDService<ReportAPIUserHistory> _dbServiceUH;
        //private readonly SqlConnection _connection;
        public SkillLevelService(IDapperCRUDService<SkillLevel> dbService, IDapperCRUDService<ReportAPIUserHistory> dbServiceUH)
        {
            _dbService = dbService;
            _dbServiceUH = dbServiceUH;
            //_connection = _dbService.Connection;
        }

        public async Task<SkillLevel> AddAsync(SkillLevel item)
        {

            var nextId = await GetNextSkillLevelId();
            item.SkillLevelId = nextId;

            var savedEntity = await _dbService.SaveEntityAsync(item) ??
                    throw new Exception(ErrorKeys.UnsuccesfullInsertUpdate);
            return savedEntity;
        }

        public async Task<string> DeleteAsync(string Id)
        {
            try
            {
                string response = string.Empty;
   
                bool isDeleted = await _dbService.DeleteEntityAsync(new SkillLevel(), Id);
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

        public async Task<IEnumerable<SkillLevel>> GetAllAsync()
        {
            string query = "select sl.SkillLevelId, sl.Name, sl.DateAdded, sl.AddedBy, sl.DateUpdated, sl.UpdatedBy, sl.IsActive, lu.Name as AddedByName from SkillLevel as sl Inner Join  EPYHRMS..LoginUser as lu on sl.AddedBy = lu.UserCode";
            return await _dbService.GetDataAsync<SkillLevel>(query);
        }

        public async Task<IEnumerable<SkillLevel>> GetAsync()
        {
            ////No need to write try catch. Exception will handle by GlobalExceptionHandler middleware
            //try
            //{
                string query = "Select * From SkillLevel where IsActive = 1";
                //using (var conn = this._dbContext.CreateConnection())
                //{ 
                //    var lst = await conn.QueryAsync<SkillLevel>(query);
                //    return lst.ToList();
                //}

                return await _dbService.GetDataAsync<SkillLevel>(query);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public async Task<SkillLevel> GetByIdAsync(string Id)
        {
            string query = "Select * From SkillLevel Where SkillLevelId=@Id";
            var item = await _dbService.GetFirstOrDefaultAsync<SkillLevel>(query, new { Id }) ??
                throw new ItemNotFoundException(ErrorKeys.NoRecord);

            return item;
        }

        public async Task<List<UserInformationSkillMap>> GetByUserCode(int userCode)
        {
            //string query = "SELECT * FROM UserInformationSkillMap as uis WHERE uis.UserCode = @userCode";
            string query = @"
                            SELECT 
                                uis.UserInformationSkillMapId, 
                                uis.UserCode, 
                                s.SkillName, 
                                sl.Name AS SkillLevelName,
                                uis.CertificateName as CertificateName,
                                uis.CertificatePath as CertificatePath,
                                uis.HandsOnExperience as HandsOnExperience,
                                uis.HandsOnExperienceFromDate as HandsOnExperienceFromDate,
                                uis.HandsOnExperienceToDate as HandsOnExperienceToDate
                            FROM UserInformationSkillMap AS uis
                            INNER JOIN Skill s ON uis.SkillId = s.SkillId
                            INNER JOIN SkillLevel sl ON sl.SkillLevelId = uis.SkillLevelId
                            WHERE uis.UserCode = @userCode";

            var items = await _dbService.GetDataAsync<UserInformationSkillMap>(query, new { userCode });

            if (items == null || !items.Any())
            {
                throw new ItemNotFoundException(ErrorKeys.NoRecord);
            }

            return items.ToList();
        }

        public async Task<List<UserBySkillVM>> GetBySkillId(int skillId)
        {
            string query = @"
        SELECT 
            lu.UserCode,
            lu.UserName,
            lu.Name AS EmployeeName,
            lu.Email,
            lu.IsAdmin,
            lu.IsActive,
            lu.CompanyID,
            s.SkillName,
            sl.Name AS SkillLeveName,
            uism.HandsOnExperienceFromDate,
            uism.HandsOnExperienceToDate,

            -- Experience Duration (only show non-zero parts, or default to '0')
            CASE 
                WHEN uism.HandsOnExperienceFromDate IS NOT NULL AND uism.HandsOnExperienceToDate IS NOT NULL THEN
                (
                    SELECT 
                        CASE 
                            WHEN years + months + days = 0 THEN '0'
                            ELSE
                                CONCAT(
                                    CASE WHEN years > 0 THEN CAST(years AS VARCHAR) + ' year(s) ' ELSE '' END,
                                    CASE WHEN months > 0 THEN CAST(months AS VARCHAR) + ' month(s) ' ELSE '' END,
                                    CASE WHEN days > 0 THEN CAST(days AS VARCHAR) + ' day(s)' ELSE '' END
                                )
                        END
                    FROM (
                        SELECT 
                            DATEDIFF(YEAR, uism.HandsOnExperienceFromDate, uism.HandsOnExperienceToDate) AS years,
                            DATEDIFF(MONTH, uism.HandsOnExperienceFromDate, uism.HandsOnExperienceToDate) % 12 AS months,
                            DATEDIFF(DAY, DATEADD(MONTH, DATEDIFF(MONTH, uism.HandsOnExperienceFromDate, uism.HandsOnExperienceToDate), uism.HandsOnExperienceFromDate), uism.HandsOnExperienceToDate) AS days
                    ) AS duration
                )
                ELSE 'N/A'
            END AS ExperienceDuration

        FROM 
            EPYHRMS..LoginUser AS lu
            INNER JOIN UserInformationSkillMap AS uism ON lu.UserCode = uism.UserCode
            INNER JOIN Skill s ON s.SkillId = uism.SkillId
            INNER JOIN SkillLevel sl ON sl.SkillLevelId = uism.SkillLevelId
        WHERE 
            (@skillId = 0 OR s.SkillId = @skillId);";

            var items = await _dbService.GetDataAsync<UserBySkillVM>(query, new { skillId });

            if (items == null || !items.Any())
            {
                throw new ItemNotFoundException(ErrorKeys.NoRecord);
            }

            return items.ToList();
        }

        public async Task<bool> UpdateAsync(long Id, SkillLevel item)
        {
            var savedEntity = await _dbService.SaveEntityAsync(item) ??
                 throw new Exception(ErrorKeys.UnsuccesfullInsertUpdate);
            return true;
        }

        public async Task<int> GetNextSkillLevelId()
        {
            string query = "SELECT ISNULL(MAX(SkillLevelId), 0) + 1 FROM SkillLevel";
            int nextId = await _dbService.GetScalarAsync<int>(query);
            return nextId;
        }

    }
}
