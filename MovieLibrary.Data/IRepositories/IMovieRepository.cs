using System;
using System.Collections.Generic;
using System.Text;
using MovieLibrary.Data.Entities;

namespace MovieLibrary.Data.IRepositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        //IEnumerable<Movie> FilterMovies(
        //    string title = null,
        //    IEnumerable<int> categoryIds = null,
        //    double? minRating = null,
        //    double? maxRating = null,
        //    int pageNumber = 1,
        //    int pageSize = 10);  
        
    }
}
