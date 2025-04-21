#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.IRepositories;

namespace MovieLibrary.Data.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieLibraryContext context) : base(context) { }

        public override IEnumerable<Movie> GetAll() {
            return _context.Movies
            .Include(m => m.MovieCategories)
            .ThenInclude(mc => mc.Category)
            .ToList();
        }
        public override Movie GetById(int id) { 
            return _context.Movies
            .Include(m => m.MovieCategories)
            .ThenInclude(mc => mc.Category)
            .FirstOrDefault(m => m.Id == id);
        }

     
        public IEnumerable<Movie> FilterMovies(string? text = null, IEnumerable<int>? categoryIds = null,
                        decimal? minImdb = null, decimal? maxImdb = null)
        {

            IQueryable<Movie> query = _context.Movies
                .Include(m => m.MovieCategories)
                .ThenInclude(mc => mc.Category);

            if (!string.IsNullOrEmpty(text))
            {
                string lowerText = text.ToLower();
                query = query.Where(m => m.Title.ToLower().Contains(lowerText));
            }

            if (categoryIds != null && categoryIds.Any())
            {
                query = query.Where(m => m.MovieCategories.Any(c => categoryIds.Contains(c.CategoryId)));
            }

            if (minImdb.HasValue)
            {
                query = query.Where(m => m.ImdbRating >= minImdb.Value);
            }
            if (maxImdb.HasValue)
            {
                query = query.Where(m => m.ImdbRating <= maxImdb.Value);
            }

            query = query.OrderByDescending(m => (double)m.ImdbRating);

            return query.ToList();
        }
    }
}
