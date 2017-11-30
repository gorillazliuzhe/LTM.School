using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App
{
    // IApplicationBuilder--用于注册中间件并创建管道
    public interface IApplicationBuilder
    {
        RequestDelegate Build();
        IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware);
    }

    public class ApplicationBuilder : IApplicationBuilder
    {
        private IList<Func<RequestDelegate, RequestDelegate>> middlewares = new List<Func<RequestDelegate, RequestDelegate>>();

        // 构建管道,返回一组中间件对http上下文处理的结果(形成方法链),返回的是第一个注册的中间件
        public RequestDelegate Build()
        {
            //RequestDelegate seed = context => Task.Run(() => { });
            //return middlewares.Reverse().Aggregate(seed, (next, current) => current(next));

            // 更好的理解 参考:http://www.cnblogs.com/durow/p/5736385.html
            // 没有任何中间操作返回状态码404
            RequestDelegate app = context =>
            {
                context.Response.StatusCode = 404;
                return Task.CompletedTask;
            };
            // 为了保证中间件的顺序需要先Reverse()
            foreach (var component in middlewares.Reverse())
            {
                // 把当前中间件app作为next传入当前component,传入的是下一个component(中间件方法),返回当前中间件方法
                // 返回的中间件存入app,然后作为next(下一个)传入下一个component
                app = component(app); // 会调用 Func<RequestDelegate, RequestDelegate> middleware = next =>{...},{...}不会调用.这时候next就赋值,并返回实现此RequestDelegate的方法,相当于"委托和管道理解" 中的 mi1
            }
            // 最后返回第一个中间件,可以选择是否调用第二个中间件
            return app;
        }

        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            middlewares.Add(middleware);
            return this;
        }
    }
}
