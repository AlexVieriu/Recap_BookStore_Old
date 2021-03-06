using BookStore_API.Data;

namespace BookStore_API.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string Isbn { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public string File { get; set; }
        public int? AuthorId { get; set; }
        public virtual AuthorDTO Author { get; set; }
    }
}
