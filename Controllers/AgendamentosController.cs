using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetCare.Data;
using PetCare.Models;

namespace PetCare.Controllers
{
    public class AgendamentosController : Controller
    {
        private readonly PetCareContext _context;

        public AgendamentosController(PetCareContext context)
        {
            _context = context;
        }

        private bool UsuarioLogado()
        {
            return HttpContext.Session.GetString("UsuarioLogado") != null;
        }

        // LISTAGEM
        public async Task<IActionResult> Index()
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            var agendamentos = _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Pet)
                .Include(a => a.Servico);

            return View(await agendamentos.ToListAsync());
        }

        // CREATE GET
        public IActionResult Create()
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            ViewData["ClienteId"] =
                new SelectList(_context.Clientes, "Id", "Nome");

            ViewData["PetId"] =
                new SelectList(_context.Pets, "Id", "Nome");

            ViewData["ServicoId"] =
                new SelectList(
                    _context.Servicos.Where(s => s.Ativo),
                    "Id",
                    "Nome"
                );

            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Agendamento agendamento)
        {
            if (agendamento.DataHora < DateTime.Now)
            {
                ModelState.AddModelError("", "Data inválida.");
            }

            bool horarioExiste = _context.Agendamentos
                .Any(a => a.DataHora == agendamento.DataHora);

            if (horarioExiste)
            {
                ModelState.AddModelError("", "Horário já agendado.");
            }

            var servico = await _context.Servicos
                .FirstOrDefaultAsync(s =>
                    s.Id == agendamento.ServicoId);

            if (servico == null || !servico.Ativo)
            {
                ModelState.AddModelError("",
                    "O serviço selecionado está inativo.");
            }

            if (ModelState.IsValid)
            {
                agendamento.Status = StatusAgendamento.Pendente;

                _context.Add(agendamento);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ClienteId"] =
                new SelectList(
                    _context.Clientes,
                    "Id",
                    "Nome",
                    agendamento.ClienteId
                );

            ViewData["PetId"] =
                new SelectList(
                    _context.Pets,
                    "Id",
                    "Nome",
                    agendamento.PetId
                );

            ViewData["ServicoId"] =
                new SelectList(
                    _context.Servicos.Where(s => s.Ativo),
                    "Id",
                    "Nome",
                    agendamento.ServicoId
                );

            return View(agendamento);
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            if (id == null)
                return NotFound();

            var agendamento = await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Pet)
                .Include(a => a.Servico)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (agendamento == null)
                return NotFound();

            return View(agendamento);
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            if (id == null)
                return NotFound();

            var agendamento = await _context.Agendamentos.FindAsync(id);

            if (agendamento == null)
                return NotFound();

            ViewData["ClienteId"] =
                new SelectList(
                    _context.Clientes,
                    "Id",
                    "Nome",
                    agendamento.ClienteId
                );

            ViewData["PetId"] =
                new SelectList(
                    _context.Pets,
                    "Id",
                    "Nome",
                    agendamento.PetId
                );

            ViewData["ServicoId"] =
                new SelectList(
                    _context.Servicos.Where(s => s.Ativo),
                    "Id",
                    "Nome",
                    agendamento.ServicoId
                );

            return View(agendamento);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Agendamento agendamento)
        {
            if (id != agendamento.Id)
                return NotFound();

            bool horarioExiste = _context.Agendamentos
                .Any(a =>
                    a.Id != agendamento.Id &&
                    a.DataHora == agendamento.DataHora);

            if (horarioExiste)
            {
                ModelState.AddModelError("",
                    "Horário já agendado.");
            }

            var servico = await _context.Servicos
                .FirstOrDefaultAsync(s =>
                    s.Id == agendamento.ServicoId);

            if (servico == null || !servico.Ativo)
            {
                ModelState.AddModelError("",
                    "O serviço selecionado está inativo.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var agendamentoBanco =
                        await _context.Agendamentos
                        .AsNoTracking()
                        .FirstOrDefaultAsync(a => a.Id == id);

                    if (agendamentoBanco == null)
                        return NotFound();

                    agendamento.Status = agendamentoBanco.Status;

                    _context.Update(agendamento);

                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ClienteId"] =
                new SelectList(
                    _context.Clientes,
                    "Id",
                    "Nome",
                    agendamento.ClienteId
                );

            ViewData["PetId"] =
                new SelectList(
                    _context.Pets,
                    "Id",
                    "Nome",
                    agendamento.PetId
                );

            ViewData["ServicoId"] =
                new SelectList(
                    _context.Servicos.Where(s => s.Ativo),
                    "Id",
                    "Nome",
                    agendamento.ServicoId
                );

            return View(agendamento);
        }

        // ALTERAR STATUS GET
        public async Task<IActionResult> AlterarStatus(int? id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            if (id == null)
                return NotFound();

            var agendamento = await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Pet)
                .Include(a => a.Servico)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agendamento == null)
                return NotFound();

            return View(agendamento);
        }

        // ALTERAR STATUS POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarStatus(int id, Agendamento agendamento)
        {
            var agendamentoBanco = await _context.Agendamentos
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agendamentoBanco == null)
                return NotFound();

            agendamentoBanco.Status = agendamento.Status;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // DELETE GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            if (id == null)
                return NotFound();

            var agendamento = await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Pet)
                .Include(a => a.Servico)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (agendamento == null)
                return NotFound();

            return View(agendamento);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agendamento = await _context.Agendamentos
                .FindAsync(id);

            if (agendamento != null)
            {
                _context.Agendamentos.Remove(agendamento);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}