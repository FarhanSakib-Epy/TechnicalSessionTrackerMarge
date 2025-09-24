using Dapper.Contrib.Extensions;
using EPYTST.Infrastructure.Data;
using EPYTST.Infrastructure.Static;


namespace EPYTST.Application.Entities
{
    public class UserIntormetionVM
    {
        public int UserCode { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public int CompanyID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }

    }
}
