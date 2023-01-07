using System.ComponentModel.DataAnnotations;

namespace AspNetMVC.Models
{
    public class LoginViewModel
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Senha { get; set; }
    }
}
