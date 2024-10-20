using Microsoft.AspNetCore.Mvc;
using ArtFold.Models;
using ArtFold.Data;
using Microsoft.EntityFrameworkCore;


namespace TechShop.Areas.Admin.ViewComponents
{
    public class AddProductImageListViewComponent : ViewComponent
    {
        private readonly ArtFoldDbContext _context;
        public AddProductImageListViewComponent(ArtFoldDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(Product product)
        {
            List<ProductImage> slideImages = await _context.ProductImages
                                                    .Where(p => p.ProductID == product.ProductID)
                                                    .OrderBy(p => p.CreatedAt)
                                                    .ToListAsync();
            ViewBag.SlideImages = slideImages;

            return View(product);
        }
    }
}
