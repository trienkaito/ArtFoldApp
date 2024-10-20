using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArtFold.Data;
using ArtFold.Models;
using Microsoft.AspNetCore.Identity;
using X.PagedList;
using X.PagedList.Extensions;


namespace ArtFold.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ArtFoldDbContext _context;
        public ProductsController(ArtFoldDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var artFoldDbContext = _context.Products.Include(p => p.Category);
            return View(await artFoldDbContext.ToListAsync());
        }

        // GET: ListProducts
        public async Task<IActionResult> ShowProductsWithPagination(int? page)
        {
            if (page == null) page = 1;

            var allProducts = await _context.Products
                                    .Include(p => p.Category)
                                    .OrderByDescending(p => p.CreatedAt)
                                    .ToListAsync();

            int pageSize = 9;
            int pageNumber = (page ?? 1);


            return PartialView("_listProducts", allProducts.ToPagedList(pageNumber, pageSize));
            
        }

        public async Task<IActionResult> FilterByCategory(string? category, int? page)
        {
            if (page == null) page = 1;

            var product = await _context.Products
                                   .Where(p => p.Category.CategoryName == category)
                                   .OrderByDescending(p => p.CreatedAt)
                                   .ToListAsync();

            int pageSize = 9;
            int pageNumber = (page ?? 1);

            return PartialView("_ListProducts", product.ToPagedList(pageNumber, pageSize));

        }


        public async Task<IActionResult> FilterByPrice(int minPrice, int maxPrice, int? page)
        {
            if (page == null) page = 1;

            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice) 
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            int pageSize = 9;
            int pageNumber = (page ?? 1);

            return PartialView("_listProducts", products.ToPagedList(pageNumber, pageSize));
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> GetRelatedProduct()
        {
            var relatedProducts = await _context.Products
                .Include (p => p.Category)
                .OrderBy(p => Guid.NewGuid())
                .Take(4)
                .ToListAsync();
            return PartialView("_relatedProducts", relatedProducts);
        }

        public async Task<IActionResult> GetLatestProduct()
        {
            var latestProduct = await _context.Products
                                    .OrderByDescending(p => p.CreatedAt)
                                    .Take(3)
                                    .ToListAsync();
            return PartialView("_lastestProduct", latestProduct);
        }

        public async Task<IActionResult> SalesOffProducts()
        {
            var saleOffProduct = await _context.Products
                                    .Include(p => p.Category)
                                    .OrderBy(p => Guid.NewGuid())
                                    .Take(6)
                                    .ToListAsync();
            return PartialView("_salesOff", saleOffProduct);
        }




        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,CategoryID,Name,ImgUrl,PrintPaperType,Price,ProductQuantity,Description,CreatedAt,UpdatedAt")] Product product)
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

        // GET: Products/Edit/5
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

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProductID,CategoryID,Name,ImgUrl,PrintPaperType,Price,ProductQuantity,Description,CreatedAt,UpdatedAt")] Product product)
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

        // GET: Products/Delete/5
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

        // POST: Products/Delete/5
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
