using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace App
{
    public interface IHttpApplication<TContext>
    {
        TContext CreateContext(IFeatureCollection contextFeatures);
        void DisposeContext(TContext context, Exception exception);
        Task ProcessRequestAsync(TContext context);
    }

    public class HostingApplication : IHttpApplication<Context>
    {
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

        public void DisposeContext(Context context, Exception exception)=> context.Scope?.Dispose();
        public Task ProcessRequestAsync(Context context)=> this.Application(context.HttpContext);
    }

    public class Context
    {
        public HttpContext HttpContext { get; set; }
        public IDisposable Scope { get; set; }
        public long StartTimestamp { get; set; }
    }
}
