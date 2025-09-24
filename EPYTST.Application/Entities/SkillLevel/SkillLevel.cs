using Dapper.Contrib.Extensions;
using EPYTST.Infrastructure.Data;
using EPYTST.Infrastructure.Static;


namespace EPYTST.Application.Entities
{
    [Table("SkillLevel")]
    public class SkillLevel : DapperBaseEntity
    {
        [ExplicitKey]
        public int SkillLevelId { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public int AddedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsActive { get; set; }


        //#region Additional Properties
        [Write(false)]
        public string? AddedByName { get; set; }

        [Write(false)]
        public override bool IsModified => EntityState == System.Data.Entity.EntityState.Modified || SkillLevelId != 0;

    }
}
