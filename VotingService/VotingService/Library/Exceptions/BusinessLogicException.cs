using System;
using System.Net;

namespace VotingService.Library.Exception
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
