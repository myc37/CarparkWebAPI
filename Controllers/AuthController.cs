using CarparkWebAPI.DbContext;
using CarparkWebAPI.Models;
using CarparkWebAPI.Service;
using CarparkWebAPI.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarparkWebAPI.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private string token = null;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Contact
                };

                var res = await _userManager.CreateAsync(user, model.Password);
                if (res.Succeeded)
                {
                    // await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in res.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        token = _tokenService.BuildToken(_configuration["JWT:Secret"], user);
                        if (token != null)
                        {
                            HttpContext.Session.SetString("JWToken", token);
                            return RedirectToAction("index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to generate JWT token");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Unregistered Email Address");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> AccountDetails(AccountDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    model = new AccountDetailsViewModel
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Contact = user.PhoneNumber,
                    };
                }
                else
                {
                    ModelState.AddModelError("", "Could not find User");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unauthorized");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private object JwtSecurityTokenHandler()
        {
            throw new NotImplementedException();
        }
    }
}

