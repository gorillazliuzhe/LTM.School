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
            RequestDelegate seed = context => Task.Run(() => { });
            //return middlewares.Reverse().Aggregate(seed, (next, current) => current(next));
            //更好的理解
            foreach (var compoent in middlewares.Reverse())
            {
                seed = compoent(seed);
            }
            return seed;
        }

        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            middlewares.Add(middleware);
            return this;
        }
    }
}
