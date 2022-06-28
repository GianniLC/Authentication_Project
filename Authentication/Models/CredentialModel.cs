using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class CredentialModel
    {
        [Required]
        [Display(Name = " User name")]
        public string Username { get; set; }
        [Required] public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe;
    }
}