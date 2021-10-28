using CarparkWebAPI.Models;
using CarparkWebAPI.Service;
using CarparkWebAPI.ViewModels;
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
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CarparkWebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, ITokenService tokenService, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _tokenService = tokenService;
            _clientFactory = clientFactory;
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
        [HttpGet]
        public async Task<IActionResult> Carpark(CarparkViewModel model)
        {
            if (ModelState.IsValid) {
                var httpClient = _clientFactory.CreateClient();
                var req = new HttpRequestMessage(HttpMethod.Get, "https://api.data.gov.sg/v1/transport/carpark-availability");
                HttpResponseMessage res = await httpClient.SendAsync(req);
                if (res.IsSuccessStatusCode)
                {
                    model = await res.Content.ReadFromJsonAsync<CarparkViewModel>();
                }
                else
                {
                    ModelState.AddModelError("", "Failed to fetch data");
                }
            } else
            {
                ModelState.AddModelError(string.Empty, "Unauthorized");
            }
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
