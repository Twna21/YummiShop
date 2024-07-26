using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using Repository.MailService;
using Repository.MailUtils;

namespace ShopWeb.Pages.Login
{
    public class forgotPassModel : PageModel
    {

        private readonly IOtpService _otpService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISendGmailService _sendGmailService;


        public forgotPassModel(IOtpService otpService, UserManager<IdentityUser> userManager, ISendGmailService sendGmailService)
        {
            _otpService = otpService;
            _userManager = userManager;
            _sendGmailService = sendGmailService;
        }

        [BindProperty]
        public string Email { get; set; }

        public void OnGet()
        {
        }

        public  IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _otpService.GenerateOtp(Email, out var otp);

            MailContent content = new MailContent
            {
                To = Email,
                Subject = "OTP - YUMMISHOP",
                Body = $"<h2>Mail from YumiShop</h2>" +
                $"Your OTP is: {otp}"
            };

            _sendGmailService.SendMail(content);

            return RedirectToPage("/Login/CheckOtp", new { email = Email });
        }
    }
}
