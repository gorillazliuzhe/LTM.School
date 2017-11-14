using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace App
{
public interface IServer
{
    void Start<TContext>(IHttpApplication<TContext> application);
    IFeatureCollection Features { get; }
}

public class HttpListenerServer : IServer
{
    public HttpListener Listener { get; }
    public IFeatureCollection Features { get; }

    public HttpListenerServer()
    {
        this.Listener = new HttpListener();
        this.Features = new FeatureCollection().Set<IServerAddressesFeature>(new ServerAddressesFeature());
    }

    public void Start<TContext>(IHttpApplication<TContext> application)
    {
        IServerAddressesFeature addressFeatures = this.Features.Get<IServerAddressesFeature>();
        foreach (string address in addressFeatures.Addresses)
        {
            this.Listener.Prefixes.Add(address.TrimEnd('/') + "/");
        }

        this.Listener.Start();
        while (true)
        {
            HttpListenerContext httpListenerContext = this.Listener.GetContext();

            HttpListenerContextFeature feature = new HttpListenerContextFeature(httpListenerContext, this.Listener);
            IFeatureCollection contextFeatures = new FeatureCollection()
                .Set<IHttpRequestFeature>(feature)
                .Set<IHttpResponseFeature>(feature);
            TContext context = application.CreateContext(contextFeatures);

            application.ProcessRequestAsync(context)
                .ContinueWith(_ => httpListenerContext.Response.Close())
                .ContinueWith(_ => application.DisposeContext(context, _.Exception));
        }
    }
}

public class HttpListenerContextFeature : IHttpRequestFeature, IHttpResponseFeature
{
    private readonly HttpListenerContext context;
    public HttpListenerContextFeature(HttpListenerContext context, HttpListener listener)
    {
        this.context = context;
        this.Url = context.Request.Url;
        this.OutputStream = context.Response.OutputStream;
        this.PathBase = (from it in listener.Prefixes
                            let pathBase = new Uri(it).LocalPath.TrimEnd('/')
                            where context.Request.Url.LocalPath.StartsWith(pathBase, StringComparison.OrdinalIgnoreCase)
                            select pathBase).First();
    }
    public string ContentType
    {
        get { return context.Response.ContentType; }
        set { context.Response.ContentType = value; }
    }

    public Stream OutputStream { get; }

    public int StatusCode
    {
        get { return context.Response.StatusCode; }
        set { context.Response.StatusCode = value; }
    }

    public Uri Url { get; }
    public string PathBase { get; }
}
}
