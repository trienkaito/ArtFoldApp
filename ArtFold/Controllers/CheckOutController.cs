using ArtFold.Data;
using ArtFold.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json.Nodes;

namespace ArtFold.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly ArtFoldDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private string PaypalClientId { get; set; } = "";
        private string PaypalSecret { get; set; } = "";
        private string PaypalUrl { get; set; } = "";


        public CheckOutController(ArtFoldDbContext context, UserManager<User> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            PaypalClientId = configuration["PayPal:ClientId"];
            PaypalSecret = configuration["PayPal:ClientSecret"];
            PaypalUrl = configuration["PayPal:Url"];
        }
        public async Task<IActionResult> Index()
        {
            string clientId = _configuration["PayPal:ClientId"];

            // Truyền clientId vào View
            ViewBag.PayPalClientId = clientId;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            var listCartProducts = await _context.Carts
                                        .Include(c => c.CartProducts)
                                        .ThenInclude(cp => cp.Product)
                                        .FirstOrDefaultAsync(c => c.UserID == user.Id);


            return View(listCartProducts);
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrder([FromBody] JsonObject data)
        {
            var totalAmount = data?["amount"]?.ToString();
            if(totalAmount == null)
            {
                return new JsonResult(new { Id = "" });
            }

            JsonObject createOrderRequest = new JsonObject();
            createOrderRequest.Add("intent", "CAPTURE");

            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "USD");
            amount.Add("value", totalAmount);

            JsonObject purchaseUnit1 = new JsonObject();
            purchaseUnit1.Add("amount", amount);

            JsonArray purchaseUnits = new JsonArray();
            purchaseUnits.Add(purchaseUnit1);

            createOrderRequest.Add("purchase_units", purchaseUnits);

            string accessToken = await GetPaypalAccessToken();

            string url = PaypalUrl + "/v2/checkout/orders";


            using(var client =  new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(createOrderRequest.ToString(), null, "application/json");
                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse =  JsonNode.Parse(strResponse);

                    if(jsonResponse != null)
                    {
                        string paypalOrderId = jsonResponse["id"]?.ToString() ?? "";
                        
                        return new JsonResult(new {Id = paypalOrderId});
                    }
                }
            }

            return new JsonResult(new { Id = "" });
        }

        [HttpPost]
        public async Task<JsonResult> CompleteOrder([FromBody] JsonObject data)
        {
            var orderId = data?["orderID"]?.ToString();
            if (orderId == null)
            {
                return new JsonResult("error");
            }

            string accessToken = await GetPaypalAccessToken();

            string url = PaypalUrl + "/v2/checkout/orders/" + orderId + "/capture";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("", null, "application/json");
                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);

                    if (jsonResponse != null)
                    {
                        string paypalOrderStatus = jsonResponse["status"]?.ToString() ?? "";

                        if(paypalOrderStatus == "COMPLETED")
                        {
                            var userId = _userManager.GetUserId(User);

                            // Tìm Cart của người dùng
                            var userCart = await _context.Carts
                                .Include(c => c.CartProducts)
                                .ThenInclude(cp => cp.Product)
                                .FirstOrDefaultAsync(c => c.UserID == userId);

                            if (userCart != null)
                            {
                                var checkOut = new CheckOut
                                {
                                    UserID = userId,
                                    OrderDate = DateTime.UtcNow,
                                    PaymentMethod = "PayPal",
                                    TotalPrice = userCart.CartProducts.Sum(cp => cp.Product.Price * cp.ProductCartQuantity),
                                };

                                await _context.CheckOuts.AddAsync(checkOut);
                                await _context.SaveChangesAsync();

                                foreach (var cartProduct in userCart.CartProducts)
                                {
                                    var checkOutProduct = new CheckOutProduct
                                    {
                                        CheckOutID = checkOut.CheckOutID,
                                        ProductID = cartProduct.ProductID,
                                        Quantity = cartProduct.ProductCartQuantity
                                    };

                                    await _context.CheckOutProducts.AddAsync(checkOutProduct);
                                }

                 
                                _context.CartProducts.RemoveRange(userCart.CartProducts);
                                await _context.SaveChangesAsync();
                            }



                            return new JsonResult(new { success = true, redirectUrl = Url.Action("Success", "CheckOut") });
                        }
                    }
                }
            }

            return new JsonResult("error");
        }

        public async Task<IActionResult> PayByCash()
        {
            var userId = _userManager.GetUserId(User);

            // Tìm Cart của người dùng
            var userCart = await _context.Carts
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            if (userCart == null || userCart.CartProducts == null || !userCart.CartProducts.Any())
            {
                return Json(new { success = false, message = "Please choose product before make an order!" });
            }

            
            
                // Khởi tạo CheckOut với phương thức thanh toán bằng tiền mặt
                var checkOut = new CheckOut
                {
                    UserID = userId,
                    OrderDate = DateTime.UtcNow,
                    PaymentMethod = "Cash",
                    TotalPrice = userCart.CartProducts.Sum(cp => cp.Product.Price * cp.ProductCartQuantity),
                };

                await _context.CheckOuts.AddAsync(checkOut);
                await _context.SaveChangesAsync();

                // Lưu các sản phẩm từ giỏ hàng vào CheckOutProduct
                foreach (var cartProduct in userCart.CartProducts)
                {
                    var checkOutProduct = new CheckOutProduct
                    {
                        CheckOutID = checkOut.CheckOutID,
                        ProductID = cartProduct.ProductID,
                        Quantity = cartProduct.ProductCartQuantity
                    };

                    await _context.CheckOutProducts.AddAsync(checkOutProduct);
                }

                // Xóa các sản phẩm trong giỏ hàng
                _context.CartProducts.RemoveRange(userCart.CartProducts);
                await _context.SaveChangesAsync();

            

            return Json(new { success = true, redirectUrl = Url.Action("Success", "CheckOut") });

        }


        public async Task<IActionResult> Success()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy CheckOut mới nhất của người dùng
            var userCheckOut = await _context.CheckOuts
                .Where(c => c.UserID == user.Id)
                .OrderByDescending(c => c.OrderDate)
                .FirstOrDefaultAsync();

            if (userCheckOut == null)
            {
                return NotFound();
            }

            return View(userCheckOut);
        }



        //public async Task<string> Token()
        //{
        //    return await GetPaypalAccessToken();
        //}

        //Token: A21AAIlZmwSt5bzeJFb6AgQoWzqRYMHN6YzcvTVs3Ijh1T6C_4EU6rk5_EJ0yXK21Xi999bRNQIupo1y8vnok-araOD4dkYUQ

        private async Task<string> GetPaypalAccessToken()
        {
            string accessToken = "";

            string url = PaypalUrl + "/v1/oauth2/token";

            using (var client = new HttpClient())
            {
                string credentials64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(PaypalClientId + ":" + PaypalSecret));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials64);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("grant_type=client_credentials", null, "application/x-www-form-urlencoded");

                var httpResponse = await client.SendAsync(requestMessage);

               if(httpResponse.IsSuccessStatusCode)
               {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();

                    var jsonResponse =  JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        accessToken = jsonResponse["access_token"]?.ToString() ?? "";
                    }

               }
            }

            return accessToken;
        }


        public async Task<IActionResult> Purchase()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userCheckOuts = await _context.CheckOuts
                            .Where(c => c.UserID == user.Id)
                            .Include(c => c.CheckOutProducts)
                            .ThenInclude(cp => cp.Product)
                            .OrderByDescending(c => c.OrderDate)
                            .ToListAsync();

            return View(userCheckOuts);
        }
    }


}
