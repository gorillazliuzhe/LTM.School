using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App
{
    // RequestDelegate--所有中间件对请求的处理
    public delegate Task RequestDelegate(HttpContext context);
}
