using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Loader;
using System.Text;

namespace HiSFS.Core.Plugin
{
    /// <summary>
    /// 플러그인 관련 처리
    /// </summary>
    public sealed class PluginManager
    {
        private static PluginManager instance;

        private PluginManager()
        {
        }

        public static PluginManager Default
        {
            get
            {
                if (instance != null)
                    instance = new PluginManager();

                return instance;
            }
        }
    }
}
