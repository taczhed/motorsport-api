using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MotorsportApi.Web.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            Response.Cookies.Delete(".AspNetCore.Cookies");
            return RedirectToPage("/Login");
        }
    }
}
