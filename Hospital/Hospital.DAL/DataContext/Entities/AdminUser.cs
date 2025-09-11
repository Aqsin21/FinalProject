using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.DataContext.Entities
{
    public enum AdminRole
    {
        Admin,
        SuperAdmin
    }
    public class AdminUser:BaseEntity
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public AdminRole Role { get; set; }
    }
}
