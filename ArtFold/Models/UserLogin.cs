using System.ComponentModel.DataAnnotations;

namespace ArtFold.Models
{
    public class UserLogin
    {
        [Required]
        [RegularExpression(@"^\S+@\S+\.\S+$", ErrorMessage = "Email cannot contain whitespace.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
