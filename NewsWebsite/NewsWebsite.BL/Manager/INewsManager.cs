using NewsWebsite.BL.DTO;
using NewsWebsite.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BL.Manager
{
    public interface INewsManager
    {
        IEnumerable<NewsDTO> GetAll();
        NewsDTO GetById(int id);
        void Insert(NewsDTO obj);
        void Update(NewsDTO obj);
        void Delete(int id);
        void Save();
    }
}
