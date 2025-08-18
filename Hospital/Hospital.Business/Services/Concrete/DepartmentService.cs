using Hospital.Business.Services.Abstract;
using Hospital.DAL.DataContext.Entities;
using Hospital.DAL.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Business.Services.Concrete
{
    public class DepartmentService : GenericService<Department>, IDepartmentService
    {
        public DepartmentService(IDepartmentRepository departmentRepository)
            : base(departmentRepository) { }
    }
}
