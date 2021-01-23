using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TWS.Models
{
    public class MessageModel
    {
        public string Number { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Message { get; set; }

        public IFormFile file { get; set; }
    }
}
