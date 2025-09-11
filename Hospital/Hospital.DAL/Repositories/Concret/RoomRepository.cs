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
    public class RoomRepository:GenericRepository<Room> ,IRoomRepository
    {
        public RoomRepository(AppDbContext context) : base(context) { }
    }
}
