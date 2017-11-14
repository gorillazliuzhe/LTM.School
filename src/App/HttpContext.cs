using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace App
{
    public abstract class HttpContext
    {
        public abstract HttpRequest Request { get; }
        public abstract HttpResponse Response { get; }
    }

    public abstract class HttpRequest
    {
        public abstract Uri Url { get; }
        public abstract string PathBase { get; }
    }

    public abstract class HttpResponse
    {
        public abstract Stream OutputStream { get; }
        public abstract string ContentType { get; set; }
        public abstract int StatusCode { get; set; }
    }
}
