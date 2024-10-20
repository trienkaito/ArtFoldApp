using System.ComponentModel.DataAnnotations;

namespace ArtFold.Models
{
    public class UserRegister
    {
        [RegularExpression(@"^\S+$", ErrorMessage = "User Name cannot contain whitespace.")]
        public string UserName { get; set; }

        [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
        [RegularExpression(@"^[\p{L}\p{M}]+(\s[\p{L}\p{M}]+)*$", ErrorMessage = "Full Name cannot contain special characters.")]
        public string FullName { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$", ErrorMessage = "Password must be at least 6 characters long, contain at least one uppercase letter, one lowercase letter, one special character, one number, and no spaces.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string RepeatPassword { get; set; }

        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone Number must contain 10 digits, start with 0, and not contain special characters or letters.")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^\S+@\S+\.\S+$", ErrorMessage = "Email cannot contain whitespace.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must agree to the terms.")]
        public bool AgreeTerms { get; set; }
    }
}
