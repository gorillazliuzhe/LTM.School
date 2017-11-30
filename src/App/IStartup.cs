using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App
{
    // IStarup--从事启动时的初始化工作  
    // 作用:  利用ApplicationBuilder注册中间件
    public interface IStartup
    {
        void Configure(IApplicationBuilder app);
    }
    // "委托和管道理解" 中的 DelegateStartup 类
    public class DelegateStartup : IStartup
    {
        // Func<string, string> f = HelloEnglish;//实现委托 
        private Action<IApplicationBuilder> configure;
        public DelegateStartup(Action<IApplicationBuilder> configure)
        {
            this.configure = configure;
        }

        public void Configure(IApplicationBuilder app)
        {
            // 相当于调用Middle 方法
            // string s= f("Srping ji")//调用委托,实际是调用委托的实现方法
            this.configure(app);
        }
    }
}
