using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore_UI_WASM.Models
{
    public class LoginModel
    {
        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
