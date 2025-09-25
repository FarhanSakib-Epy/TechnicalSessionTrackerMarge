namespace EPYTST.API.Models
{
    public class SkillUploadViewModel
    {
        public int SkillId { get; set; }
        public int SkillLevelId { get; set; }
        public string? CertificateName { get; set; }
        public IFormFile? CertificateFile { get; set; }
        public string? HandsOnExperience { get; set; }
        public DateTime? HandsOnExperienceFromDate { get; set; }
        public DateTime? HandsOnExperienceToDate { get; set; }
        public int UserCode { get; set; }

        public int? UserInformationSkillMapId { get; set; }
        

    }
}
