using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWeb.Pages.Admin
{
    public class AddProductModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public AddProductModel(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public Product Product { get; set; } = new Product();

        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();

        public async Task<IActionResult> OnGetAsync()
        {
            Categories = await _categoryRepository.GetCategoryAll();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categories = await _categoryRepository.GetCategoryAll();
                return Page();
            }

            try
            {
                await _productRepository.Add(Product);
                return RedirectToPage("/Admin/ProductManager");
            }
            catch (Exception ex)
            {
                // Log the exception (ex) for debugging
                ModelState.AddModelError(string.Empty, "An error occurred while adding the product. Please try again.");
                Categories = await _categoryRepository.GetCategoryAll();
                return Page();
            }
        }
    }
}
