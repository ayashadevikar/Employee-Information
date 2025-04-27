using System.ComponentModel.DataAnnotations;

namespace FullStackCrud.Server.Models
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
