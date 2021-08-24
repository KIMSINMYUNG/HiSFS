using System;
using System.Collections.Generic;
using System.Text;

namespace HiSFS.Rpc.Shared.Realm
{
    public interface IRealm
    {
        IRealmService Service { get; }
    }
}
