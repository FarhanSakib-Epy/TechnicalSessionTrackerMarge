using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPYTST.Application.Entities.UserInformation
{
    [Table("UserInformation")]
    public class UserInformation
    {
        [ExplicitKey]
        public int UserInformationId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Designation { get; set; }

        public string DepartmentName { get; set; }

        public int HumanSpiritNo { get; set; }

        public string ProfileImagePath { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime DateAdded { get; set; }
        public int AddedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsActive { get; set; }


    }
}
