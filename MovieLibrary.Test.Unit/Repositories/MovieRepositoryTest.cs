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
    public class MovieRepositoryTest
    {
        private MovieLibraryContext _context;
        private MovieRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<MovieLibraryContext>()
                .UseInMemoryDatabase(databaseName: $"MovieLibraryTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new MovieLibraryContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repository = new MovieRepository(_context);

            SeedTestData();
        }

        private void SeedTestData()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Action" },
                new Category { Id = 2, Name = "Drama" }
            };
            _context.Categories.AddRange(categories);

            var movies = new List<Movie>
            {
                new Movie { Id = 1, Title = "Movie 1", ImdbRating = 8.0m },
                new Movie { Id = 2, Title = "Movie 2", ImdbRating = 6.5m }
            };
            _context.Movies.AddRange(movies);

            var movieCategories = new List<MovieCategory>
            {
                new MovieCategory { CategoryId = 1, MovieId = 1 },
                new MovieCategory { CategoryId = 2, MovieId = 1 },
                new MovieCategory { CategoryId = 1, MovieId = 2 }
            };
            _context.MovieCategories.AddRange(movieCategories);

            _context.SaveChanges();
        }

        [TestMethod]
        public void FilterMovies_ShouldReturnMoviesByText()
        {
            var result = _repository.FilterMovies(text: "Movie 1", minImdb: 0).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Movie 1", result.First().Title);
        }

        [TestMethod]
        public void FilterMovies_ShouldReturnMoviesByCategory()
        {
            var result = _repository.FilterMovies(text: null, categoryIds: new List<int> { 1 }).ToList();

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void FilterMovies_ShouldReturnMoviesByImdbRating()
        {
            var result = _repository.FilterMovies(minImdb: 7.0m, maxImdb: 10.0m).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Movie 1", result.First().Title);
        }

        [TestMethod]
        public void FilterMovies_ShouldReturnMoviesByTextAndCategory()
        {
            var result = _repository.FilterMovies(text: "Movie 1", categoryIds: new List<int> { 1 }).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Movie 1", result.First().Title);
        }

        [TestMethod]
        public void GetAllMovies_ShouldReturnAllMovies()
        {
            var result = _repository.GetAll().ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(m => m.Title == "Movie 1"));
            Assert.IsTrue(result.Any(m => m.Title == "Movie 2"));
        }

        [TestMethod]
        public void GetMovieById_ShouldReturnMovieById()
        {
            var movie = _repository.GetById(1);

            Assert.IsNotNull(movie);
            Assert.AreEqual("Movie 1", movie.Title);
        }

        [TestMethod]
        public void AddMovie_ShouldAddMovie()
        {
            var newMovie = new Movie { Title = "Movie 3", ImdbRating = 7.5m };

            _repository.Add(newMovie);
            var addedMovie = _repository.GetById(newMovie.Id);
            var allMovies = _repository.GetAll().ToList();

            Assert.IsNotNull(addedMovie);
            Assert.AreEqual("Movie 3", addedMovie.Title);
            Assert.AreEqual(3, allMovies.Count);

            var dbMovie = _context.Movies.FirstOrDefault(m => m.Title == "Movie 3");
            Assert.IsNotNull(dbMovie);
        }

        [TestMethod]
        public void UpdateMovie_ShouldUpdateMovie()
        {
            var movieToUpdate = _context.Movies.First(m => m.Title == "Movie 1");
            movieToUpdate.Title = "Updated Movie 1";

            _repository.Update(movieToUpdate);
            var updatedMovie = _repository.GetById(movieToUpdate.Id);

            Assert.IsNotNull(updatedMovie);
            Assert.AreEqual("Updated Movie 1", updatedMovie.Title);

            var dbMovie = _context.Movies.Find(movieToUpdate.Id);
            Assert.AreEqual("Updated Movie 1", dbMovie.Title);
        }

        [TestMethod]
        public void DeleteMovie_ShouldRemoveMovie()
        {
            var movieToDelete = _context.Movies.First(m => m.Title == "Movie 2");
            var initialCount = _context.Movies.Count();

            _repository.Delete(movieToDelete.Id);
            var remainingMovies = _repository.GetAll().ToList();

            Assert.AreEqual(initialCount - 1, remainingMovies.Count);
            Assert.IsFalse(remainingMovies.Any(m => m.Title == "Movie 2"));

            var dbMovie = _context.Movies.Find(movieToDelete.Id);
            Assert.IsNull(dbMovie);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
