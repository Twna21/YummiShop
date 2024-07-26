using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Repository;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ShopWeb.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductRepository _productRepository;
   
        public IndexModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
         
        }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<Product> PagedProducts { get; set; } = new List<Product>();
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }
        public string SearchString { get; set; } = string.Empty;

        private const int PageSize = 6;

     
        public async Task OnGetAsync(int pageIndex = 1, string searchString = "")
        {
            HeaderModelView.UserName = HttpContext.Request.Cookies["UserName"];

            if (HeaderModelView.UserName == null)
            {
                HeaderModelView.UserName = "";
            }
            ViewData["UserName"] = HeaderModelView.UserName.ToString();
            SearchString = searchString.Trim();

            var allProducts = await _productRepository.GetProducts();
            if (!string.IsNullOrEmpty(SearchString))
            {
                allProducts = allProducts.Where(p => p.ProductName.Contains(SearchString, StringComparison.OrdinalIgnoreCase));
            }

            TotalPages = (int)Math.Ceiling(allProducts.Count() / (double)PageSize);
            PageIndex = pageIndex;
            PagedProducts = allProducts.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            var product = await _productRepository.GetProductById(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.GetString("Cart");
            var cartItems = string.IsNullOrEmpty(cart)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cart);

            var cartItem = cartItems.FirstOrDefault(ci => ci.ProductId == product.ProductId);
            if (cartItem == null)
            {
                cartItems.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice,
                    Quantity = 1,
                    Image = product.ProductImage
                });

                TempData["SuccessMessage"] = "Item added to cart successfully!";
            }
            else
            {
                cartItem.Quantity++;
                TempData["SuccessMessage"] = "Quantity updated in cart!";
            }

            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cartItems));

            return RedirectToPage(new { pageIndex = PageIndex, searchString = SearchString });
        }

        public class CartItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string Image { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
        }

        public class HeaderModel
        {
            public string? UserName { get; set; }
            public string? Type { get; set; }
            public string? WelcomeMessage { get; set; }
        }

        public HeaderModel? HeaderModelView = new HeaderModel();
    }
}
