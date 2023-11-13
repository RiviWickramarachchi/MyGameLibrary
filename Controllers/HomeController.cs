using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GamesLibrary.Models;
using GamesLibrary.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using GamesLibrary.DTOs;
using GamesLibrary.Security;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GamesLibrary.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IIGDBRepository _igdbRepo;
    private readonly IUsersRepository _iuserRepo;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public HomeController(ILogger<HomeController> logger, IIGDBRepository igdbRepo, IUsersRepository iuserRepo, SignInManager<ApplicationUser> signInManager)
    {
        _logger = logger;
        _igdbRepo = igdbRepo;
        _iuserRepo = iuserRepo;
        _signInManager = signInManager;
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
            IEnumerable<GameModel> games = await _igdbRepo.SearchForGameAsync(gameName);
            ViewData["Games"] = games; //viewdata should be changed
            return View();
        }
        else {
            return RedirectToAction("Index");
        }
    }


    public IActionResult About()
    {
        return View();
    }

    public async Task<IActionResult> MyGames()
    {
        if(_signInManager.IsSignedIn(User))
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if(userEmail != null)
            {
                UserModel user = await _iuserRepo.SearchUserByEmailAsync(userEmail);
                IEnumerable<GameModel> gamesList = user.Games;
                if(gamesList != null)
                {
                    return View(gamesList);
                }
                else
                {
                    ViewData["GamesListNullMsg"] = "You havent added any games to your library yet.";
                }
            }
            return View();
        }
        else
        {
            return RedirectToAction("Login","UserAccount");
        }
    }

    //GET all users
    //Test Route
    public async Task<IEnumerable<UserDTO>> GetUsersAsync() {
        var users = await _iuserRepo.GetUsersAsync();
        return users.Select(user => user.ReturnAsDTO());;
    }


    //GET to items/{id}
    //Action result lets the user return more than one type. Can be used to return the status code if the user is not found
    //or an error occurs
    public async Task<ActionResult<UserDTO>> GetUserAsync(Guid id) {
        var user = await _iuserRepo.GetUserAsync(id);

        if(user == null) {
            return NotFound(); //returns a 404 error
        }
        return user.ReturnAsDTO();
    }

    public async Task<ActionResult> AddGame(string gameId, string gameName, string rating, string description, string imageUrl)
    {
        //check if the user is logged in
        if(_signInManager.IsSignedIn(User))
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if(userEmail != null)
            {
                UserModel user = await _iuserRepo.SearchUserByEmailAsync(userEmail);
                double gameRating = Convert.ToDouble(rating);
                GameModel gameModel = new() {
                    GameID = gameId,
                    GameName = gameName,
                    Description = description,
                    Rating = gameRating,
                    ImgUrl = imageUrl
                };
                foreach(GameModel game in user.Games)
                {
                    if(game.GameID == gameModel.GameID)
                    {
                        //notify that the game is already added
                        TempData["ErrorMessage"] = "The game already exists in your list.";
                        return RedirectToAction("Index");
                    }
                }
                user.Games.Add(gameModel);
                await _iuserRepo.AddGameToListAsync(user);
            }
            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Login","UserAccount");
        }
        //if logged in add the game to games list
        //else redirect to login page
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
