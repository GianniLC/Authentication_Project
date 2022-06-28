using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Authentication.Models;
using Authentication.Authorization;

namespace Authentication.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public CredentialModel Credential { get; set; }
        private ClaimsAuthentication _claimsAuthentication;

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


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
            ClaimsAuthentication claims = new ClaimsAuthentication(Credential.Username, Credential.Password);

            var identity = new ClaimsIdentity(claims.claims, "GHE56S85HF647SNE7GLX72NH69DT35LD537Z");

            if (claims._error) return Page();

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            // do we need persistent cookies?
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = Credential.RememberMe
            };

            await HttpContext.SignInAsync("GHE56S85HF647SNE7GLX72NH69DT35LD537Z", claimsPrincipal, authProperties);

            return RedirectToPage("/index");
        }
    }
}
