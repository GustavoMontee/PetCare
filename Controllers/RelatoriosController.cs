using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCare.Data;
using PetCare.Models;

namespace PetCare.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly PetCareContext _context;

        public RelatoriosController(PetCareContext context)
        {
            _context = context;
        }

        private bool UsuarioLogado()
        {
            return HttpContext.Session.GetString("UsuarioLogado") != null;
        }

        // RELATÓRIO DE AGENDAMENTOS
        public IActionResult Agendamentos(
            string? status,
            DateTime? dataInicial,
            DateTime? dataFinal)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            var query = _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Pet)
                .Include(a => a.Servico)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<StatusAgendamento>(
                    status,
                    out var statusEnum))
                {
                    query = query.Where(a =>
                        a.Status == statusEnum);
                }
            }

            if (dataInicial.HasValue)
            {
                query = query.Where(a =>
                    a.DataHora >= dataInicial.Value);
            }

            if (dataFinal.HasValue)
            {
                var fim = dataFinal.Value.Date.AddDays(1);

                query = query.Where(a =>
                    a.DataHora < fim);
            }

            ViewBag.Status = status;
            ViewBag.DataInicial = dataInicial;
            ViewBag.DataFinal = dataFinal;

            return View(
                query.OrderByDescending(a => a.DataHora)
                     .ToList());
        }

        // RELATÓRIO DE SERVIÇOS
        public IActionResult Servicos()
        {
            if (!UsuarioLogado())
                return RedirectToAction("Index", "Login");

            var dados = _context.Agendamentos
                .Include(a => a.Servico)
                .ToList()
                .GroupBy(a => a.Servico!.Nome)
                .Select(g => new RelatorioServicoViewModel
                {
                    NomeServico = g.Key,
                    Quantidade = g.Count()
                })
                .OrderByDescending(x => x.Quantidade)
                .ToList();

            return View(dados);
        }
        
    }
}