using NewsWebsite.BL.DTO;
using NewsWebsite.DAL.Data.Models;
using NewsWebsite.DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NewsWebsite.BL.Manager
{
    public class AuthorManager : IAuthorManager
    {
        private IGenericRepository<Author> IR { get; }

        public AuthorManager(IGenericRepository<Author> _IR)
        {
            IR = _IR;
        }

        public IEnumerable<AuthorDTO> GetAll()
        {
            var ins = IR.GetAll();
            List<AuthorDTO> authorDTOs = new List<AuthorDTO>();
            foreach (var i in ins)
            {
                AuthorDTO dTO = new AuthorDTO()
                {
                    Id = i.Id,
                    Name = i.Name,
                };
                authorDTOs.Add(dTO);
            }
            return authorDTOs;
        }

        public AuthorDTO GetById(int id)
        {
            var o = IR.GetById(id);
            AuthorDTO c = new AuthorDTO();
            c.Id = o.Id;
            c.Name = o.Name;
            return c;
        }

        public void Insert(AuthorDTO obj)
        {
            Author a = new Author();
            a.Name = obj.Name;
            IR.Insert(a);
        }

        public void Update(AuthorDTO obj)
        {
            var a = IR.GetById(obj.Id);
            if (a != null)
            {
                a.Name = obj.Name;
                IR.Update(a);
            }

        }

        public void Delete(int id)
        {
            IR.Delete(id);
        }

        public void Save()
        {
            IR.Save();
        }
    }
}
