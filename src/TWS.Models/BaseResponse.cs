using System;
using System.Collections.Generic;
using System.Text;

namespace TWS.Models
{
    public abstract class BaseResponse
    {
        public int Code { get; protected set; }

        public string Message { get; protected set; }

        public BaseResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
