using System;
using System.Collections.Generic;
using System.Text;
using MovieLibrary.Core.Interfaces;
using MovieLibrary.Core.Models;
using MovieLibrary.Data.IRepositories;
using MovieLibrary.Data.Entities;
using System.Linq;

namespace MovieLibrary.Core.Services
{
    public class CategoryService : ICategoryService
    {
        protected readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

        }
        IEnumerable<CategoryDto> IService<CategoryDto>.GetAll()
        {
            var categories = _categoryRepository.GetAll();

            return categories.Select(category => new CategoryDto { 
                Id = category.Id,
                Name = category.Name
            });
        }
        CategoryDto IService<CategoryDto>.GetById(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
                return null;
            else return new CategoryDto { 
                Id = category.Id,
                Name = category.Name};
        }

        void IService<CategoryDto>.Create(CategoryDto entity)
        {
            var category = new Category { 
                Name = entity.Name};

            _categoryRepository.Add(category);
            entity.Id = category.Id;
        }
        void IService<CategoryDto>.Update(CategoryDto entity)
        {
            var category = _categoryRepository.GetById(entity.Id);

            category.Id = entity.Id;
            _categoryRepository.Update(category);
        }
        public void Delete(int id)
        {
            var category = _categoryRepository.GetById(id);
            _categoryRepository.Delete(category.Id);
        }
    }
}
