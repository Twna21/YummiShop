using BusinessObject;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Repository;
using System.Security.Cryptography;
using System.Text;


namespace ShopWeb.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IAccountRepository _accountRepository;
        public IndexModel(SignInManager<IdentityUser> signInManager, IAccountRepository accountRepository)
        {
            _signInManager = signInManager;
            _accountRepository = accountRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }


        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }


            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }


        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }




        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);
                    SetUserCookies(user);
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }
            return Page();
        }


        public async Task<IActionResult> OnPostDbLoginAsync()
        {

            var user = await _accountRepository.GetAccountByEmail(Input.Email);
            if (user == null)
            {
                ErrorMessage = "Invalid login attempt";
                return Page();
            }


            var hashedInputPassword = ComputeMD5Hash(Input.Password);

            if (Input.Email == user.Email && hashedInputPassword == user.Password)
            {
                SetUserCookies(new IdentityUser { Email = Input.Email, Id = user.AccountID.ToString(), UserName = user.FullName.ToString() });
                if (user.Type == 1)
                {
                    return RedirectToPage("/index");
                }
                else
                {
                    return RedirectToPage("/Admin/index");
                }
            }

            else
            {
                ErrorMessage = "Invalid login attempt";
                return Page();
            }

        }


        public IActionResult OnPostExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Page("./Index", pageHandler: "ExternalLoginCallback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("/Login/index", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("/Login/index", new { ReturnUrl = returnUrl });
            }
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var fullName = info.Principal.FindFirstValue(ClaimTypes.Name);

            var picture = info.Principal.FindFirstValue("picture");
            if (email != null)
            {
                Account existingUser = await _accountRepository.GetAccountByEmail(email);

                var user = new Account
                {
                    Password = "",
                    Photo = picture,
                    Status = 1,
                    Type = 1,
                    FullName = fullName,
                    Email = email,
                    UserName = GenerateUsername(fullName),
                };
                if (existingUser == null)
                {
                    SetUserCookies(new IdentityUser { Email = email, Id = user.AccountID.ToString(), UserName = user.FullName.ToString() });
                    await _accountRepository.Add(user);
                    return RedirectToPage("/index", new { ReturnUrl = returnUrl });

                }
                SetUserCookies(new IdentityUser { Email = email, Id = existingUser.AccountID.ToString(), UserName = existingUser.FullName.ToString() });
                if (existingUser.Type == 1)
                {
                    return RedirectToPage("/index", new { ReturnUrl = returnUrl });
                }
                else
                {
                    return RedirectToPage("/Admin/index", new { ReturnUrl = returnUrl });
                }
            }
            else
            {
                ErrorMessage = "Error creating user account.";
                return RedirectToPage("/Login/index", new { ReturnUrl = returnUrl });
            }
        }

        public static string GenerateUsername(string fullName)
        {
            string[] nameParts = fullName.Split(' ');


            string baseUsername = nameParts[nameParts.Length - 1]; // Get the third part of the name
            Random random = new Random();
            int randomInt = random.Next(1000, 9999);
            return $"{baseUsername}{randomInt}";
        }

        private void SetUserCookies(IdentityUser user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            };

            HttpContext.Response.Cookies.Append("UserEmail", user.Email, cookieOptions);
            HttpContext.Response.Cookies.Append("UserId", user.Id, cookieOptions);
            HttpContext.Response.Cookies.Append("UserName", user.UserName, cookieOptions);
        }


        private string ComputeMD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }




    }
}
