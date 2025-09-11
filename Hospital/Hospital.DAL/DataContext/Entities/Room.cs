using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.DataContext.Entities
{
    public class Room : BaseEntity
    {
        public required int RoomNumber { get; set; }
        public required string Type { get; set; } 
        public bool IsAvailable { get; set; }
        public  string? ImagePath { get; set; }
    }
}
