using AspNetMVC.Models;

namespace AspNetMVC.Helpers
{
    public interface ISessaoUsuarioHelper
    {
        LoginViewModel BuscarSessao();
        void CriarSessao(LoginViewModel loginViewModel);
        void RemoverSessao();
    }
}
