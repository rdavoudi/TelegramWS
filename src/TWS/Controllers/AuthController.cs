using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TWS.Models;
using TWS.Services;

namespace TWS.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ITLSharpTelegramClient _client;
        public AuthController(ITLSharpTelegramClient client)
        {
            _client = client?? throw new ArgumentNullException(nameof(_client));
        }

        public async Task<IActionResult> Index()
        {
            var hash = await _client.GetAuthenticationCode();

            if (string.IsNullOrWhiteSpace(hash))
                return BadRequest("There is a problem");

            return View("Index", hash);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string hash, string code)
        {
            var result = await _client.SetAuthenticationCode(hash, code);

            if (result)
                return Json("Registration Successfully");
            else
                return Json("Regiatration Failed");
        }

        
    }
}
