using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using Repository; // Ensure this namespace matches your actual project structure
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ShopWeb.Pages.Admin
{
    public class ProductManagerModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly IHubContext<SignalRServer> _signalRHub;
        public ProductManagerModel(IProductRepository productRepository, IHubContext<SignalRServer> signalRHub)
        {
            _productRepository = productRepository;
            _signalRHub = signalRHub;
        }

        [BindProperty]
        public IList<Product> Products { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public async Task<IActionResult> OnGetGetProductAsync()
        {
            var res = _productRepository.GetProducts();
            return new JsonResult(res);
        }


        public async Task OnGetAsync()
        {
            if (string.IsNullOrEmpty(SearchTerm))
            {
                Products = (await _productRepository.GetProducts()).ToList();
            }
            else
            {
                Products = (await _productRepository.GetProducts())
                    .Where(p => p.ProductId.ToString().Contains(SearchTerm) ||
                                p.ProductName.Contains(SearchTerm))
                    .ToList();
            }
        }

        public async Task<IActionResult> OnGetDeleteProductAsync(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.Delete(product);
            await _signalRHub.Clients.All.SendAsync("LoadProducts");

            return RedirectToPage("./ProductManager");
        }
    }
}
