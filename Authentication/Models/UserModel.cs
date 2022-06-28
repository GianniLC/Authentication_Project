using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class UserModel
    {
        [Required]
        public int ID { get; set; }

        public string? username { get; set; }
        public string? password { get; set; }
        
    }
}
