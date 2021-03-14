using System.ComponentModel.DataAnnotations;

namespace BookStore_API.DTOs
{
    public class AuthorCreateDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Bio { get; set; }

    }
}
