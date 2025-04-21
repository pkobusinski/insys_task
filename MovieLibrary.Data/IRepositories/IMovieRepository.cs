#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using MovieLibrary.Data.Entities;
using static MovieLibrary.Data.Repositories.MovieRepository;

namespace MovieLibrary.Data.IRepositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        public IEnumerable<Movie> FilterMovies(
                string? text = null, IEnumerable<int>? categoryIds = null,
                decimal? minImdb = null, decimal? maxImdb = null);

    }
}
