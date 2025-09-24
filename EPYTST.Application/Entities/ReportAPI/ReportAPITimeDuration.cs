using Dapper.Contrib.Extensions;
using EPYTST.Infrastructure.Data;
using EPYTST.Infrastructure.Static;


namespace EPYTST.Application.Entities
{
    [Table("ReportAPITimeDuration")]
    public class ReportAPITimeDuration : DapperBaseEntity
    {
        [ExplicitKey]
        public int UserCode { get; set; }
        public int ReportID { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }


        //// Assuming there is another table "User" with "UserId" as its primary key
        //[System.ComponentModel.DataAnnotations.Schema.ForeignKey("User")] // This indicates that UserId is a foreign key referring to the User table
        //public int UserId { get; set; }

        #region Additional Properties
        [Write(false)]
        public override bool IsModified => EntityState == System.Data.Entity.EntityState.Modified || UserCode != 0;

        #endregion Additional Properties

    }
}
