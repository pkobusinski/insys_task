using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieLibrary.Api.Controllers;
using MovieLibrary.Core.Interfaces;
using MovieLibrary.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace MovieLibrary.Test.Unit.Controllers
{
    [TestClass]
    public class CategoryManagementControllerTest
    {
        private Mock<ICategoryService> _categoryServiceMock;
        private CategoryManagementController _controller;

        [TestInitialize]
        public void Setup()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _controller = new CategoryManagementController(_categoryServiceMock.Object);
        }

        [TestMethod]
        public void GetAllCategories_ShouldReturnOkWithCategories()
        {
            var mockCategories = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Action" },
                new CategoryDto { Id = 2, Name = "Drama" }
            };
            _categoryServiceMock.Setup(s => s.GetAll()).Returns(mockCategories);

            var result = _controller.GetAllCategories();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedCategories = okResult.Value as IEnumerable<CategoryDto>;
            Assert.AreEqual(2, returnedCategories.Count());
        }

        [TestMethod]
        public void GetCategory_ExistingId_ReturnsOk()
        {
            var category = new CategoryDto { Id = 1, Name = "Action" };
            _categoryServiceMock.Setup(s => s.GetById(1)).Returns(category);

            var result = _controller.GetCategory(1);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(category, okResult.Value);
        }

        [TestMethod]
        public void GetCategory_InvalidId_ReturnsNotFound()
        {
            _categoryServiceMock.Setup(s => s.GetById(999)).Returns((CategoryDto)null);

            var result = _controller.GetCategory(999);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void CreateCategory_ValidInput_ReturnsOk()
        {
            var category = new CategoryDto { Name = "Comedy" };
            _categoryServiceMock.Setup(s => s.Create(It.IsAny<CategoryDto>())).Returns(category);

            var result = _controller.CreateCategory(category);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(category, okResult.Value);
        }

        [TestMethod]
        public void CreateCategory_NullInput_ReturnsBadRequest()
        {
            var result = _controller.CreateCategory(null);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateCategory_Valid_ReturnsOk()
        {
            var category = new CategoryDto { Id = 1, Name = "Updated Action" };
            _categoryServiceMock.Setup(s => s.Update(category)).Returns(category);

            var result = _controller.UpdateCategory(1, category);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(category, okResult.Value);
        }

        [TestMethod]
        public void UpdateCategory_IdMismatch_ReturnsBadRequest()
        {
            var category = new CategoryDto { Id = 1, Name = "Test Category" };

            var result = _controller.UpdateCategory(2, category);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateCategory_NotFound_ReturnsNotFound()
        {
            var category = new CategoryDto { Id = 999, Name = "Invalid Category" };
            _categoryServiceMock.Setup(s => s.Update(category)).Returns((CategoryDto)null);

            var result = _controller.UpdateCategory(999, category);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteCategory_ValidId_ReturnsOk()
        {
            var category = new CategoryDto { Id = 1, Name = "To Delete" };
            _categoryServiceMock.Setup(s => s.GetById(1)).Returns(category);
            _categoryServiceMock.Setup(s => s.Delete(1)).Returns(true);

            var result = _controller.DeleteCategory(1);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(category, okResult.Value);
        }

        [TestMethod]
        public void DeleteCategory_InvalidId_ReturnsNotFound()
        {
            _categoryServiceMock.Setup(s => s.Delete(999)).Returns(false);

            var result = _controller.DeleteCategory(999);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
