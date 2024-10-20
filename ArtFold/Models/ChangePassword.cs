using System.ComponentModel.DataAnnotations;

namespace ArtFold.Models
{
    public class ChangePassword
    {
        public string OldPassword { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$", ErrorMessage = "Password must be at least 6 characters long, contain at least one uppercase letter, one lowercase letter, one special character, one number, and no spaces.")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "NewPassword and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
