using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Authentication.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public CredentialModel Credential { get; set; }

        public void OnGet() 
        { 
            // this will happen when the page is being loaded in
            //this.credential = new CredentialModel { Username = "admin" };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // this happens when something is posted from the page 
            if (!ModelState.IsValid) return Page();

            // verify the credential
            if (Credential.Username == "admin" && Credential.Password == "password")
            {
                // Create the security context || READ MORE ABOUT THIS!
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                    new Claim("Department", "HR"),
                    new Claim("Admin", "true"),
                    new Claim("Manager", "true"),
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                return RedirectToPage("/index");
            }

            return Page();
        }
    }

    public class CredentialModel
    {
        [Required]
        [Display(Name = " User name")]
        public string Username { get; set; }
        [Required] public string Password { get; set; }

    }
}
