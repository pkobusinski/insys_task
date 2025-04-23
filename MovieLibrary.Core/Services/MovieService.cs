#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using MovieLibrary.Core.Interfaces;
using MovieLibrary.Core.Models;
using MovieLibrary.Data.IRepositories;
using MovieLibrary.Data.Entities;
using System.Linq;
using MovieLibrary.Data;

namespace MovieLibrary.Core.Services
{
    public class MovieService : IMovieService
    {
        protected readonly IMovieRepository _movieRepository;

        public MovieLibraryContext Context { get; }

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public IEnumerable<MovieDto> GetAll()
        {
            var movies = _movieRepository.GetAll();
            return movies.Select(movie => new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Year = movie.Year,
                ImdbRating = movie.ImdbRating,
                Categories = movie.MovieCategories.Select(c => new CategoryDto
                {
                    Id = c.Category.Id,
                    Name = c.Category.Name
                }).ToList(),
            });
        }

        public MovieDto GetById(int id)
        {
            var movie = _movieRepository.GetById(id);
            if (movie == null)
                return null;

            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Year = movie.Year,
                ImdbRating = movie.ImdbRating,
                Categories = movie.MovieCategories.Select(c => new CategoryDto
                {
                    Id= c.Category.Id,
                    Name = c.Category.Name
                }).ToList(),
            };
        }

        public MovieDto Create(MovieDto entity)
        {
            var movie = new Movie
            {
                Title = entity.Title,
                Description = entity.Description,
                Year = entity.Year,
                ImdbRating = entity.ImdbRating
            };

            _movieRepository.Add(movie);
            entity.Id = movie.Id;

            return entity; 
        }

        public MovieDto Update(MovieDto entity)
        {
            var movie = _movieRepository.GetById(entity.Id);
            if (movie == null)
                return null;

            movie.Title = entity.Title;
            movie.Description = entity.Description;
            movie.Year = entity.Year;
            movie.ImdbRating = entity.ImdbRating; 

            _movieRepository.Update(movie);

            return entity; 
        }

        public bool Delete(int id)
        {
            var movie = _movieRepository.GetById(id);
            if (movie == null)
                return false;

            _movieRepository.Delete(movie.Id);
            return true; 
        }

        public IEnumerable<MovieDto> FilterMovies( string? text = null, IEnumerable<int>? categoryIds = null, 
            decimal? minImdb = null, decimal? maxImdb = null, int? page = 1, int? pageSize = 10)
        {
            
            int currentPage = page == null ? 1 : page.Value;
            int currentPageSize = pageSize == null ? 10: pageSize.Value;

            var movies = _movieRepository.FilterMovies(text, categoryIds, minImdb, maxImdb);

            var pagedMovies = movies
                .Skip((currentPage - 1) * currentPageSize)  
                .Take(currentPageSize)              
                .Select(movie => new MovieDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Description = movie.Description,
                    Year = movie.Year,
                    ImdbRating = movie.ImdbRating,
                    Categories = movie.MovieCategories.Select(c => new CategoryDto
                    {
                        Id = c.Category.Id,
                        Name = c.Category.Name
                    }).ToList(),
                }).ToList();

            return pagedMovies;  
        }
    }
}

