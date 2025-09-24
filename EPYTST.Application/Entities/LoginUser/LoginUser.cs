using Dapper.Contrib.Extensions;
using EPYTST.Infrastructure.Data;
using EPYTST.Infrastructure.Static;


namespace EPYTST.Application.Entities
{
    [Table("LoginUser")]
    public class LoginUser : DapperBaseEntity
    {
        [ExplicitKey]
        public int UserCode { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; }
        
        //#region Additional Properties
        [Write(false)]
        public override bool IsModified => EntityState == System.Data.Entity.EntityState.Modified || UserCode != 0;

    }
}
