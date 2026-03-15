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

        private bool UsuarioNaoLogado()
        {
            return HttpContext.Session.GetString("UsuarioLogado") == null;
        }

        public IActionResult Index()
        {
            if (UsuarioNaoLogado())
                return RedirectToAction("Index", "Login");

            var usuarios = _context.UsuariosSistema.ToList();
            return View(usuarios);
        }

        public IActionResult Create()
        {
            if (UsuarioNaoLogado())
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public IActionResult Create(UsuarioSistema usuario)
        {
            if (UsuarioNaoLogado())
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                _context.UsuariosSistema.Add(usuario);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        public IActionResult Edit(int id)
        {
            var usuario = _context.UsuariosSistema.Find(id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        [HttpPost]
        public IActionResult Edit(UsuarioSistema usuario)
        {
            if (ModelState.IsValid)
            {
                _context.UsuariosSistema.Update(usuario);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        public IActionResult Delete(int id)
        {
            var usuario = _context.UsuariosSistema.Find(id);

            if (usuario == null)
                return NotFound();

            _context.UsuariosSistema.Remove(usuario);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}