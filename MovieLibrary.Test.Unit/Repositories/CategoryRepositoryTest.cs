using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;
using MovieLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieLibrary.Test.Unit.Repositories
{
    [TestClass]
    public class CategoryRepositoryTest
    {
        private MovieLibraryContext _context;
        private CategoryRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<MovieLibraryContext>()
                .UseInMemoryDatabase(databaseName: $"MovieLibraryTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new MovieLibraryContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repository = new CategoryRepository(_context);

            SeedTestData();
        }

        private void SeedTestData()
        {
            var categories = new List<Category>
            {
                new Category { Name = "Action" },
                new Category { Name = "Drama" }
            };

            _context.Categories.AddRange(categories);
            _context.SaveChanges();
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllCategories()
        {
            var result = _repository.GetAll().ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(c => c.Name == "Action"));
            Assert.IsTrue(result.Any(c => c.Name == "Drama"));
        }

        [TestMethod]
        public void GetById_ShouldReturnCategoryById()
        {
            var firstCategoryId = _context.Categories.First(c => c.Name == "Action").Id;

            var category = _repository.GetById(firstCategoryId);

            Assert.IsNotNull(category);
            Assert.AreEqual("Action", category.Name);
        }

        [TestMethod]
        public void Add_ShouldAddCategory()
        {
            var newCategory = new Category { Name = "Comedy" };

            _repository.Add(newCategory);
            var addedCategory = _repository.GetById(newCategory.Id);
            var allCategories = _repository.GetAll().ToList();

            Assert.IsNotNull(addedCategory);
            Assert.AreEqual("Comedy", addedCategory.Name);
            Assert.AreEqual(3, allCategories.Count);

            var dbCategory = _context.Categories.FirstOrDefault(c => c.Name == "Comedy");
            Assert.IsNotNull(dbCategory);
        }

        [TestMethod]
        public void Update_ShouldUpdateCategory()
        {
            var categoryToUpdate = _context.Categories.First(c => c.Name == "Action");
            categoryToUpdate.Name = "Updated Action";

            _repository.Update(categoryToUpdate);
            var updatedCategory = _repository.GetById(categoryToUpdate.Id);

            Assert.IsNotNull(updatedCategory);
            Assert.AreEqual("Updated Action", updatedCategory.Name);

            var dbCategory = _context.Categories.Find(categoryToUpdate.Id);
            Assert.AreEqual("Updated Action", dbCategory.Name);
        }

        [TestMethod]
        public void Delete_ShouldRemoveCategory()
        {
            var categoryToDelete = _context.Categories.First(c => c.Name == "Drama");
            var initialCount = _context.Categories.Count();

            _repository.Delete(categoryToDelete.Id);
            var remainingCategories = _repository.GetAll().ToList();

            Assert.AreEqual(initialCount - 1, remainingCategories.Count);
            Assert.IsFalse(remainingCategories.Any(c => c.Name == "Drama"));

            var dbCategory = _context.Categories.Find(categoryToDelete.Id);
            Assert.IsNull(dbCategory);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
