using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.DataContext.Entities
{
    public class News:BaseEntity
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public string? ImagePath { get; set; }
    }
}
