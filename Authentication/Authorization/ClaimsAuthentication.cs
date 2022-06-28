using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication.Authorization
{
    public class ClaimsAuthentication : PageModel
    {
        public List<Claim> claims { get; set; }

        public bool _error; 
        

        public ClaimsAuthentication(string name, string password)
        {
            // set it to false at the start
            _error = false;

            if(!CheckCredential(name, password))
            {
                _error = true;
                return;
            };

            if(name == "admin" && password == "password"){
                claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                        new Claim("Department", "HR"),
                        new Claim("Admin", "true"),
                        new Claim("Manager", "true"),
                        new Claim("EmploymentDate", "2021-05-01"),
                };

                return;
            };

            if (name == "user" && password == "password")
            {
                claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, "user"),
                        new Claim("User", "true"),
                };

                return;
            };
        }
        bool CheckCredential(string name, string password)
        {
            // check to see if its in DB if so continue
            if (name == "admin" && password == "password" || name == "user" && password == "password")
            {
                return true;
            } 
            else
            {
                return false;
            }
        }
    }
}
