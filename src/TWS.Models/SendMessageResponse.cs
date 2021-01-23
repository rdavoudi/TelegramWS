using System;
using System.Collections.Generic;
using System.Text;

namespace TWS.Models
{
    public class SendMessageResponse : BaseResponse
    {
        public SendMessageModel SendMessage { get; private set; }

        public SendMessageResponse(int code, string message, SendMessageModel sendMessage) : base(code, message)
        {
            SendMessage = sendMessage;
        }

        public SendMessageResponse(SendMessageModel sendMessage) : this(200, "Sending Message Successfully", sendMessage)
        {

        }

        public SendMessageResponse(string message) : this(500, message, null)
        {

        }




    }
}
