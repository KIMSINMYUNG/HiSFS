using HiSFS.Agent.Service.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Agent.Service.Drivers
{
    public interface IDriver
    {
        IDevice Create(string connectionString);
    }
}
