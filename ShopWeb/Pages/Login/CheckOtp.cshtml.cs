using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace ShopWeb.Pages.Login
{

    public class CheckOtpModel : PageModel
    {
        private readonly IOtpService _otpService;

        public CheckOtpModel(IOtpService otpService)
        {
            _otpService = otpService;
        }

        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        [BindProperty]
        public string Otp { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (_otpService.ValidateOtp(Email, Otp))
            {
                return RedirectToPage("/Login/resetpass", new { email = Email });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid OTP.");
                return Page();
            }
        }
    }
}

