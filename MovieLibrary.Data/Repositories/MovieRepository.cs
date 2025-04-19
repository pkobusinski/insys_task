using System;
using System.Collections.Generic;
using System.Text;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.IRepositories;

namespace MovieLibrary.Data.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieLibraryContext context) : base(context) { }
    }
}
