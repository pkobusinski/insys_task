using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.Data.IRepositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add (T entity);
        void Update (T entity);
        void Delete (int id);

    }
}
