using EPYTST.Application.Entities;



namespace EPYTST.Application.Interfaces
{
    public interface ILoginUserService : ICommonService<LoginUser>
    {
        Task<LoginUser> AddAsync(LoginUser item);
        Task<IEnumerable<LoginUser>> GetAllAsync();
    }
}
