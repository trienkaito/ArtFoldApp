using ArtFold.Data;
using ArtFold.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtFold.Areas.Admin.Controllers
{
    public class AdminAuthenticationController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ArtFoldDbContext _context;

        public AdminAuthenticationController(SignInManager<User> signInManager,
            UserManager<User> userManager, ArtFoldDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        [Area("Admin")]
        [Route("Admin/Login/")]
        public IActionResult Login()
        {
            return View();
        }

        [Area("Admin")]
        [Route("Admin/Logout/")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Admin/Login");
        }

        [HttpPost]
        [Route("Admin/Login/")]
        public async Task<ActionResult> Login(UserLogin user)
        {
            if (ModelState.IsValid)
            {
                // Tìm kiếm người dùng theo email
                var user_login = await _context.Users.FirstOrDefaultAsync(l => l.Email.Equals(user.Email));

                var adminRole = await _userManager.GetRolesAsync(user_login);
                if (adminRole.Any(i => i.Equals("Admin")))
                {
                    // Check the password for the associated user
                    var result = await _signInManager.PasswordSignInAsync(user_login, user.Password, isPersistent: false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Successful login, redirect to the admin dashboard
                        return Redirect("/Admin/Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Invalid password, please enter again.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Admin user not found.");
                }
            }

            return Redirect("/Admin/Login");
        }
    }
}
