using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace HiSFS.Core.Plugin
{
    public class PluginLoadContext : AssemblyLoadContext
    {
        protected override Assembly Load(AssemblyName assemblyName)
        {
            //this.Unload
            // Microsoft.NET.Sdk
            return null;
        }
    }
}
