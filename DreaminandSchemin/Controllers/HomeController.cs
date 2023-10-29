using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DreaminandSchemin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;


namespace DreaminandSchemin.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    //[AllowAnonymous]
    public IActionResult Index()
    {
        return View();
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

