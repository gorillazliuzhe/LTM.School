using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace App
{
    // IWebHost--Web应用宿主
    // 作用:ASP.NET Core管道是由作为应用宿主的WebHost对象创建出来的(实际是IApplicationBuilder 创建管道,WebHost只是调用创建方法)
    public interface IWebHost
    {
        void Start();
    }

    public class WebHost : IWebHost
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;

        public WebHost(IServiceCollection services, IConfiguration config)
        {
            // 依赖注入:构建容器
            _serviceProvider = services.BuildServiceProvider();
            this._config = config;
        }

        public void Start()
        {
            /* 参考 委托和管道理解 中的分解方法 */

            // 依赖注入:解析 根据 注入的类型 获取服务,  "委托和管道理解" 中的 Middleware middware = new Middleware();
            IApplicationBuilder applicationBuilder = _serviceProvider.GetRequiredService<IApplicationBuilder>();
            // 调用IApplicationBuilder Use方法注册中间件. "委托和管道理解" 中的 ds.Configure(middware);
            _serviceProvider.GetRequiredService<IStartup>().Configure(applicationBuilder);

            // 获取解析的服务会调用 实现IServer接口的 HttpListenerServer 类的构造函数,相当于new. 会启动HttpListener 和设置监听地址
            IServer server = _serviceProvider.GetRequiredService<IServer>();

            // 给设置的地址特性 赋值
            IServerAddressesFeature addressFeatures = server.Features.Get<IServerAddressesFeature>();
            string addresses = _config["ServerAddresses"] ?? "http://localhost:5000";
            foreach (string address in addresses.Split(';'))
            {
                addressFeatures.Addresses.Add(address);
            }
            // 服务器Start方法开始的时候,管道已经构建完成了,需要的是等待http的请求到来,封装好上下文之后调用管道
            // applicationBuilder.Build()是属于返回第一个中间件,相当于返回了所有中间件对httpcontext的处理过程
            // "委托和管道理解" 中的 RequsetDelegate app = middware.Build();
            server.Start(new HostingApplication(applicationBuilder.Build()));
        }
    }
}
