namespace EPYTST.API.Models
{
    public class LoginUser
    {
        public int UserCode { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; }


        //Extra for profile
        public string? PhoneNumber { get; set; }
        public string? Designation { get; set; }
        public string? DepartmentName { get; set; }
        public string? ProfileImagePath { get; set; }

    }
}
