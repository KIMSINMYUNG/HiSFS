using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Agent.Service.Devices
{
    public interface IDevice : IDisposable
    {
        public bool IsConnected { get; }

        public string DeviceName { get; }
        public string ConnectionString { get; set; }

        public void Open();
        public void Close();
    }
}
