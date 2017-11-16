using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace App
{
    // IServerAddressesFeature--服务器监听地址集合
    public interface IServerAddressesFeature
    {
        ICollection<string> Addresses { get; }
    }

    public class ServerAddressesFeature : IServerAddressesFeature
    {
        public ICollection<string> Addresses { get; } = new Collection<string>();
    }
}
