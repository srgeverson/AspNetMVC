using AspNetMVC.Helpers;
using AspNetMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetMVC.ViewComponents
{
    public class Menu : ViewComponent
    {
        private const string USUARIO_LOGADO = "Sessao.Usuario.Logado";

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sessaoUsuarioLogado = HttpContext.Session.GetString(USUARIO_LOGADO);
            return View(new LoginViewModel() { Email = sessaoUsuarioLogado });
        }
    }
}
