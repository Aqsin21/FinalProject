using Hospital.DAL.DataContext;
using Hospital.DAL.DataContext.Entities;
using Hospital.DAL.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.Repositories.Concret
{
    public class DoctorRepository:GenericRepository<Doctor> ,IDoctorRepository
    {
        public DoctorRepository(AppDbContext context) : base(context) { }
    }
}
