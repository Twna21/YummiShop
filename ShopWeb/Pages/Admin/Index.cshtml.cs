using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
namespace ShopWeb.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IAccountRepository accountRepository = new AccountRepository();
        public async Task<IActionResult> OnGetAsync()
        {
            var id = HttpContext.Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int uid))
            {
                return RedirectToPage("/Error");
            }
            var user = await accountRepository.GetAccountById(uid);
            if (user.Type == 1)
            {
                return RedirectToPage("/dashboard");
            }
            return Page();

        }
    }
}
