using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PetCare.Models;
using PetCare.Data;

namespace PetCare.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PetCareContext _context;

    public HomeController(
        ILogger<HomeController> logger,
        PetCareContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("UsuarioLogado") == null)
        {
            return RedirectToAction("Index", "Login");
        }

        ViewBag.TotalClientes =
            _context.Clientes.Count();

        ViewBag.TotalPets =
            _context.Pets.Count();

        ViewBag.TotalServicos =
            _context.Servicos.Count();

        ViewBag.TotalAgendamentos =
            _context.Agendamentos.Count();

        ViewBag.AgendamentosHoje =
            _context.Agendamentos.Count(a =>
                a.DataHora.Date == DateTime.Today);

        ViewBag.AgendamentosPendentes =
            _context.Agendamentos.Count(a =>
                a.Status == StatusAgendamento.Pendente);

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ??
                           HttpContext.TraceIdentifier
            });
    }
}