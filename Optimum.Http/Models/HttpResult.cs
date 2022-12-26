using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Optimum.Http.Models
{
    public class HttpResult <T>
    {
        public bool IsSuccess { get; set; }
        public bool IsOk { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public T? Data { get; set; } = default(T?);
        public string? Message { get; set; }
    }
}
