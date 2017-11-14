using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App
{
    public interface IApplicationBuilder
    {
        RequestDelegate Build();
        IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware);
    }

    public class ApplicationBuilder : IApplicationBuilder
    {
        private IList<Func<RequestDelegate, RequestDelegate>> middlewares = new List<Func<RequestDelegate, RequestDelegate>>();

        public RequestDelegate Build()
        {
            RequestDelegate seed = context => Task.Run(() => { });
            return middlewares.Reverse().Aggregate(seed, (next, current) => current(next));
        }

        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            middlewares.Add(middleware);
            return this;
        }
    }
}
