namespace EPYTST.API.Models
{
    public class LoginUserSkillMap
    {
        public int LoginUserSkillMapId { get; set; }
        public int LoginUserId { get; set; }
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

    }
}
