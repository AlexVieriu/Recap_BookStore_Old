using System.ComponentModel.DataAnnotations;

namespace BookStore_API.DTOs
{
    public class BookCreateDTO
    {
        [Required]
        public string Title { get; set; }
        public int? Year { get; set; }
        [Required]
        public string Isbn { get; set; }
        [StringLength(500)]
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public string File { get; set; }
    }
}
