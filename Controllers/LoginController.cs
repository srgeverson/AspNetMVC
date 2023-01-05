using AspNetMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNetMVC.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(String mensagem)
        {
            TempData["MensagemAlerta"] = mensagem;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public void Logar(UsuarioLoginModel usuarioLoginModel)
        {
            if (usuarioLoginModel != null && usuarioLoginModel.Email != null && usuarioLoginModel.Email.Equals("geversonjosedesouza@gmail.com"))
            {
                ViewData["UsuarioAutenticado"] = usuarioLoginModel;
                Response.Redirect("/Home");
            }
            else
                Error(String.Format("Usuário ou senha inválidos!"));
        }
    }
}