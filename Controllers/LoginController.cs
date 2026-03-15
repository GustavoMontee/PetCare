using Microsoft.AspNetCore.Mvc;
using PetCare.Data;
using System.Linq;

namespace PetCare.Controllers
{
    public class LoginController : Controller
    {
        private readonly PetCareContext _context;

        public LoginController(PetCareContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string senha)
        {
            var usuario = _context.UsuariosSistema
                .FirstOrDefault(u => u.Email == email && u.Senha == senha);

            if (usuario != null)
            {
                HttpContext.Session.SetString("UsuarioLogado", usuario.Email);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Erro = "Usuário ou senha inválidos";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}