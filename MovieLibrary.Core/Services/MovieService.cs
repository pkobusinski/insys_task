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
    public class MovieService : IMovieService
    {
        protected readonly IMovieRepository _movieRepository;

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
                ImdbRating = movie.ImdbRating
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
                ImdbRating = movie.ImdbRating
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
    }
}

