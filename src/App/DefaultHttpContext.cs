using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace App
{
    // DefaultHttpContext-- ASP.net Core 默认使用的的HttpContext对象的实现
    // 作用:中间件能处理的上下文信息
    public class DefaultHttpContext : HttpContext
    {
        public IFeatureCollection HttpContextFeatures { get; }
        public DefaultHttpContext(IFeatureCollection httpContextFeatures)
        {
            this.HttpContextFeatures = httpContextFeatures;
            this.Request = new DefaultHttpRequest(this);
            this.Response = new DefaultHttpResponse(this);
        }
        public override HttpRequest Request { get; }
        public override HttpResponse Response { get; }
    }
    // 请求:获取(把原始HTTP上下文根据IHttpRequsetFeature规则)上下文封装成IHttpApplication可以处理的上下文
    public class DefaultHttpRequest : HttpRequest
    {
        public IHttpRequestFeature RequestFeature { get; }
        public DefaultHttpRequest(DefaultHttpContext context)
        {
            this.RequestFeature = context.HttpContextFeatures.Get<IHttpRequestFeature>();
        }
        public override Uri Url
        {
            get { return this.RequestFeature.Url; }
        }

        public override string PathBase
        {
            get { return this.RequestFeature.PathBase; }
        }
    }
    // 响应:获取(把原始HTTP上下文根据IHttpResponseFeature规则)上下文封装成IHttpApplication可以处理的上下文
    public class DefaultHttpResponse : HttpResponse
    {
        public IHttpResponseFeature ResponseFeature { get; }

        public override Stream OutputStream
        {
            get { return this.ResponseFeature.OutputStream; }
        }

        public override string ContentType
        {
            get { return this.ResponseFeature.ContentType; }
            set { this.ResponseFeature.ContentType = value; }
        }

        public override int StatusCode
        {
            get { return this.ResponseFeature.StatusCode; }
            set { this.ResponseFeature.StatusCode = value; }
        }

        public DefaultHttpResponse(DefaultHttpContext context)
        {
            this.ResponseFeature = context.HttpContextFeatures.Get<IHttpResponseFeature>();
        }
    }
    // IHttpRequsetFeature--原始HTTP特性需要实现的接口规则(IServer中服务器获取原始上下文之后实现并设置数据(封装)),以被DefaultHttpContext所用
    public interface IHttpRequestFeature
    {
        Uri Url { get; }
        string PathBase { get; }
    }
    // IHttpResponseFeature--原始HTTP特性需要实现的接口规则(IServer中服务器获取原始上下文之后实现并设置数据(封装)),以被DefaultHttpContext所用
    public interface IHttpResponseFeature
    {
        Stream OutputStream { get; }
        string ContentType { get; set; }
        int StatusCode { get; set; }
    }
}
