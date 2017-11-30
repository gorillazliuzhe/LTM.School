using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace App
{
    // HttpContext--对当前HTTP上下文的抽象,DefaultHttpContext是HttpContext的实现
    public abstract class HttpContext
    {
        public abstract HttpRequest Request { get; }
        public abstract HttpResponse Response { get; }
    }
    // 表示请求
    public abstract class HttpRequest
    {
        public abstract Uri Url { get; }
        public abstract string PathBase { get; }
    }
    // 表示响应
    public abstract class HttpResponse
    {
        public abstract Stream OutputStream { get; }
        public abstract string ContentType { get; set; }
        public abstract int StatusCode { get; set; }
    }
}
