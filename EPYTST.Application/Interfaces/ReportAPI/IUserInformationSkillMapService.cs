using EPYTST.Application.Entities;
using EPYTST.Application.Entities.UserInformation;



namespace EPYTST.Application.Interfaces
{
    public interface IUserInformationSkillMapService : ICommonService<UserInformationSkillMap>
    {
        Task<UserInformationSkillMap> AddAsync(UserInformationSkillMap item);
        Task<IEnumerable<UserInformationSkillMap>> GetAllAsync();
        Task<UserInformationSkillMap> GetUserInformationSkillMapByIdAsync(string Id);
        Task<UserInformation> GetUserByUserNameAsync(int userName);
        Task<bool> UpdateUserInformationSkillMapAsync(int Id, UserInformationSkillMap item);
    }
}
