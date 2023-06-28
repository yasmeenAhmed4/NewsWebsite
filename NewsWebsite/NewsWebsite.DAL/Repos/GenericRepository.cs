using Microsoft.EntityFrameworkCore;
using NewsWebsite.DAL.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.DAL.Repos
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private ApplicationDBContext _context;

        private DbSet<T> table = null;

        public GenericRepository(ApplicationDBContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }
        public void Delete(int id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T GetById(int id)
        {
            return table.Find(id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
    }
}
