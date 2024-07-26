using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using Repository;

namespace ShopWeb.Pages.Users
{
    public class EditProfileModel : PageModel
    {
        private readonly IAccountRepository _accountRepository = new AccountRepository();

        public EditProfileModel()
        {
        }

        [BindProperty]
        public Account UserAccount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var id = HttpContext.Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int uid))
            {
                return RedirectToPage("/Error");
            }

            UserAccount = await _accountRepository.GetAccountById(uid);
            Console.WriteLine(UserAccount.AccountID);
            if (UserAccount == null)
            {
                return RedirectToPage("/Error");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Console.WriteLine(UserAccount);
            await _accountRepository.Update(UserAccount);

            return RedirectToPage("/Users/EditProfile");
        }
    }
}
