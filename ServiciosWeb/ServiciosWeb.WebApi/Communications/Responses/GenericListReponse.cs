using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiciosWeb.WebApi.Communications.Responses
{
    public class GenericListResponse<T>
    {
        public ResponseStatus Status { get; set; }
         public IList<T> Items { get; set; }
    }
}