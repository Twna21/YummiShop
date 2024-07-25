using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopWeb.Pages.Accounts
{
    public class AccountManagerModel : PageModel
    {
        private readonly IAccountRepository _accountRepository;

        public AccountManagerModel(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [BindProperty]
        public IList<Account> Accounts { get; set; }



        public async Task OnGetAsync()
        {

            Accounts = (await _accountRepository.GetAccounts()).ToList();

        }

        public async Task<IActionResult> OnGetDeleteAccountAsync(int id)
        {
            var account = await _accountRepository.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }

            await _accountRepository.Delete(account);

            return RedirectToPage("./AccountManager");
        }
    }
}
