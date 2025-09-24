using Dapper.Contrib.Extensions;
using EPYTST.Infrastructure.Data;
using EPYTST.Infrastructure.Static;


namespace EPYTST.Application.Entities
{
    [Table("ReportAPISetup")]
    public class ReportAPISetup : DapperBaseEntity
    {
        [ExplicitKey]
        public int ReportID { get; set; }
        public string ReportName { get; set; }
        public string SQL { get; set; }
        public string Parameters { get; set; }
        public bool IsStoredProcedure { get; set; }


        //// Assuming there is another table "User" with "UserId" as its primary key
        //[System.ComponentModel.DataAnnotations.Schema.ForeignKey("User")] // This indicates that UserId is a foreign key referring to the User table
        //public int UserId { get; set; }

        #region Additional Properties
        [Write(false)]
        public override bool IsModified => EntityState == System.Data.Entity.EntityState.Modified || ReportName != "";
        [Write(false)]
        public int TotalReportCall { get; set; }
        [Write(false)]
        public int UserCode { get; set; }
        [Write(false)]
        public int LimitPerday { get; set; }

        [Write(false)]
        public int TimeLimit { get; set; }
        [Write(false)]
        public DateTime ReportCallingTime { get; set; }
        [Write(false)]
        public TimeSpan TimeFrom { get; set; }
        [Write(false)]
        public TimeSpan TimeTo { get; set; }
        #endregion Additional Properties

    }
}
