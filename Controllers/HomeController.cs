using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GamesLibrary.Models;
using GamesLibrary.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using GamesLibrary.DTOs;

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

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Register() {

        return View();
    }
    /*
    public ActionResult<UserDTO> CreateUser(CreateUserDTO userDTO) {
        UserModel user = new() {
            Id = Guid.NewGuid(),
            UserName = userDTO.UserName,
            Email = userDTO.Email,
            EncryptedPassword = userDTO.Password,
            CreatedDate = DateTimeOffset.UtcNow,
            Games = new List<GameModel>()
        };

        _iuserRepo.CreateUser(user);
    }
    */
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
