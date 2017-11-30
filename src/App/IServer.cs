using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace App
{
    // IServer--管道中的服务器,实现对请求的监听、接收和响应
    public interface IServer
    {
        // 开始启动服务器监听、接收和响应
        void Start<TContext>(IHttpApplication<TContext> application);
        // 需要的特性集合(主要是监听地址集合)
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
                // 添加需要监听的URL范围至listener.Prefixes中，可以通过如下函数实现： listener.Prefixes.Add(prefix)  prefix必须以'/'结尾
                this.Listener.Prefixes.Add(address.TrimEnd('/') + "/");
            }

            this.Listener.Start();
            while (true)
            {
                // 获取服务器接受到请求的原始上下文
                HttpListenerContext httpListenerContext = this.Listener.GetContext(); // 这里代码会停止等待请求的到来才继续
                // 把原始上下文封装成 也就是封装成特性,通过FeatureCollection做连接的桥梁
                HttpListenerContextFeature feature = new HttpListenerContextFeature(httpListenerContext, this.Listener);
                // 设置封装后的请求和响应,以便DefaultHttpContext再次封装成管道能处理的http上下文
                IFeatureCollection contextFeatures = new FeatureCollection()
                    .Set<IHttpRequestFeature>(feature)
                    .Set<IHttpResponseFeature>(feature);
                // 创建管道能处理的上下文
                TContext context = application.CreateContext(contextFeatures);

                // 在上下文中处理请求
                application.ProcessRequestAsync(context)
                    .ContinueWith(_ =>
                    {
                        httpListenerContext.Response.Close(); // 这个地方执行完,浏览器才真正请求结束,小圈不转了
                    })
                    .ContinueWith(_ =>
                    {
                        application.DisposeContext(context, _.Exception);// 最后释放 资源
                    });
            }
        }
    }
    // HttpListenerContextFeature--表示"原始HTTP上下文"封装后的特性,是一个纽带作用
    // 作用:HostingApplication 可以利用这个HttpListenerContextFeature 对象创建DefaultHttpContext对象
    public class HttpListenerContextFeature : IHttpRequestFeature, IHttpResponseFeature
    {
        private readonly HttpListenerContext context;
        public HttpListenerContextFeature(HttpListenerContext context, HttpListener listener)
        {
            this.context = context;
            this.Url = context.Request.Url;// 给实现接口IHttpRequestFeature的属性赋值,之后DefaultHttpContext对象就等通过获取这个接口的属性值了
            this.OutputStream = context.Response.OutputStream;
            this.PathBase = (from it in listener.Prefixes
                             let pathBase = new Uri(it).LocalPath.TrimEnd('/')
                             where context.Request.Url.LocalPath.StartsWith(pathBase, StringComparison.OrdinalIgnoreCase)
                             select pathBase).First();
        }

        public string ContentType
        {
            get { return context.Response.ContentType; }
            set
            {
                context.Response.ContentType = value;
            }
        }

        public int StatusCode
        {
            get { return context.Response.StatusCode; }
            set
            {
                context.Response.StatusCode = value;
            }
        }

        public Stream OutputStream { get; }
        public Uri Url { get; }
        public string PathBase { get; }
    }
}
