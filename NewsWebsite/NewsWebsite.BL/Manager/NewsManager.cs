using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NewsWebsite.BL.DTO;
using NewsWebsite.DAL.Data.Models;
using NewsWebsite.DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NewsWebsite.BL.Manager
{
    public class NewsManager : INewsManager
    {
        private IGenericRepository<News> IR { get; }
        private IWebHostEnvironment env;
        private IHttpContextAccessor httpContextAccessor;
        public NewsManager(IGenericRepository<News> _IR, IWebHostEnvironment _env, IHttpContextAccessor _httpContextAccessor)
        {
            IR = _IR;
            env = _env;
            httpContextAccessor = _httpContextAccessor;
        }

        public void Delete(int id)
        {
            IR.Delete(id);
        }

        public IEnumerable<NewsDTO> GetAll()
        {
            var ins = IR.GetAll();
            List<NewsDTO> newsDTOs = new List<NewsDTO>();
            foreach (var i in ins)
            {
                NewsDTO dTO = new NewsDTO()
                {
                    Id = i.Id,
                    Title = i.Title,
                    newsDescription = i.newsDescription,
                    //imageURL = i.image,
                    image = i.image,
                    publicationDate = i.publicationDate,
                    creationDate = i.creationDate,
                    AuthorId = i.AuthorId,
                };
                newsDTOs.Add(dTO);
            }
            return newsDTOs;
        }

        public NewsDTO GetById(int id)
        {
            var o = IR.GetById(id);
            NewsDTO c = new NewsDTO();
            c.Id = o.Id;
            c.Title = o.Title;

            //c.imageURL = o.image;
            c.image = o.image;
            c.newsDescription = o.newsDescription;
            c.publicationDate = o.publicationDate;
            c.creationDate = o.creationDate;
            c.AuthorId = o.AuthorId;
            return c;
        }

        public void Insert(NewsDTO obj)
        {
            News a = new News();
            a.Title = obj.Title;
            a.newsDescription = obj.newsDescription;
            //a.image = AddImage(obj.image);
            a.image = obj.image;
            a.publicationDate = obj.publicationDate;
            //a.creationDate = obj.creationDate;
            a.AuthorId = obj.AuthorId;
            IR.Insert(a);
        }

        public void Save()
        {
            IR.Save();
        }

        public void Update(NewsDTO obj)
        {
            var n = IR.GetById(obj.Id);
            if (n != null)
            {
                n.Title = obj.Title;
                n.newsDescription = obj.newsDescription;

                //n.image = AddImage(obj.image);
                n.image = obj.image;
                n.publicationDate = obj.publicationDate;
                //n.creationDate = obj.creationDate;
                n.AuthorId = obj.AuthorId;
                IR.Update(n);
            }
        }


        //public string AddImage(IFormFile file)
        //{

        //    var path = Path.Combine(env.WebRootPath, "uploads", file.FileName);

        //    using (var fileStream = new FileStream(path, FileMode.Create))
        //    {
        //        file.CopyTo(fileStream);

        //    }
        //    var baseURL = httpContextAccessor.HttpContext.Request.Scheme + "://" +
        //        httpContextAccessor.HttpContext.Request.Host +
        //        httpContextAccessor.HttpContext.Request.PathBase;

        //    var im = baseURL + "/uploads/" + file.FileName;

        //    return im;

        //}
    }
            
}
