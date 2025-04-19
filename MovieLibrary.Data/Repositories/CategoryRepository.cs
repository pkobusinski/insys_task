using System;
using System.Collections.Generic;
using System.Text;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.IRepositories;


namespace MovieLibrary.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(MovieLibraryContext context) : base(context) { }
    }
}
