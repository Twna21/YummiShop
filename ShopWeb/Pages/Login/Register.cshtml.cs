using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using BusinessObject;
using System.Security.Cryptography;
using System.Text;
namespace ShopWeb.Pages.Login
{
    public class RegisterModel : PageModel
    {
        private readonly IAccountRepository _accountRepository;
        public RegisterModel(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; }

        public class RegisterInputModel
        {

            public string FullName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existAcc = _accountRepository.GetAccountByEmail(Input.Email);
            if (existAcc == null)
            {
                return Page();
            }
            var user = new Account
            {
                FullName = Input.FullName,
                Email = Input.Email,
                Status = 1,
                Type = 1,
                UserName = GenerateUsername(Input.FullName),
                Password = ComputeMD5Hash(Input.Password)
            };

            _accountRepository.Add(user);
            return RedirectToPage("/Login/Index");
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
        public static string GenerateUsername(string fullName)
        {
            string[] nameParts = fullName.Split(' ');


            string baseUsername = nameParts[nameParts.Length - 1]; // Get the third part of the name
            Random random = new Random();
            int randomInt = random.Next(1000, 9999);
            return $"{baseUsername}{randomInt}";
        }
    }
}
