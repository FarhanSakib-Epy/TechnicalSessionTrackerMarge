using EPYTST.Application.Entities;



namespace EPYTST.Application.Interfaces
{
    public interface ISkillLevelService : ICommonService<SkillLevel>
    {
        Task<SkillLevel> AddAsync(SkillLevel item);
        Task<IEnumerable<SkillLevel>> GetAllAsync();
        Task<List<UserInformationSkillMap>> GetByUserCode(int userCode);
        Task<List<UserBySkillVM>> GetBySkillId(int skillId);
    }
}
