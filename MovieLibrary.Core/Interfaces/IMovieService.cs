#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using MovieLibrary.Core.Models;

namespace MovieLibrary.Core.Interfaces
{
    public interface IMovieService : IService<MovieDto>
    {
        IEnumerable<MovieDto> FilterMovies(string? text = null, IEnumerable<int>? categoryIds = null,
                decimal? minImdb = null, decimal? maxImdb = null, int? page = 1, int? pageSize = 10);
       
    }
}
