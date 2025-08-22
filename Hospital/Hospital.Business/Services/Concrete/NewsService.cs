using Hospital.Business.Services.Abstract;
using Hospital.DAL.DataContext;
using Hospital.DAL.DataContext.Entities;
using Hospital.DAL.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Business.Services.Concrete
{
    public class NewsService:GenericService<News> ,INewsService
    {
        public NewsService(IGenericRepository<News> newsRepository , AppDbContext dbContext)
            :base(newsRepository ,dbContext)
        { }
    }
}
