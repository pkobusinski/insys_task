namespace MovieLibrary.Data.Entities
{
    public class MovieCategory
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public int CategoryId { get; set; }

        public virtual Movie Movie { get; set; }

        public virtual Category Category { get; set; }
    }
}
