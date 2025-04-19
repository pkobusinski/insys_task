using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MovieLibrary.Data.IRepositories;

namespace MovieLibrary.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly MovieLibraryContext _context;

        public Repository(MovieLibraryContext context)
        {
            _context = context;
        }
        public IEnumerable<T> GetAll() { 
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }


    }
}
