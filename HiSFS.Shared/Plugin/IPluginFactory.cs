using System;
using System.Collections.Generic;
using System.Text;

namespace HiSFS.Shared.Plugin
{
    public interface IPluginFactory
    {
        T Create<T>()
            where T : class, new();
    }
}
