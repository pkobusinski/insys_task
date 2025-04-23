using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieLibrary.Core.Models;
using MovieLibrary.Core.Services;
using MovieLibrary.Data;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MovieLibrary.Test.Unit.Services
{
    [TestClass]
    public class MovieServiceTests
    {
        private MovieLibraryContext _context;
        private MovieRepository _movieRepository;
        private MovieService _movieService;

        [TestInitialize]
        public void Setup()
        {

            var options = new DbContextOptionsBuilder<MovieLibraryContext>()
                .UseInMemoryDatabase(databaseName: "TestMovieLibraryDb")
                .Options;

            _context = new MovieLibraryContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _movieRepository = new MovieRepository(_context);
            _movieService = new MovieService(_movieRepository);


            SeedTestData();
        }

        private void SeedTestData()
        {
            var action = new Category { Name = "Action" };
            var comedy = new Category { Name = "Comedy" };
            _context.Categories.AddRange(action, comedy);
            _context.SaveChanges();

            var movie1 = new Movie
            {
                Title = "Action Movie",
                Description = "An action movie description",
                Year = 2020,
                ImdbRating = 7.5m
            };

            var movie2 = new Movie
            {
                Title = "Comedy Movie",
                Description = "A comedy movie description",
                Year = 2021,
                ImdbRating = 6.5m
            };

            _context.Movies.AddRange(movie1, movie2);
            _context.SaveChanges();

            _context.MovieCategories.AddRange(
                new MovieCategory { MovieId = movie1.Id, CategoryId = action.Id },
                new MovieCategory { MovieId = movie2.Id, CategoryId = comedy.Id }
            );
            _context.SaveChanges();
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllMovies()
        {

            var movies = _movieService.GetAll().ToList();

            Assert.AreEqual(2, movies.Count);
        }

        [TestMethod]
        public void GetById_ShouldReturnCorrectMovie()
        {

            var firstMovieId = _context.Movies.First(m => m.Title == "Action Movie").Id;

            var movie = _movieService.GetById(firstMovieId);

            Assert.IsNotNull(movie);
            Assert.AreEqual("Action Movie", movie.Title);
            Assert.AreEqual(7.5m, movie.ImdbRating);
            Assert.AreEqual(1, movie.Categories.Count);
            Assert.AreEqual("Action", movie.Categories.First().Name);
        }

        [TestMethod]
        public void GetById_WithInvalidId_ShouldReturnNull()
        {
            var movie = _movieService.GetById(999);
            Assert.IsNull(movie);
        }

        [TestMethod]
        public void Create_ShouldAddNewMovie()
        {
            var newMovie = new MovieDto
            {
                Title = "New Movie",
                Description = "Test Description",
                Year = 2023,
                ImdbRating = 8.0m
            };

            var result = _movieService.Create(newMovie);
            var allMovies = _movieService.GetAll().ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, allMovies.Count);
            Assert.AreEqual("New Movie", result.Title);
            Assert.IsTrue(result.Id > 0);

            var dbMovie = _context.Movies.FirstOrDefault(m => m.Title == "New Movie");
            Assert.IsNotNull(dbMovie);
            Assert.AreEqual(8.0m, dbMovie.ImdbRating);
        }

        [TestMethod]
        public void Update_ShouldModifyExistingMovie()
        {

            var movieToUpdate = _context.Movies.First(m => m.Title == "Action Movie");
            var movieDto = new MovieDto
            {
                Id = movieToUpdate.Id,
                Title = "Updated Action Movie",
                Description = "Updated description",
                Year = 2020,
                ImdbRating = 8.0m
            };


            var result = _movieService.Update(movieDto);
            var updatedMovie = _movieService.GetById(movieToUpdate.Id);


            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Action Movie", result.Title);
            Assert.AreEqual("Updated Action Movie", updatedMovie.Title);
            Assert.AreEqual(8.0m, updatedMovie.ImdbRating);

            var dbMovie = _context.Movies.Find(movieToUpdate.Id);
            Assert.AreEqual("Updated Action Movie", dbMovie.Title);
        }

        [TestMethod]
        public void Update_WithInvalidId_ShouldReturnNull()
        {

            var invalidMovie = new MovieDto
            {
                Id = 999,
                Title = "Invalid Movie"
            };

            var result = _movieService.Update(invalidMovie);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Delete_ShouldRemoveMovie()
        {

            var movieToDelete = _context.Movies.First(m => m.Title == "Action Movie");
            var initialCount = _context.Movies.Count();

            var result = _movieService.Delete(movieToDelete.Id);
            var remainingMovies = _movieService.GetAll().ToList();

            Assert.IsTrue(result);
            Assert.AreEqual(initialCount - 1, remainingMovies.Count);
            Assert.IsFalse(remainingMovies.Any(m => m.Title == "Action Movie"));

            var dbMovie = _context.Movies.Find(movieToDelete.Id);
            Assert.IsNull(dbMovie);
        }

        [TestMethod]
        public void Delete_WithInvalidId_ShouldReturnFalse()
        {

            var result = _movieService.Delete(999);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FilterMovies_ShouldReturnOnlyMatchingCategory()
        {

            var actionCategoryId = _context.Categories.First(c => c.Name == "Action").Id;
            var result = _movieService.FilterMovies(categoryIds: new List<int> { actionCategoryId }).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Action Movie", result.First().Title);
        }

        [TestMethod]
        public void FilterMovies_WithTextFilter_ShouldReturnMatchingMovies()
        {

            var result = _movieService.FilterMovies(text: "Comedy").ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Comedy Movie", result.First().Title);
        }

        [TestMethod]
        public void FilterMovies_WithImdbRatingRange_ShouldReturnMatchingMovies()
        {

            var result = _movieService.FilterMovies(minImdb: 7.0m, maxImdb: 8.0m).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Action Movie", result.First().Title);
        }

        [TestMethod]
        public void FilterMovies_WithPagination_ShouldReturnCorrectPage()
        {

            for (int i = 3; i <= 15; i++)
            {
                _context.Movies.Add(new Movie
                {
                    Title = $"Movie {i}",
                    Description = $"Description {i}",
                    Year = 2020,
                    ImdbRating = 6.0m
                });
            }
            _context.SaveChanges();

            var page1 = _movieService.FilterMovies(page: 1, pageSize: 5).ToList();
            var page2 = _movieService.FilterMovies(page: 2, pageSize: 5).ToList();

            Assert.AreEqual(5, page1.Count);
            Assert.AreEqual(5, page2.Count);
            Assert.AreNotEqual(page1[0].Id, page2[0].Id);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }
    }
}