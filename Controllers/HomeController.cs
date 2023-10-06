using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GamesLibrary.Models;
using GamesLibrary.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using GamesLibrary.DTOs;
using GamesLibrary.Security;

namespace GamesLibrary.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IIGDBRepository _igdbRepo;
    private readonly IUsersRepository _iuserRepo;
    private readonly IPasswordHasher _ipasshasher;
    public HomeController(ILogger<HomeController> logger, IIGDBRepository igdbRepo, IUsersRepository iuserRepo, IPasswordHasher ipasshasher)
    {
        _logger = logger;
        _igdbRepo = igdbRepo;
        _iuserRepo = iuserRepo;
        _ipasshasher = ipasshasher;
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

    //GET all users
    //Test Route
    public IEnumerable<UserDTO> GetUsers() {
        var users = _iuserRepo.GetUsers().Select(user => user.ReturnAsDTO());
        return users;
    }

    public IActionResult Login() {
        return View();
    }

    [HttpPost]
    public ActionResult<string> Login([FromBody] LoginDTO loginVals) {

        string inputEmail = loginVals.Email;
        string inputPassword = loginVals.Password;
        //Make a post request to send email and password
        //Search for email in mongo Database and get user
        var user = _iuserRepo.SearchUserByEmail(inputEmail);

        //Encrypt password
        bool verification = _ipasshasher.Verify(user.EncryptedPassword,inputPassword);

        //Validate
        if(user != null) {
            if(verification)
                return "Password matches";
        }
        return "Incorrect email or password";
        //return View("Index");
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

    [HttpPost]
    public ActionResult CreateUser([FromForm] CreateUserDTO userDTO) {
        UserModel user = new() {
            Id = Guid.NewGuid(),
            UserName = userDTO.UserName,
            Email = userDTO.Email,
#nullable disable
            EncryptedPassword = _ipasshasher.HashPassword(userDTO.Password),
#nullable enable
            CreatedDate = DateTimeOffset.UtcNow,
            Games = new List<GameModel>()
        };

        _iuserRepo.CreateUser(user);
        //return CreatedAtAction(nameof(GetUser), new{id = user.Id,}, user.ReturnAsDTO());
        return RedirectToAction("Index"); //Redirect to home page
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
