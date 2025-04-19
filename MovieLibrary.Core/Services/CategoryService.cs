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

        public IEnumerable<CategoryDto> GetAll() 
        {
            var categories = _categoryRepository.GetAll();
            return categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            });
        }

        public CategoryDto GetById(int id) 
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
                return null;
            else
                return new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                };
        }

        public CategoryDto Create(CategoryDto entity) 
        {
            var category = new Category
            {
                Name = entity.Name
            };
            _categoryRepository.Add(category);
            entity.Id = category.Id;
            return entity;
        }

        public CategoryDto Update(CategoryDto entity) 
        {
            var category = _categoryRepository.GetById(entity.Id);
            if (category == null)
                return null;
            category.Name = entity.Name;
            _categoryRepository.Update(category);
            return entity;
        }

        public bool Delete(int id) 
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
                return false;
            _categoryRepository.Delete(category.Id);
            return true;
        }
    }
}