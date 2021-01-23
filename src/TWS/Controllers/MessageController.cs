using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TelegramWebService.Common;
using TWS.Models;
using TWS.Services;

namespace TWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ITLSharpTelegramClient _client;
        public MessageController(ITLSharpTelegramClient client)
        {
            _client = client?? throw new ArgumentNullException(nameof(_client));
        }

        public IActionResult GetAsync()
        {
            return Ok("Ok");
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SendMessageModel model) 
        {
            if (ModelState.IsValid)
            {
                var result = await _client.SendMessage(model); 

                return Ok(result);
            }

            return BadRequest(ModelState.GetErrorMessages());
        }
    }
}
