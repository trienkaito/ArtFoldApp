using ArtFold.Data;
using ArtFold.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArtFold.Controllers
{
    public class UsersController : Controller
    {
        private readonly ArtFoldDbContext _context;
        private readonly UserManager<User> _userManager;

        public UsersController(ArtFoldDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAddress(string city, string district, string ward, string houseAddress)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Cập nhật thông tin địa chỉ
                user.City = city;
                user.District = district;
                user.Ward = ward;
                user.HouseAddress = houseAddress;


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Json(new { success = true, userName = user.FullName, userPhone = user.PhoneNumber, houseAddress = user.HouseAddress, ward = user.Ward, district = user.District, city = user.City });
                }
            }

            return RedirectToAction("Login", "Authentication");
        }


        public async Task<IActionResult> GetUserAddressInformation()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Kiểm tra xem các trường city, district, ward có null không
                if (string.IsNullOrEmpty(user.City) || string.IsNullOrEmpty(user.District) || string.IsNullOrEmpty(user.Ward))
                {
                    return Json(new { hasAddress = false });
                }

                var userAddress = new
                {
                    userName = user.FullName,
                    userPhone = user.PhoneNumber,
                    houseAddress = user.HouseAddress,
                    ward = user.Ward,
                    district = user.District,
                    city = user.City
                };

                return Json(new { hasAddress = true, userAddress });
            }

            return RedirectToAction("Login", "Authentication");
        }


        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                return View(user);
            }


            return RedirectToAction("Login", "Authentication");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(User model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    // Kiểm tra email có tồn tại trong hệ thống hay không
                    if (_context.Users.Any(u => u.Email == model.Email && u.Id != user.Id))
                    {
                        ModelState.AddModelError("Email", "Email already exists.");
                        return View(model);
                    }

                    // Cập nhật các trường dữ liệu
                    user.FullName = model.FullName;
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.PhoneNumber = model.PhoneNumber;

                    // Cập nhật thông tin người dùng
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return Json(new { success = true, message = "Profile updated successfully!" });
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // Trả về các lỗi validation nếu có
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                                   .Select(x => new { field = x.Key, errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList() })
                                   .ToList();
            return Json(new { success = false, errors });
        }

    }
}
