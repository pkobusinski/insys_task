using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieLibrary.Core.Models;
using MovieLibrary.Core.Services;
using MovieLibrary.Data;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;
using System.Linq;

namespace MovieLibrary.Test.Unit.Services
{
    [TestClass]
    public class CategoryServiceTests
    {
        private MovieLibraryContext _context;
        private CategoryRepository _categoryRepository;
        private CategoryService _categoryService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MovieLibraryContext>()
                .UseInMemoryDatabase(databaseName: "TestCategoryLibraryDb")
                .Options;

            _context = new MovieLibraryContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _categoryRepository = new CategoryRepository(_context);
            _categoryService = new CategoryService(_categoryRepository);

            SeedTestData();
        }

        private void SeedTestData()
        {
            _context.Categories.AddRange(
                new Category { Name = "Action" },
                new Category { Name = "Comedy" }
            );
            _context.SaveChanges();
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllCategories()
        {
            var categories = _categoryService.GetAll().ToList();
            Assert.AreEqual(2, categories.Count);
        }

        [TestMethod]
        public void GetById_ShouldReturnCorrectCategory()
        {
            var categoryId = _context.Categories.First(c => c.Name == "Action").Id;
            var category = _categoryService.GetById(categoryId);

            Assert.IsNotNull(category);
            Assert.AreEqual("Action", category.Name);
        }

        [TestMethod]
        public void GetById_WithInvalidId_ShouldReturnNull()
        {
            var category = _categoryService.GetById(999);
            Assert.IsNull(category);
        }

        [TestMethod]
        public void Create_ShouldAddNewCategory()
        {
            var newCategory = new CategoryDto { Name = "Drama" };
            var created = _categoryService.Create(newCategory);

            Assert.IsNotNull(created);
            Assert.IsTrue(created.Id > 0);
            Assert.AreEqual("Drama", created.Name);

            var dbCategory = _context.Categories.FirstOrDefault(c => c.Name == "Drama");
            Assert.IsNotNull(dbCategory);
        }

        [TestMethod]
        public void Update_ShouldModifyExistingCategory()
        {
            var category = _context.Categories.First(c => c.Name == "Action");
            var updatedDto = new CategoryDto
            {
                Id = category.Id,
                Name = "Updated Action"
            };

            var updated = _categoryService.Update(updatedDto);

            Assert.IsNotNull(updated);
            Assert.AreEqual("Updated Action", updated.Name);

            var dbCategory = _context.Categories.Find(category.Id);
            Assert.AreEqual("Updated Action", dbCategory.Name);
        }

        [TestMethod]
        public void Update_WithInvalidId_ShouldReturnNull()
        {
            var invalidDto = new CategoryDto { Id = 999, Name = "Invalid" };
            var result = _categoryService.Update(invalidDto);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Delete_ShouldRemoveCategory()
        {
            var category = _context.Categories.First(c => c.Name == "Action");
            var initialCount = _context.Categories.Count();

            var result = _categoryService.Delete(category.Id);

            Assert.IsTrue(result);
            Assert.AreEqual(initialCount - 1, _context.Categories.Count());
            Assert.IsNull(_context.Categories.Find(category.Id));
        }

        [TestMethod]
        public void Delete_WithInvalidId_ShouldReturnFalse()
        {
            var result = _categoryService.Delete(999);
            Assert.IsFalse(result);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }
    }
}
