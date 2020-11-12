using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ServiciosWeb.WebApi.Communications.Responses
{
    public class ResponseStatus
    {
        public HttpStatusCode HttpCode { get; set; }

        public string Message { get; set; }
    }
}