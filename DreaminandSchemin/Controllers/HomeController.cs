using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DreaminandSchemin.Models;
using DreaminandSchemin.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;


namespace DreaminandSchemin.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    //[AllowAnonymous]
    public IActionResult Index()
    {
        var mgr = new Managers.RoundRobinManager(_dbContext);
        var model = mgr.LoadTournaments();
        return View(model);
    }

    public IActionResult DivisionList(int tournamentId)
    {
        var mgr = new Managers.RoundRobinManager(_dbContext);
        var model = mgr.LoadDivisionList(tournamentId);
        return View(model);
    }

    public IActionResult Division(int tournamentId, string divisionName)
    {
        var mgr = new Managers.RoundRobinManager(_dbContext);
        var model = mgr.LoadDivisionViewModel(tournamentId, divisionName);
        return View(model);
    }

    [HttpPost]
    public ActionResult Signin(string provider, string returnUrl)
    {
        var auth = new AuthenticationProperties();
        return View();
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

