using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace App
{
public interface IServerAddressesFeature
{
    ICollection<string> Addresses { get; }
}

public class ServerAddressesFeature : IServerAddressesFeature
{
    public ICollection<string> Addresses { get; } = new Collection<string>();
}
}
