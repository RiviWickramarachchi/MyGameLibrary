using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GamesLibrary.Models;
using GamesLibrary.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GamesLibrary.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IIGDBRepository _igdbRepo;
    public HomeController(ILogger<HomeController> logger, IIGDBRepository igdbRepo)
    {
        _logger = logger;
        _igdbRepo = igdbRepo;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<GameModel> topGames = await _igdbRepo.ReturnGamesAsync();

        return View(topGames);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public ActionResult SignUp() {
        ViewBag.Message = "Employee Registered";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
