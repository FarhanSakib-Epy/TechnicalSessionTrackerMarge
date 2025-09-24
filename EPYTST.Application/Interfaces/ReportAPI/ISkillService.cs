using EPYTST.Application.Entities;



namespace EPYTST.Application.Interfaces
{
    public interface ISkillService : ICommonService<Skill>
    {
        Task<Skill> AddAsync(Skill item);
        Task<IEnumerable<Skill>> GetAllAsync();
    }
}
