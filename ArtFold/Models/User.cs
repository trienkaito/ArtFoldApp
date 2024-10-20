using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ArtFold.Models
{
    public class User : IdentityUser
    {
        [RegularExpression(@"^\S+$", ErrorMessage = "User Name cannot contain whitespace.")]
        public override string? UserName { get; set; }


        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^\S+@\S+\.\S+$", ErrorMessage = "Email cannot contain whitespace.")]
        public override string? Email { get; set; }

        [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
        [RegularExpression(@"^[\p{L}\p{M}]+(\s[\p{L}\p{M}]+)*$", ErrorMessage = "Full Name cannot contain special characters.")]
        public string? FullName {  get; set; }

        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone Number must contain 10 digits, start with 0, and not contain special characters or letters.")]
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? HouseAddress {  get; set; }
        public DateTime? CreatedAt { get; set; }
        public ICollection<CheckOut>? CheckOuts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
