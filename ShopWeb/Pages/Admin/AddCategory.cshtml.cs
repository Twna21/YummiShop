using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using Repository; // Adjust this namespace based on your project structure
using System.Threading.Tasks;

namespace ShopWeb.Pages.Admin
{
    public class AddCategoryModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public AddCategoryModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public Category Category { get; set; }

        public void OnGet()
        {
            // Initialize the Category property if needed
            Category = new Category();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Save the new category to the repository
                await _categoryRepository.Add(Category);
                TempData["SuccessMessage"] = "Category added successfully.";
                return RedirectToPage("/Admin/CategoryManager"); // Redirect to the category list or another relevant page
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here) and return to the page with an error message
                ModelState.AddModelError(string.Empty, "An error occurred while adding the category.");
                return Page();
            }
        }
    }
}
