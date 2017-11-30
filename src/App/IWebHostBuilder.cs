using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace App
{
    //WebHost的创建者和提供服务数据
    //作用:管道创建过程中所需的所有信息都来源于作为创建者的WebHostBuilder,采用“依赖注入”的形式来为创建的WebHost提供这些信息
    public interface IWebHostBuilder
    {
        IWebHost Build();
        IWebHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);
        IWebHostBuilder UseSetting(string key, string value);
    }

    public class WebHostBuilder : IWebHostBuilder
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _config;

        public WebHostBuilder()
        {
            // 用于注册中间件 创建管道服务 (依赖注入:注入)
            _services = new ServiceCollection().AddSingleton<IApplicationBuilder, ApplicationBuilder>();
            // 自定义配置
            _config = new ConfigurationBuilder().AddInMemoryCollection().Build();
        }
        // IWebHostBuilder负责创建IWebHost
        public IWebHost Build() => new WebHost(_services, _config);

        public IWebHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            // 也就是调用委托的实现方法,比如:services => services.AddSingleton<IServer, HttpListenerServer>();
            configureServices(_services);
            return this;
        }

        public IWebHostBuilder UseSetting(string key, string value)
        {
            _config[key] = value;
            return this;
        }
      
    }    
}
