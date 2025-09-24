using EPYTST.Application.Entities;



namespace EPYTST.Application.Interfaces
{
    public interface IDepartmentService : ICommonService<Department>
    {
        Task<Department> AddAsync(Department item);
        Task<IEnumerable<Department>> GetAllAsync();
        //Task<Department> GetByIdAsync(string Id);
    }
}
