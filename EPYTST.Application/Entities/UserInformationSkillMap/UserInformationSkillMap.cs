using Dapper.Contrib.Extensions;
using EPYTST.Infrastructure.Data;
using EPYTST.Infrastructure.Static;


namespace EPYTST.Application.Entities
{
    [Table("UserInformationSkillMap")]
    public class UserInformationSkillMap : DapperBaseEntity
    {
        [ExplicitKey]
        public int UserInformationSkillMapId { get; set; }
       // public int LoginUserId { get; set; }
        public int SkillId { get; set; }
        public int SkillLevelId { get; set; }
        public int UserCode { get; set; }
        public string? CertificateName { get; set; }
        public string? CertificatePath { get; set; }
        public string? HandsOnExperience { get; set; }
        public DateTime? HandsOnExperienceFromDate { get; set; }
        public DateTime? HandsOnExperienceToDate { get; set; }
        public DateTime DateAdded { get; set; }
        public int AddedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsActive { get; set; }

        //#region Additional Properties
        [Write(false)]
        public override bool IsModified => EntityState == System.Data.Entity.EntityState.Modified || UserInformationSkillMapId != 0;


        //Extra
        [Write(false)]
        public string? SkillName { get; set; }
        [Write(false)]
        public string? SkillLevelName { get; set; }


        [Write(false)]
        public List<Skill> Skills { get; set; } = new List<Skill>();
        [Write(false)]
        public List<SkillLevel> SkillLevels { get; set; } = new List<SkillLevel>();

    }
}
