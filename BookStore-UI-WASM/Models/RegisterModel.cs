using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore_UI_WASM.Models
{
    public class RegisterModel
    {
        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Pass must be between {2} and {1} characters")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password doesn't match")]
        public string ConfirmedPassword { get; set; }
    }
}
