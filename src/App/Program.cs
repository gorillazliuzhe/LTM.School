
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region core1.1
            new WebHostBuilder() // 准备阶段： var builder = new WebHostBuilder()，然后给WebHostBuilder各种填参数
              .UseHttpListener()
              .UseUrls("http://localhost:3721/images")
              .Configure(app =>
              {
                  app.UseImages(@"D:\Picture");
                  app.Use(next => // ApplicationBuilder build的时候给next赋值
                    {
                        return async context => // 这块是 服务器接受到请求 上一个中间件处理请求之后调用此方法给context 赋值,之后这个方法执行完成返回上一方法最后一行
                        {
                            await Task.Run(() => Console.WriteLine("刘哲"));
                            await next(context);
                        };
                    });
              })
              .Build() // 构建阶段： var host = builder.Builder() ，主要负责依懒注入的初始化，以及host的初始化
              .Start(); // 启动阶段： host.start(); 之后是 运行阶段
            #endregion

            #region core 2.0
            //BuildWebHost(args).Run();
            #endregion

            Console.Read();
        }

        public static IWebHost BuildWebHost(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
                .Configure(app =>app.UseImages(@"D:\Picture"))
                .Build();
    }
    //扩展类,中间件的方法可以写在这里
    public static class Extensions
    {
        private static Dictionary<string, string> mediaTypeMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        static Extensions()
        {
            mediaTypeMappings.Add(".jpg", "image/jpeg");
            mediaTypeMappings.Add(".gif", "image/gif");
            mediaTypeMappings.Add(".png", "image/png");
            mediaTypeMappings.Add(".bmp", "image/bmp");
        }

        public static IWebHostBuilder UseHttpListener(this IWebHostBuilder builder)
        {
            return builder.ConfigureServices(services =>
            {
                services.AddSingleton<IServer, HttpListenerServer>();
            });
        }
        public static IWebHostBuilder UseUrls(this IWebHostBuilder builder, params string[] urls)
        {
            string addresses = string.Join(";", urls);
            return builder.UseSetting("ServerAddresses", addresses);
        }

        public static IWebHostBuilder Configure(this IWebHostBuilder builder, Action<IApplicationBuilder> configure)
        {
            // "委托和管道理解" 中的  DelegateStartup ds = new DelegateStartup(Middle); 注入直接赋值
            return builder.ConfigureServices(services => services.AddSingleton<IStartup>(new DelegateStartup(configure)));
        }
        /// <summary>
        /// 自己实现的中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="rootDirectory"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseImages(this IApplicationBuilder app, string rootDirectory)
        {
            //Func<RequestDelegate, RequestDelegate> context1 = mi1;mi1相当于实现这个委托的方法,也就是next之后的内容
            // next在build的时候给赋值了
            Func<RequestDelegate, RequestDelegate> middleware = next =>
            {
                // 返回多个中间件对请求的上下文处理  相当于 "委托和管道理解" 中的t1方法,是mi1的具体实现方法.Application(context.HttpContext) 调用此方法
                return async context =>
                {
                    // 根据URL解析出来图片物理路径
                    string filePath = context.Request.Url.LocalPath.Substring(context.Request.PathBase.Length + 1);
                    filePath = Path.Combine(rootDirectory, filePath).Replace('/', Path.DirectorySeparatorChar);
                    filePath = File.Exists(filePath)
                      ? filePath
                      : Directory.GetFiles(Path.GetDirectoryName(filePath))
                      .FirstOrDefault(it => string.Compare(Path.GetFileNameWithoutExtension(it), Path.GetFileName(filePath), true) == 0);

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        string extension = Path.GetExtension(filePath);
                        if (mediaTypeMappings.TryGetValue(extension, out string mediaType))
                        {
                            await context.Response.WriteFileAsync(filePath, mediaType);
                        }
                    }
                    await next(context);// 启动下一个中间件,处理完成后,返回此方法,最后返回多个中间件对请求的上下文 处理
                }; // 所有的中间件执行完成,会返回到第一个中间件 此程序就是这里
            };

            return app.Use(middleware); // 把中间件方法前面添加到中间件集合 [_serviceProvider.GetRequiredService<IStartup>().Configure(applicationBuilder);会调用这个地方]
        }
        public static async Task WriteFileAsync(this HttpResponse response, string fileName, string contentType)
        {
            if (File.Exists(fileName))
            {
                byte[] content = File.ReadAllBytes(fileName);
                response.ContentType = contentType;
                await response.OutputStream.WriteAsync(content, 0, content.Length); // 这个地方响应流有数据,浏览器就显示图片了.但是还没有请求完,小圈还转
            }
            //response.StatusCode = 404;
            response.StatusCode = 200;
        }
    }
}
