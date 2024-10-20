using System.ComponentModel.DataAnnotations;

namespace ArtFold.Models
{
    public class ForgotPassword
    {
        [Required]
        public string Email { get; set; }
    }
}
