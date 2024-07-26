using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ShopWeb.Pages
{
    public class IndexModel : PageModel
    {


        public IndexModel(ILogger<IndexModel> logger)
        {
            HeaderModelView = new HeaderModel();
        }

        public class HeaderModel
        {
            public string? UserName { get; set; }
            public string? Type { get; set; }
            public string? WelcomeMessage { get; set; }
        }

        public HeaderModel? HeaderModelView = new HeaderModel();

        public void OnGet()
        {
            HeaderModelView.UserName = HttpContext.Request.Cookies["UserName"];

            if (HeaderModelView.UserName == null)
            {
                HeaderModelView.UserName = "";
            }
            ViewData["UserName"] = HeaderModelView.UserName.ToString();
        }
    }
}
