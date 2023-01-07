using System.ComponentModel.DataAnnotations;

namespace AspNetMVC.Models
{
    public class LoginViewModel
    {
        public string? Nome { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Senha { get; set; }
    }
}
