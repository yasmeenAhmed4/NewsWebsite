using NewsWebsite.BL.DTO;
using NewsWebsite.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BL.Manager
{
    public interface IAuthorManager
    {
        IEnumerable<AuthorDTO> GetAll();
        AuthorDTO GetById(int id);
        void Insert(AuthorDTO obj);
        void Update(AuthorDTO obj);
        void Delete(int id);
        void Save();
    }
}
