using CarparkWebAPI.Models;
using CarparkWebAPI.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CarparkWebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, ITokenService tokenService)
        {
            _logger = logger;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public IActionResult Index()
        {
            string token = HttpContext.Session.GetString("JWToken");
            if (token != null)
            {
                if (!_tokenService.ValidateToken(_configuration["JWT:Secret"], token))
                {
                    ViewBag.Message = "Unable to validate token";
                } else
                {
                    ViewBag.Message = token;
                }
            } 
            else
            {
                ViewBag.Message = "Expired - relogin to refresh jwt token";
            }
            return View();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
