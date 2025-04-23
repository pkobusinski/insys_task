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
    public class MovieManagementControllerTest
    {
        private Mock<IMovieService> _movieServiceMock;
        private MovieManagementController _controller;

        [TestInitialize]
        public void Setup()
        {
            _movieServiceMock = new Mock<IMovieService>();
            _controller = new MovieManagementController(_movieServiceMock.Object);
        }

        [TestMethod]
        public void GetAllMovies_ShouldReturnOkWithMovies()
        {
            var mockMovies = new List<MovieDto>
            {
                new MovieDto { Id = 1, Title = "Test Movie 1" },
                new MovieDto { Id = 2, Title = "Test Movie 2" }
            };
            _movieServiceMock.Setup(s => s.GetAll()).Returns(mockMovies);

            var result = _controller.GetAllMovies();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedMovies = okResult.Value as IEnumerable<MovieDto>;
            Assert.AreEqual(2, returnedMovies.Count());
        }

        [TestMethod]
        public void GetMovieById_ExistingId_ReturnsOk()
        {
            var movie = new MovieDto { Id = 1, Title = "Test Movie" };
            _movieServiceMock.Setup(s => s.GetById(1)).Returns(movie);

            var result = _controller.GetMovieById(1);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(movie, okResult.Value);
        }

        [TestMethod]
        public void GetMovieById_InvalidId_ReturnsNotFound()
        {
            _movieServiceMock.Setup(s => s.GetById(999)).Returns((MovieDto)null);

            var result = _controller.GetMovieById(999);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void CreateMovie_ValidInput_ReturnsOk()
        {
            var movie = new MovieDto { Title = "New Movie" };
            _movieServiceMock.Setup(s => s.Create(It.IsAny<MovieDto>())).Returns(movie);

            var result = _controller.CreateMovie(movie);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(movie, okResult.Value);
        }

        [TestMethod]
        public void CreateMovie_NullInput_ReturnsBadRequest()
        {
            var result = _controller.CreateMovie(null);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateMovie_Valid_ReturnsOk()
        {
            var movie = new MovieDto { Id = 1, Title = "Updated Movie" };
            _movieServiceMock.Setup(s => s.Update(movie)).Returns(movie);

            var result = _controller.UpdateMovie(1, movie);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(movie, okResult.Value);
        }

        [TestMethod]
        public void UpdateMovie_IdMismatch_ReturnsBadRequest()
        {
            var movie = new MovieDto { Id = 1, Title = "Test" };

            var result = _controller.UpdateMovie(2, movie);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateMovie_NotFound_ReturnsNotFound()
        {
            var movie = new MovieDto { Id = 999, Title = "Invalid" };
            _movieServiceMock.Setup(s => s.Update(movie)).Returns((MovieDto)null);

            var result = _controller.UpdateMovie(999, movie);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteMovie_ValidId_ReturnsOk()
        {
            var movie = new MovieDto { Id = 1, Title = "To Delete" };
            _movieServiceMock.Setup(s => s.GetById(1)).Returns(movie);
            _movieServiceMock.Setup(s => s.Delete(1)).Returns(true);

            var result = _controller.DeleteMovie(1);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(movie, okResult.Value);
        }

        [TestMethod]
        public void DeleteMovie_InvalidId_ReturnsNotFound()
        {
            _movieServiceMock.Setup(s => s.Delete(999)).Returns(false);

            var result = _controller.DeleteMovie(999);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void FilterMovies_WithMatches_ReturnsOk()
        {
            var filtered = new List<MovieDto>
            {
                new MovieDto { Id = 1, Title = "Filtered Movie" }
            };
            _movieServiceMock.Setup(s => s.FilterMovies("filter", null, null, null, 1, 10)).Returns(filtered);

            var result = _controller.FilterMovies("filter");

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(filtered, okResult.Value);
        }
    }
}

