using EPYTST.Infrastructure.Data;
using Dapper.Contrib.Extensions;

namespace EPYTST.Application.Entities.ReportAPI
{
    [Table("ReportAPISetupUserPermission")]
    public class ReportAPISetupUserPermission : DapperBaseEntity
    {
        [ExplicitKey]
        public int ReportID { get; set; }
        [ExplicitKey]
        public int UserCode { get; set; }
        public string ReportToken { get; set; }
        public int LimitPerday { get; set; }
        public int TimeLimit { get; set; }
        #region Additional Properties
        [Write(false)]
        public override bool IsModified => EntityState == System.Data.Entity.EntityState.Modified || UserCode == 0;
        #endregion Additional Properties
    }
}
