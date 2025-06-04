using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MotorsportApi.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _config;
        public string? ErrorMessage { get; set; }

        public LoginModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if ((Username == "admin" && Password == "admin") || (Username == "manager" && Password == "manager"))
            {
                var role = Username == "admin" ? "Admin" : "RaceManager";
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Username),
                new Claim(ClaimTypes.Role, role)
            };

                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToPage("/Index");
            }

            ErrorMessage = "Invalid login";
            return Page();
        }
    }
}
