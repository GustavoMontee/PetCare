using Microsoft.AspNetCore.Mvc;
using PetCare.Data;
using PetCare.Models;
using System.Linq;

namespace PetCare.Controllers
{
    public class ClientesController : Controller
    {
        private readonly PetCareContext _context;

        public ClientesController(PetCareContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UsuarioLogado") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var clientes = _context.Clientes.ToList();
            return View(clientes);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UsuarioLogado") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        public IActionResult Edit(int id)
        {
            var cliente = _context.Clientes.Find(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost]
        public IActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Clientes.Update(cliente);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        public IActionResult Delete(int id)
        {
            var cliente = _context.Clientes.Find(id);

            if (cliente == null)
                return NotFound();

            _context.Clientes.Remove(cliente);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}