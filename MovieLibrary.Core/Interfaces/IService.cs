using MovieLibrary.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.Core.Interfaces
{
    public interface IService<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
