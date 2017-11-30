using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace App
{
    // HttpApplication--一组中间件的有序集合
    // 作用:IHttpApplication<TContext>对象作为参数传给IServer,来处理请求,并且响应
    public interface IHttpApplication<TContext>
    {
        // 创建上下文 TContext:每个请求建立的上下文
        TContext CreateContext(IFeatureCollection contextFeatures);
        // 释放上下文
        void DisposeContext(TContext context, Exception exception);        
        // 在上下文中处理请求
        Task ProcessRequestAsync(TContext context);
    }
    // HostingApplication--管道采用的对象
    public class HostingApplication : IHttpApplication<Context>
    {
        // RequestDelegate--所有中间件对请求的处理
        public RequestDelegate Application { get; }

        public HostingApplication(RequestDelegate application)
        {
            this.Application = application;
        }

        public Context CreateContext(IFeatureCollection contextFeatures)
        {
            HttpContext httpContext = new DefaultHttpContext(contextFeatures);
            return new Context
            {
                HttpContext = httpContext,
                StartTimestamp = Stopwatch.GetTimestamp()
            };
        }
        // Application(context.HttpContext) 启动中间件处理  "委托和管道理解" 中的 app("刘哲");
        public Task ProcessRequestAsync(Context context) => this.Application(context.HttpContext);
        public void DisposeContext(Context context, Exception exception) => context.Scope?.Dispose();

    }
    // Context--针对当前请求的上下文对象(封装后,管道可用的上下文)
    public class Context
    {
        // 真正描述当前HTTP请求的上下文
        public HttpContext HttpContext { get; set; }
        // 针对同一请求的多次日记记录关联到同一个上下文范围
        public IDisposable Scope { get; set; }
        // 处理请求的时间戳
        public long StartTimestamp { get; set; }
    }
}
