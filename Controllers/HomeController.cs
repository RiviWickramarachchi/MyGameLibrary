using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GamesLibrary.Models;
using GamesLibrary.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using GamesLibrary.DTOs;
using GamesLibrary.Security;
using Microsoft.AspNetCore.Identity;

namespace GamesLibrary.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IIGDBRepository _igdbRepo;
    private readonly IUsersRepository _iuserRepo;

    public HomeController(ILogger<HomeController> logger, IIGDBRepository igdbRepo, IUsersRepository iuserRepo)
    {
        _logger = logger;
        _igdbRepo = igdbRepo;
        _iuserRepo = iuserRepo;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<GameModel> topGames = await _igdbRepo.ReturnGamesAsync();
        return View(topGames);

    }

    [HttpPost]
    public async Task<IActionResult> Index([FromForm]string gameName) {
        if(gameName != null)
        {
            GameModel game = await _igdbRepo.SearchForGameAsync(gameName);
            ViewData["Game"] = game;
            return View();
        }
        else {
            return RedirectToAction("Index");
        }
    }


    public IActionResult Privacy()
    {
        return View();
    }

    //GET all users
    //Test Route
    public IEnumerable<UserDTO> GetUsers() {
        var users = _iuserRepo.GetUsers().Select(user => user.ReturnAsDTO());
        return users;
    }


    //GET to items/{id}
    //Action result lets the user return more than one type. Can be used to return the status code if the user is not found
    //or an error occurs
    public ActionResult<UserDTO> GetUser(Guid id) {
        var user = _iuserRepo.GetUser(id);

        if(user == null) {
            return NotFound(); //returns a 404 error
        }
        return user.ReturnAsDTO();
    }

    public ActionResult AddGame()
    {
        //check if the user is logged in
        //if logged in add the game to games list
        //else redirect to login page
        return View("Login");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
