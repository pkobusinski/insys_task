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
        T Create(T entity);
        T Update(T entity);
        bool Delete(int id);
    }
}
