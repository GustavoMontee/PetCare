using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCare.Data;
using PetCare.Models;

namespace PetCare.Controllers
{
    public class ServicosController : Controller
    {
        private readonly PetCareContext _context;

        public ServicosController(PetCareContext context)
        {
            _context = context;
        }

        private bool UsuarioLogado()
        {
            return HttpContext.Session.GetString("UsuarioLogado") != null;
        }

        // LISTAGEM
        public IActionResult Index()
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            var servicos = _context.Servicos.ToList();

            return View(servicos);
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            if (id == null)
                return NotFound();

            var servico = await _context.Servicos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (servico == null)
                return NotFound();

            return View(servico);
        }

        // CREATE GET
        public IActionResult Create()
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Servico servico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(servico);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(servico);
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            if (id == null)
                return NotFound();

            var servico = await _context.Servicos.FindAsync(id);

            if (servico == null)
                return NotFound();

            return View(servico);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Servico servico)
        {
            if (id != servico.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servico);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Servicos.Any(e => e.Id == servico.Id))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(servico);
        }

        // DELETE GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            if (id == null)
                return NotFound();

            var servico = await _context.Servicos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (servico == null)
                return NotFound();

            return View(servico);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);

            if (servico != null)
            {
                _context.Servicos.Remove(servico);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}