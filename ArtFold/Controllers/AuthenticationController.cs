using ArtFold.Data;
using ArtFold.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ArtFold.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ArtFoldDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEmailSender _emailSender;

        public AuthenticationController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ArtFoldDbContext context,
            IPasswordHasher<User> passwordHasher,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _passwordHasher = passwordHasher;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(e => e.Email == user.Email);
                if (existingUser != null)
                {
                    //Kiểm tra mật khẩu
                    var result = await _signInManager.PasswordSignInAsync(existingUser, user.Password, isPersistent: false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        await _userManager.GetUserAsync(User);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Invalid password please enter again.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "User not found.");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegister model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View(model);
                }
                if (!model.AgreeTerms)
                {
                    ModelState.AddModelError("AgreeTerms", "Please agree our Terms before Sign In.");
                    return View(model);
                }

                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    CreatedAt = DateTime.Now,
                };

                user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

                var otp = new Random().Next(100000, 999999).ToString();
                await _emailSender.SendEmailAsync(model.Email, "Mã xác nhận OTP", $"Mã OTP của bạn là: {otp}");

                HttpContext.Session.SetString("Otp", otp);
                HttpContext.Session.SetString("Email", model.Email);
                HttpContext.Session.SetString("UserName", model.UserName);
                HttpContext.Session.SetString("PasswordHash", user.PasswordHash);
                HttpContext.Session.SetString("PhoneNumber", user.PhoneNumber);
                HttpContext.Session.SetString("FullName", user.FullName);

                return RedirectToAction("VerifyOtpAfterSignUp", new { email = model.Email });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult VerifyOtpAfterSignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOtpAfterSignUp(VerifyOtp model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var otp = $"{model.OtpDigit1}{model.OtpDigit2}{model.OtpDigit3}{model.OtpDigit4}{model.OtpDigit5}{model.OtpDigit6}";

            // Lấy OTP và Email đã lưu trong Session
            var storedOtp = HttpContext.Session.GetString("Otp");
            var storedEmail = HttpContext.Session.GetString("Email");

            if (otp.Equals(storedOtp) && !string.IsNullOrEmpty(storedEmail))
            {
                // Xóa OTP và email khỏi session
                HttpContext.Session.Remove("Otp");

                var userName = HttpContext.Session.GetString("UserName");
                var passwordHash = HttpContext.Session.GetString("PasswordHash");
                var phone = HttpContext.Session.GetString("PhoneNumber");
                var fullName = HttpContext.Session.GetString("FullName");

                var user = new User
                {
                    UserName = userName,
                    Email = storedEmail,
                    PasswordHash = passwordHash,
                    PhoneNumber = phone,
                    FullName = fullName,
                    CreatedAt = DateTime.Now,
                    EmailConfirmed = true // Xác nhận email
                };

                var cart = new Cart
                {
                    CartID = Guid.NewGuid(),
                    UserID = user.Id,
                };

                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();

                var result = await _userManager.CreateAsync(user, user.PasswordHash);

                if (result.Succeeded)
                {
                    // Gán role "Customer" cho người dùng
                    await _userManager.AddToRoleAsync(user, "Customer");

                    // Xóa thông tin OTP khỏi Session
                    HttpContext.Session.Remove("Otp");
                    HttpContext.Session.Remove("Email");
                    HttpContext.Session.Remove("UserName");
                    HttpContext.Session.Remove("PasswordHash");
                    HttpContext.Session.Remove("PhoneNumber");

                    // Chuyển hướng đến trang đăng nhập
                    return RedirectToAction("Login", "Authentication");
                }
            }
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(Models.ForgotPassword user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var forgotPassUser = await _context.Users.FirstOrDefaultAsync(f => f.Email == user.Email);
            if (forgotPassUser == null)
            {
                ModelState.AddModelError("Email", "Email dose not exist!");
                return View(user);
            }

            // Tạo mã OTP (có thể là 6 chữ số ngẫu nhiên)
            var otp = new Random().Next(100000, 999999).ToString();

            HttpContext.Session.SetString("ForgotPasswordEmail", user.Email);
            HttpContext.Session.SetString("ForgotPasswordOTP", otp);

            // Gửi email chứa mã OTP (sử dụng dịch vụ gửi email)
            await _emailSender.SendEmailAsync(user.Email, "Mã xác nhận OTP", $"Mã OTP của bạn là: {otp}");

            // Chuyển đến bước nhập mã OTP
            return RedirectToAction("VerifyOtp", new { email = user.Email });
        }



        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyOtp(VerifyOtp model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            var otp = $"{model.OtpDigit1}{model.OtpDigit2}{model.OtpDigit3}{model.OtpDigit4}{model.OtpDigit5}{model.OtpDigit6}";

            // Lấy mã OTP và email từ Session
            var sessionOtp = HttpContext.Session.GetString("ForgotPasswordOTP");
            var sessionEmail = HttpContext.Session.GetString("ForgotPasswordEmail");

            if (otp.Equals(sessionOtp) && !string.IsNullOrEmpty(sessionEmail))
            {
                // Xóa OTP khỏi session
                HttpContext.Session.Remove("ForgotPasswordOTP");

                return RedirectToAction("ResetPassword", new { email = sessionEmail });
            }
            else
            {
                ModelState.AddModelError("OtpDigit1", "Mã OTP không chính xác, vui lòng thử lại.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(Models.ResetPassword model)
        {
            // Kiểm tra xem email có tồn tại trong session không
            var email = HttpContext.Session.GetString("ForgotPasswordEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ForgotPassword", "Authentication");
            }

            // Kiểm tra tính hợp lệ của model
            if (ModelState.IsValid)
            {
                // Tìm người dùng theo email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user != null)
                {
                    // Hash mật khẩu mới
                    user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

                    // Cập nhật mật khẩu vào cơ sở dữ liệu
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    // Xóa email khỏi session
                    HttpContext.Session.Remove("ForgotPasswordEmail");

                    // Đăng nhập người dùng
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Chuyển hướng đến trang đăng nhập hoặc trang chính
                    return RedirectToAction("Login", "Authentication");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }


            // Nếu có lỗi, trả về view với model đã nhập
            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(Models.ChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Lấy thông tin người dùng hiện tại
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            // Kiểm tra mật khẩu cũ có chính xác không
            var isOldPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.OldPassword);
            if (!isOldPasswordCorrect)
            {
                ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng.");
                return View(model);
            }

            // Kiểm tra mật khẩu mới và xác nhận mật khẩu có giống nhau không
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                ModelState.AddModelError("ConfirmNewPassword", "Mật khẩu mới và xác nhận mật khẩu không khớp.");
                return View(model);
            }

            // Thay đổi mật khẩu
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                // Đăng xuất sau khi đổi mật khẩu thành công
                await _signInManager.SignOutAsync();

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                // Chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Authentication");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}
