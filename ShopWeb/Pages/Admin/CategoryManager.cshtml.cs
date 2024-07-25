using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWeb.Pages.Admin
{
    public class CategoryManagerModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryManagerModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public IList<Category> Categories { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public async Task OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                Categories = (await _categoryRepository.GetCategoryAll()).ToList();
            }
            else
            {
                // Implement search functionality if needed
                Categories = (await _categoryRepository.GetCategoryAll())
                    .Where(c => c.CategoryName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        public async Task<IActionResult> OnGetDeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryRepository.Delete(id);

            return RedirectToPage("./CategoryManager");
        }
    }
}
