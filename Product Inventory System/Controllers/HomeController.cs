using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Inventory_System.Data;
using Product_Inventory_System.Models;

namespace Product_Inventory_System.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string currentFilter, int? pageNumber, string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var products = from p in _context.Products
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "category_asc":
                    products = products.OrderBy(p => p.Category);
                    ViewData["CategorySort"] = "category_desc";
                    break;
                case "category_desc":
                    products = products.OrderByDescending(p => p.Category);
                    ViewData["CategorySort"] = "category";
                    break;
                default:
                    products = products.OrderBy(p => p.ProductId);
                    ViewData["CategorySort"] = "category_asc";
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {

            if(ModelState.ErrorCount == 1)
            {
                product.AddedBy = User.Identity.Name;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
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
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Category,Price,Quantity")] Product product)
        {
            if (ModelState.ErrorCount == 1)
            {
                try
                {
                    var originalProduct = await _context.Products.FindAsync(id);

                    originalProduct.Name = product.Name;
                    originalProduct.Category = product.Category;
                    originalProduct.Price = product.Price;
                    originalProduct.Quantity = product.Quantity;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            return View(product);
        }

            private bool ProductExists(int id)
        {
            return _context.Products.Any(p => p.ProductId == id);
        }
    }

}
