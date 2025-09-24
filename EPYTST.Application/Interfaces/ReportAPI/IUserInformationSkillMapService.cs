using EPYTST.Application.Entities;



namespace EPYTST.Application.Interfaces
{
    public interface IUserInformationSkillMapService : ICommonService<UserInformationSkillMap>
    {
        Task<UserInformationSkillMap> AddAsync(UserInformationSkillMap item);
        Task<IEnumerable<UserInformationSkillMap>> GetAllAsync();
    }
}
