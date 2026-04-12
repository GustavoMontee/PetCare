using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetCare.Data;
using PetCare.Models;

namespace PetCare.Controllers
{
    public class PetController : Controller
    {
        private readonly PetCareContext _context;

        public PetController(PetCareContext context)
        {
            _context = context;
        }

        //Verifica se está logado
        private bool UsuarioLogado()
        {
            return HttpContext.Session.GetString("UsuarioLogado") != null;
        }

        // LISTAGEM
        public async Task<IActionResult> Index()
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            var pets = _context.Pets.Include(p => p.Cliente);
            return View(await pets.ToListAsync());
        }

        // CREATE (GET)
        public IActionResult Create(int? clienteId)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome", clienteId);
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pet pet)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                _context.Pets.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome", pet.ClienteId);
            return View(pet);
        }

        // EDIT (GET)
        public async Task<IActionResult> Edit(int id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            var pet = await _context.Pets.FindAsync(id);
            if (pet == null) return NotFound();

            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome", pet.ClienteId);
            return View(pet);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pet pet)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            if (id != pet.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome", pet.ClienteId);
            return View(pet);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            var pet = await _context.Pets
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pet == null) return NotFound();

            return View(pet);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}