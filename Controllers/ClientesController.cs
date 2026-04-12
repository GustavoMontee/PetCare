using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCare.Data;
using PetCare.Models;

namespace PetCare.Controllers
{
    public class ClientesController : Controller
    {
        private readonly PetCareContext _context;

        public ClientesController(PetCareContext context)
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

            var clientes = _context.Clientes.ToList();
            return View(clientes);
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
        public IActionResult Create(Cliente cliente)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        // EDIT (GET)
        public IActionResult Edit(int id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            var cliente = _context.Clientes
                .Include(c => c.Pets)
                .FirstOrDefault(c => c.Id == id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Cliente cliente)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                _context.Clientes.Update(cliente);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            var cliente = _context.Clientes.Find(id);

            if (cliente == null)
                return NotFound();

            _context.Clientes.Remove(cliente);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}