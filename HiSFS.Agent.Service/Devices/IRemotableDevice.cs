using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HiSFS.Api.Shared.Client;

namespace HiSFS.Agent.Service.Devices
{
    public interface IRemotableDevice : IDevice
    {
        IRemoteCommand Command { set; }
        bool IsRemoteReady { set; }
    }
}
