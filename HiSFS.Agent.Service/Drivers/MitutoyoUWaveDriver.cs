using HiSFS.Agent.Service.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Agent.Service.Drivers
{
    public class MitutoyoUWaveDriver : IDriver
    {
        public MitutoyoUWaveDriver()
        {
        }

        public IDevice Create(string connectionString)
        {
            var result = new MitutoyoUWave();
            result.ConnectionString = connectionString;
            return result;
        }
    }
}
