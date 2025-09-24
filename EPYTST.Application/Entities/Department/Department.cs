using Dapper.Contrib.Extensions;
using EPYTST.Infrastructure.Data;
using EPYTST.Infrastructure.Static;


namespace EPYTST.Application.Entities
{
    [Table("Department")]
    public class Department : DapperBaseEntity
    {
        [ExplicitKey]
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public DateTime DateAdded { get; set; }
        public int AddedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsActive { get; set; }



        //#region Additional Properties
        [Write(false)]
        public override bool IsModified => EntityState == System.Data.Entity.EntityState.Modified || this.DepartmentId > 0;
        //[Write(false)]
        //public int TotalReportCall { get; set; }
        //[Write(false)]
        //public int UserCode { get; set; }
        //[Write(false)]
        //public int LimitPerday { get; set; }

        //[Write(false)]
        //public int TimeLimit { get; set; }
        //[Write(false)]
        //public DateTime ReportCallingTime { get; set; }
        //[Write(false)]
        //public TimeSpan TimeFrom { get; set; }
        //[Write(false)]
        //public TimeSpan TimeTo { get; set; }
        //#endregion Additional Properties

    }
}
