using Hospital.Business.Services.Abstract;
using Hospital.DAL.DataContext;
using Hospital.DAL.DataContext.Entities;
using Hospital.DAL.Repositories.Abstract;
using Hospital.DAL.Repositories.Concret;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Business.Services.Concrete
{
    public class DoctorService : GenericService<Doctor>, IDoctorService
    {
        public DoctorService(IGenericRepository<Doctor> repository, AppDbContext dbContext)
            : base(repository, dbContext)
        {
        }
    }
}
