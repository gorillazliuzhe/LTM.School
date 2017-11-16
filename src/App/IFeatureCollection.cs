using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App
{
    // IFeatureCollection--服务器原始上下文的特性集合
    // 作用:其实是一种描述规则的载体,可以描述服务器原始上下文的特性集合和监听地址等
    public interface IFeatureCollection
    {
        // 根据类型获取特性描述
        TFeature Get<TFeature>();
        // 设置类型及其描述
        IFeatureCollection Set<TFeature>(TFeature instance);
    }

    public class FeatureCollection : IFeatureCollection
    {
        private readonly Dictionary<Type, object> features = new Dictionary<Type, object>();
        public TFeature Get<TFeature>()
        {
            return features.TryGetValue(typeof(TFeature), out object feature)
                ? (TFeature)feature
                : default(TFeature);
        }

        public IFeatureCollection Set<TFeature>(TFeature instance)
        {
            features[typeof(TFeature)] = instance;
            return this;
        }
    }
}
