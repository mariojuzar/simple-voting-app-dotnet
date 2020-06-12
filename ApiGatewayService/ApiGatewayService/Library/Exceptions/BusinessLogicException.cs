﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApiGatewayService.Library.Exceptions
{
    public class BusinessLogicException : SystemException
    {
        public HttpStatusCode Code { get; set; }

        public String ErrorMessage { get; set; }

        public BusinessLogicException(HttpStatusCode code, String Message)
        {
            this.Code = code;
            this.ErrorMessage = Message;
        }
    }
}
