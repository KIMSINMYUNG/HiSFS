using HiSFS.Shared.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Mes.Plugin
{
    public class PluginDefine : IPluginDefine
    {
        public string Prefix => nameof(HiSFS.Api);

        public string Name => nameof(Mes);

        public IPluginFactory GetPluginFactory()
        {
            return new PluginFactory();
        }
    }
}
