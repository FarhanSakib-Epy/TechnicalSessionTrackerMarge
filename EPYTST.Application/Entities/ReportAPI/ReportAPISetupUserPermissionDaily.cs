using Dapper.Contrib.Extensions;
using EPYTST.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPYTST.Application.Entities.ReportAPI
{
    [Table("ReportAPISetupUserPermission")]
    public class ReportAPISetupUserPermissionDaily : DapperBaseEntity
    {
        [ExplicitKey]
        public int ReportID { get; set; }
        [ExplicitKey]
        public int UserCode { get; set; }
        [ExplicitKey]
        public DateTime UseDate { get; set; }
        public int LimitPerday { get; set; }
        public int TimeLimit { get; set; }
        #region Additional Properties
        [Write(false)]
        public override bool IsModified => EntityState == System.Data.Entity.EntityState.Modified || UserCode == 0;
        #endregion Additional Properties
    }
}
