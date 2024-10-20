using ArtFold.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using ArtFold.Data;

namespace ArtFold.Controllers
{
    public class LoginGGController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ArtFoldDbContext _context;

        public LoginGGController(SignInManager<User> signInManager, UserManager<User> userManager, ArtFoldDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context; 
        }

        [HttpGet]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "LoginGG");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse(string returnUrl = null)
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (result?.Principal == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            // Lấy thông tin người dùng từ Google
            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var fullName = result.Principal.FindFirstValue(ClaimTypes.Name);
            var phone = result.Principal.FindFirstValue(ClaimTypes.MobilePhone);
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // Nếu người dùng chưa tồn tại, có thể tạo tài khoản mới
                user = new User
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    CreatedAt = DateTime.Now
                };
                var resultCreate = await _userManager.CreateAsync(user);
                if (!resultCreate.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                await _userManager.AddToRoleAsync(user, "Customer");

                // Tạo Cart cho người dùng mới
                var cart = new Cart
                {
                    UserID = user.Id,
                    CartID = Guid.NewGuid()
                };

                // Lưu Cart vào cơ sở dữ liệu
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Đăng nhập người dùng
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
    }
}
