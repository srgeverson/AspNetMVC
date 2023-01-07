using AspNetMVC.Models;
using Microsoft.AspNetCore.Http;

namespace AspNetMVC.Helpers
{
    public class SessaoUsuarioHelper : ISessaoUsuarioHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string USUARIO_LOGADO = "Sessao.Usuario.Logado";

        public SessaoUsuarioHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public LoginViewModel BuscarSessao()
        {
            var usuario = _httpContextAccessor.HttpContext.Session.GetString(USUARIO_LOGADO);
            if (String.IsNullOrEmpty(usuario))
                return null;
            else
                return new LoginViewModel() { Email = usuario };
        }

        public void CriarSessao(LoginViewModel loginViewModel)
        {
            _httpContextAccessor.HttpContext.Session.SetString(USUARIO_LOGADO, loginViewModel.Email);
        }

        public void RemoverSessao()
        {
            _httpContextAccessor.HttpContext.Session.Remove(USUARIO_LOGADO);
        }
    }
}
