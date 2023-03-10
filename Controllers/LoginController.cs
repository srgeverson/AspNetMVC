using AspNetMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AspNetMVC.Helpers;
using AppClassLibraryDomain.service;

namespace AspNetMVC.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ISessaoUsuarioHelper _sessaoUsuarioHelper;
        private readonly IUsuarioService _usuarioService;
        private readonly ISistemaService _sistemaService;

        public LoginController(
            ILogger<LoginController> logger, 
            ISessaoUsuarioHelper sessaoUsuarioHelper,
            IUsuarioService usuarioService,
            ISistemaService sistemaService)
        {
            _logger = logger;
            _sessaoUsuarioHelper = sessaoUsuarioHelper;
            _usuarioService = usuarioService;
            _sistemaService = sistemaService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (_sistemaService.Sistema("") == null)
                return RedirectToAction("Error", "Login");
            else
            {
                if (_sessaoUsuarioHelper.BuscarSessao() == null)
                    return View();
                else
                    return RedirectToAction("Index", "Home");
            }
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
        public async Task<IActionResult> Logar(LoginViewModel loginViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuario = _usuarioService.BuscarPorEmail(loginViewModel.Email);
                    
                    if (usuario == null)
                        throw new ArgumentNullException("Usuário ou senha inválido");

                    loginViewModel.Nome= usuario.Nome;

                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, loginViewModel.Email) };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authenticationProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authenticationProperties);

                    if (loginViewModel != null && loginViewModel.Email != null && loginViewModel.Email.Equals("geversonjosedesouza@gmail.com"))
                    {
                        _sessaoUsuarioHelper.CriarSessao(loginViewModel);
                        TempData["MensagemSucesso"] = String.Format("Usuário logado com sucesso!");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["MensagemAlerta"] = String.Format("Usuário ou senha inválidos!");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["MensagemAlerta"] = String.Format("Usuário e senha são obrigatórios!");
                    return RedirectToAction("Index");
                }
            }
            catch (ArgumentNullException aex)
            {
                TempData["MensagemAlerta"] = aex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new ErrorViewModel() { RequestId = ex.Message });
            }
        }
        public async Task<IActionResult> Sair()
        {
            _sessaoUsuarioHelper.RemoverSessao();
            await HttpContext.SignOutAsync();
            TempData["MensagemSucesso"] = "Secessão encerrada!";
            return View("Index");
        }
    }
}