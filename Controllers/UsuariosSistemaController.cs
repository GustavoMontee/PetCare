using Microsoft.AspNetCore.Mvc;
using PetCare.Data;
using PetCare.Models;
using System.Linq;

namespace PetCare.Controllers
{
    public class UsuariosSistemaController : Controller
    {
        private readonly PetCareContext _context;

        public UsuariosSistemaController(PetCareContext context)
        {
            _context = context;
        }

        //Verifica se está logado
        private bool UsuarioLogado()
        {
            return HttpContext.Session.GetString("UsuarioLogado") != null;
        }

        // LISTAGEM
        public IActionResult Index()
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            var usuarios = _context.UsuariosSistema.ToList();
            return View(usuarios);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UsuarioSistema usuario)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                _context.UsuariosSistema.Add(usuario);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // EDIT (GET)
        public IActionResult Edit(int id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            var usuario = _context.UsuariosSistema.Find(id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UsuarioSistema usuario)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                _context.UsuariosSistema.Update(usuario);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            var usuario = _context.UsuariosSistema.Find(id);

            if (usuario == null)
                return NotFound();

            _context.UsuariosSistema.Remove(usuario);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}