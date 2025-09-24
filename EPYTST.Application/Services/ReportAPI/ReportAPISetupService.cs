using EPYTST.Application.Entities;
using EPYTST.Application.Entities.ReportAPI;
using EPYTST.Application.Interfaces;
using EPYTST.Infrastructure.CustomException;
using EPYTST.Infrastructure.Data;
using EPYTST.Infrastructure.Static;
using System.Data;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace EPYTST.Application.Services
{
    public class ReportAPISetupService : IReportAPISetupService
    {
        //private readonly DapperDBContext _dbContext;
        //public ReportAPISetupService(DapperDBContext dapperDBContext)
        //{
        //    _dbContext = dapperDBContext;
        //}

        private readonly IDapperCRUDService<ReportAPISetup> _dbService;
        private readonly IDapperCRUDService<ReportAPIUserHistory> _dbServiceUH;
        //private readonly SqlConnection _connection;
        public ReportAPISetupService(IDapperCRUDService<ReportAPISetup> dbService, IDapperCRUDService<ReportAPIUserHistory> dbServiceUH)
        {
            _dbService = dbService;
            _dbServiceUH = dbServiceUH;
            //_connection = _dbService.Connection;
        }
        public async Task<IEnumerable<ReportAPISetup>> GetAllAsync()
        {
            ////No need to write try catch. Exception will handle by GlobalExceptionHandler middleware
            //try
            //{
                string query = "Select * From ReportAPISetup";
                //using (var conn = this._dbContext.CreateConnection())
                //{ 
                //    var lst = await conn.QueryAsync<ReportAPISetup>(query);
                //    return lst.ToList();
                //}

                return await _dbService.GetDataAsync<ReportAPISetup>(query);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public async Task<ReportAPISetup> GetAPIReportByReportName(string reportName)
        {
            //try
            //{
                string query = "Select * From ReportAPISetup Where ReportName=@ReportName";
                //using (var conn = this._dbContext.CreateConnection())
                //{
                //    var lst = await conn.QueryFirstOrDefaultAsync<ReportAPISetup>(query, new { reportName});
                //    return lst;
                //}
                return await _dbService.GetFirstOrDefaultAsync<ReportAPISetup>(query, new { reportName });

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public async Task<ReportAPISetup> AddAsync(ReportAPISetup reportAPISetup)
        {
            //SqlTransaction transaction = null;
            //try
            //{
                //await _connection.OpenAsync();
                //transaction = _connection.BeginTransaction();
                ////SqlMapperExtensions.TableNameMapper =

                //ReportAPISetup objM = await GetAPIReportByReportName(reportAPISetup.ReportName);
                //if (objM == null)
                //{
                //    objM = new ReportAPISetup();
                //    objM.EntityState = EntityState.Added;
                //    objM.ReportName = reportAPISetup.ReportName;
                //}
                //else { 
                //    objM.SQL = reportAPISetup.SQL;
                //    objM.Parameters = reportAPISetup.Parameters;
                //    objM.IsStoredProcedure = reportAPISetup.IsStoredProcedure;
                //}
                //await _dbService.SaveSingleAsync(objM, transaction);
                //transaction.Commit();

                //return objM;

                //return await _dbService.SaveEntityAsync(reportAPISetup);
                var savedEntity = await _dbService.SaveEntityAsync(reportAPISetup) ??
                    throw new Exception(ErrorKeys.UnsuccesfullInsertUpdate);
                return savedEntity;
            //}
            //catch (Exception ex)
            //{
            //    //if (transaction != null) transaction.Rollback();
            //    throw ex;
            //}
            //finally
            //{
            //    //_connection.Close();
            //}
        }

        public async Task<string> DeleteAsync(string reportName)
        {
            try
            {
                string response = string.Empty;
                //string query = "Delete From ReportAPISetup Where ReportName=@ReportName";
                ////using (var conn = this._dbContext.CreateConnection())
                ////{
                ////    var lst = await conn.ExecuteAsync(query, new { reportName });
                ////    response = "Pass";
                ////}
                //var ss = await _dbService.ExecuteAsync(query, new { reportName });
                //response = "Pass";
                //return response;


                bool isDeleted = await _dbService.DeleteEntityAsync(new ReportAPISetup(), reportName);
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

        public async Task<dynamic> GetDynamicReportAPIDataAsync(string username, string token, string reportName, string values,string localIP)
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            try
            {
                string query = $@";With u As (
	                                Select UserCode From EPYSL..LoginUser  Where UserName = @UserName
                                ),c As (
	                                Select x.ReportID, TotalReportCall = Count(x.CallingStartTime),MAX(x.CallingStartTime) AS ReportCallingTime
	                                From ReportAPIUserHistory x
	                                Inner Join u on u.UserCode = x.UserCode
	                                Where Convert(date, x.CallingStartTime) = Convert(date, getdate())
	                                Group By ReportID
                                ), d As(
	                                Select x.ReportID, x.UserCode, x.LimitPerDay, x.TimeLimit
	                                From ReportAPISetupUserPermissionDaily x
	                                Inner Join u on u.UserCode = x.UserCode
	                                Where Convert(date, x.UseDate) = Convert(date, getdate())
                                ), b As(
	                                Select x.ReportID, x.UserCode, LimitPerDay = ISNULL(d.LimitPerDay, x.LimitPerDay) , TimeLimit = ISNULL(d.TimeLimit,x.TimeLimit)
	                                From ReportAPISetupUserPermission x
	                                Left Join d on d.UserCode = x.UserCode and d.ReportID = x.ReportID
	                                Inner Join u on u.UserCode = x.UserCode
                                    Where x.ReportToken = @ReportToken
                                ), e as(
									Select x.UserCode,x.ReportID,x.TimeFrom,x.TimeTo
									From ReportAPITimeDuration x
									Left Join u on u.UserCode = x.UserCode
                                    Where @CurrentTime Between x.TimeFrom and x.TimeTo
								)
                                Select a.*, b.UserCode, b.LimitPerDay,TimeLimit,TotalReportCall = ISNULL(c.TotalReportCall,0),DATEADD(MINUTE, TimeLimit, Max(c.ReportCallingTime)) As ReportCallingTime,e.TimeFrom,e.TimeTo
                                From ReportAPISetup a
                                Inner Join b on b.ReportID = a.ReportID
                                Left Join c on c.ReportID = a.ReportID 
                                Left Join e on e.ReportID = a.ReportID
                                Where a.ReportName = @ReportName
								Group by a.ReportID,a.Reportname,a.[SQL],a.[Parameters],a.IsStoredProcedure,b.UserCode,b.LimitPerDay,TimeLimit,c.TotalReportCall,e.TimeFrom,e.TimeTo";

                var reportAPISetup = await _dbService.GetFirstOrDefaultAsync<ReportAPISetup>(query, new { @ReportName = reportName, @UserName = username, @ReportToken = token, @LocalIP = localIP, @CurrentTime = currentTime.ToString() });
                
                if (reportAPISetup == null) throw new Exception("API report is not found.");
                
                if (reportAPISetup.LimitPerday <= reportAPISetup.TotalReportCall) throw new Exception("API report data calling per day request limit exit.");
                
                if(DateTime.Now < reportAPISetup.ReportCallingTime ) throw new Exception("Your Report will be refresh after " + reportAPISetup.ReportCallingTime);
                
                if(reportAPISetup.TimeFrom == TimeSpan.Zero && reportAPISetup.TimeTo == TimeSpan.Zero) throw new Exception("You are not in this time slot");
                //if (currentTime < reportAPISetup.TimeFrom || currentTime > reportAPISetup.TimeTo) throw new Exception("You are not in this time slot");
                
                var paramNames = reportAPISetup.Parameters.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var paramValues = values.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var sql = reportAPISetup.SQL;

                

                if (paramNames.Length != paramValues.Length) return ("Error:Parameter count does not match parameter value count.");
                if (reportAPISetup.IsStoredProcedure)
                {
                    paramValues = paramValues.Select(x => $"'{x}'").ToArray();
                    sql += $" {string.Join(",", paramValues)}";
                }
                else
                {
                    for (int i = 0; i < paramNames.Length; i++)
                    {
                        sql = Regex.Replace(sql, $"@{paramNames[i]}", paramValues[i], RegexOptions.IgnoreCase);
                    }
                }
                ////Save Report calling History
                ReportAPIUserHistory objUH = new ReportAPIUserHistory();
                objUH.ReportID = reportAPISetup.ReportID;
                objUH.UserCode = reportAPISetup.UserCode;
                objUH.CallingStartTime =DateTime.Now;
                objUH.LocalIP = localIP;
                var records = await _dbService.GetDynamicDataAsync(sql);

                objUH.CallingEndTime = DateTime.Now;
                var savedEntity = await _dbServiceUH.SaveEntityCompositKeyAsync(objUH);
                return records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ReportAPISetup> GetByIdAsync(string Id)
        {
            string query = "Select * From ReportAPISetup Where ReportID=@ReportID";
            var item = await _dbService.GetFirstOrDefaultAsync<ReportAPISetup>(query, new { Id }) ??
                throw new ItemNotFoundException(ErrorKeys.NoRecord);

            return item;
        }

        public async Task<bool> UpdateAsync(long Id, ReportAPISetup reportAPISetup)
        {
            var savedEntity = await _dbService.SaveEntityAsync(reportAPISetup) ??
                throw new Exception(ErrorKeys.UnsuccesfullInsertUpdate);
            return true;
        }

    }
}
