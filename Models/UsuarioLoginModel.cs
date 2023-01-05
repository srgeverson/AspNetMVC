namespace AspNetMVC.Models
{
    public class UsuarioLoginModel
    {
        private string _email;
        private string _senha;
        public string Email { get => _email; set => _email = value; }
        public string Senha { get => _senha; set => _senha = value; }
    }
}
