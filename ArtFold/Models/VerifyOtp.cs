using System.ComponentModel.DataAnnotations;

namespace ArtFold.Models
{
    public class VerifyOtp
    {
        [Required(ErrorMessage = "Mã OTP không được để trống.")]
        public string OtpDigit1 { get; set; }

        [Required(ErrorMessage = "Mã OTP không được để trống.")]
        public string OtpDigit2 { get; set; }

        [Required(ErrorMessage = "Mã OTP không được để trống.")]
        public string OtpDigit3 { get; set; }

        [Required(ErrorMessage = "Mã OTP không được để trống.")]
        public string OtpDigit4 { get; set; }

        [Required(ErrorMessage = "Mã OTP không được để trống.")]
        public string OtpDigit5 { get; set; }

        [Required(ErrorMessage = "Mã OTP không được để trống.")]
        public string OtpDigit6 { get; set; }
    }
}
