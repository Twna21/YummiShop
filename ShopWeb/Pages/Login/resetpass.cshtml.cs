using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Repository;
using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Pages.Login
{
    public class resetpassModel : PageModel
    {

        private IAccountRepository _accountRepository;


        public resetpassModel(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter your new password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        // This method will be called when the page is accessed via GET request
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            _accountRepository.ResetPassword(Email, NewPassword);

            return RedirectToPage("/Login/PasswordResetConfirmation");
        }
    }
}
