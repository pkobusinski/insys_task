using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.Core.Models
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public decimal ImdbRating { get; set; }

        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();

    }
}
