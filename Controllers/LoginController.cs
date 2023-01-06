using AspNetMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task Logar(UsuarioLoginModel usuarioLoginModel)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, Guid.NewGuid().ToString()) };

            var claimsIdentity = new ClaimsIdentity(
              claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(
              CookieAuthenticationDefaults.AuthenticationScheme,
              new ClaimsPrincipal(claimsIdentity),
              authProperties);
            
            Response.Redirect("/Home");
          //  return View("/Home");

            //if (usuarioLoginModel != null && usuarioLoginModel.Email != null && usuarioLoginModel.Email.Equals("geversonjosedesouza@gmail.com"))
            //{
            //    ViewData["UsuarioAutenticado"] = usuarioLoginModel;
            //    Response.Redirect("/Home");
            //}
            //else
            //    Error(String.Format("Usuário ou senha inválidos!"));
        }
        public async Task Sair()
        {
            await HttpContext.SignOutAsync();
            Response.Redirect("/Login");
            //return View("Acessar");
        }
    }
}