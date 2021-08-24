using HiSFS.Shared.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Mes.Plugin
{
    public class PluginFactory : IPluginFactory
    {
        public T Create<T>() where T : class, new()
        {
            throw new NotImplementedException();
        }
    }
}
