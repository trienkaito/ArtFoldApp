using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArtFold.Data;
using ArtFold.Models;
using Microsoft.AspNetCore.Authorization;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ArtFold.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly ArtFoldDbContext _context;
        private readonly Cloudinary _cloudinary;

        public ProductsController(ArtFoldDbContext context, Cloudinary cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        // POST: Admin/Products/UploadImages
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImages(IFormFile[] slideImageInput, Guid productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                ModelState.AddModelError("", "Product does not exist.");
                return View(slideImageInput); 
            }

            if (slideImageInput != null && slideImageInput.Length > 0)
            {
                foreach (var imageFile in slideImageInput)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Upload image to Cloudinary
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream())
                        };

                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                        if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            // Lấy URL của ảnh từ Cloudinary
                            var imageUrl = uploadResult.SecureUrl.ToString();

                            // Tạo ProductImage object để lưu vào database
                            var productImage = new ProductImage
                            {
                                ImageUrl = imageUrl,
                                ProductID = productId,
                                CreatedAt = DateTime.Now,
                                UpdateAt = DateTime.Now,
                            };

                            // Lưu vào database
                            _context.ProductImages.Add(productImage);
                        }
                        else
                        {
                            // Xử lý lỗi khi upload không thành công
                            ModelState.AddModelError("", $"Error uploading image '{imageFile.FileName}' to Cloudinary.");
                        }
                    }
                }
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Images uploaded successfully!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Please select at least one image file.");
            return View("Edit"); 
        }


        [HttpPost]
        public IActionResult DeleteImage(Guid productImageID)
        {
            var image = _context.ProductImages.Find(productImageID);
            if (image != null)
            {
                _context.ProductImages.Remove(image);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Image deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Image not found!";
            }

            return RedirectToAction("Index");
        }




        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var artFoldDbContext = _context.Products.Include(p => p.Category);
            return View(await artFoldDbContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,CategoryID,Name,ImgUrl,PrintPaperType,Price,ProductQuantity,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.ProductID = Guid.NewGuid();
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProductID,CategoryID,Name,ImgUrl,PrintPaperType,Price,ProductQuantity,Description")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
