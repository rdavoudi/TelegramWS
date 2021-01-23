using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TWS.Models
{
    public class SendMessageModel
    {
        [Required(ErrorMessage = "Required")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Message { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }

        public byte[] uploadFile { get; set; }

        public bool IsImage { get; set; }
    }
}
