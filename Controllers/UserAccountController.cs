using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GamesLibrary.DTOs;
using GamesLibrary.Models;
using GamesLibrary.Repositories;
using GamesLibrary.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GamesLibrary.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly ILogger<UserAccountController> _logger;
        private readonly IUsersRepository _iuserRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public UserAccountController(ILogger<UserAccountController> logger, IUsersRepository iuserRepo, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _iuserRepo = iuserRepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromForm] LoginDTO loginVals) {

            if(ModelState.IsValid)
            {
                ApplicationUser appUser = await _userManager.FindByEmailAsync(loginVals.Email);
                if(appUser != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(appUser, loginVals.Password, false, false);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ViewData["ErrorMessage"] = "Incorrect Username or Password";
                }
            }
            return View(loginVals);
        }

        public IActionResult Register() {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromForm] CreateUserDTO userDTO) {
        //search for the email address
        //if theres already a user with the same email address, display error message
        if(userDTO.Email != null)
        {
            var existingUser = _iuserRepo.SearchUserByEmail(userDTO.Email);
            if(existingUser == null)
            {
                if(ModelState.IsValid)
                {
                    ApplicationUser appUser = new () {
                        Email = userDTO.Email,
                        UserName = userDTO.UserName
                    };

                    //adding user login credentials to applicationUsers collection
                    IdentityResult result = await _userManager.CreateAsync(appUser, userDTO.Password);

                    if(result.Succeeded)
                    {
                        //adding user details to the users collections
                        UserModel user = new() {
                        Id = Guid.NewGuid(),
                        UserName = userDTO.UserName,
                        Email = userDTO.Email,
                        CreatedDate = DateTimeOffset.UtcNow,
                        Games = new List<GameModel>()
                        };

                        _iuserRepo.CreateUser(user);
                        return RedirectToAction("Index","Home"); //Redirect to home page
                    }
                    else
                    {
                        string errorMsg = "";
                        foreach (var error in result.Errors)
                        {
                            if (error.Code == "PasswordRequiresUpper")
                            {
                            // Handle the error due to missing upper-case letter in the password.
                            errorMsg += " Password Requires uppercase letters.";
                            }
                            if (error.Code == "PasswordRequiresDigit")
                            {
                            // Handle the error due to missing numeric character in the password.
                                errorMsg += " Password Requires Numeric Characters.";
                            }
                            if(error.Code == "PasswordRequiresNonAlphanumeric")
                            {
                                errorMsg += " Password Requires Alphanumeric characters.";
                            }
                        }
                        ViewData["ErrorMessage"] = errorMsg;
                        return View("Register");
                    }
                }
            }
        }
        ViewData["ErrorMessage"] = "A user with this email address already exist.";
        return View("Register");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}