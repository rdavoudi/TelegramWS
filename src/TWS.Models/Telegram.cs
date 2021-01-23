using System;
using System.Collections.Generic;
using System.Text;

namespace TWS.Models
{
    public class Telegram
    {
        public int ApiId { get; set; }

        public string ApiHash { get; set; }

        public string NumberToAuthenticate { get; set; }

        public string PasswordToAuthenticate { get; set; }
    }
}
