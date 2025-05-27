using Hanodale.SyncService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Hanodale.SyncService
{
    public class GenericRepository<T> : ISyncRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        // Implement IRepository methods
    }
}