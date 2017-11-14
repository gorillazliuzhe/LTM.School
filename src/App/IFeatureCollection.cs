using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App
{
    public interface IFeatureCollection
    {
        TFeature Get<TFeature>();
        IFeatureCollection Set<TFeature>(TFeature instance);
    }

    public class FeatureCollection : IFeatureCollection
    {
        private readonly Dictionary<Type, object> features = new Dictionary<Type, object>();
        public TFeature Get<TFeature>()
        {
            object feature;
            return features.TryGetValue(typeof(TFeature), out feature)
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
