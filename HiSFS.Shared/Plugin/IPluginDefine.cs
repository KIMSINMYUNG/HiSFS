using System;
using System.Collections.Generic;
using System.Text;

namespace HiSFS.Shared.Plugin
{
    /// <summary>
    /// 플러그인 정의 인터페이스
    /// 플러그인은 이 인터페이스로 
    /// </summary>
    public interface IPluginDefine
    {
        string Prefix { get; }

        string Name { get; }

        IPluginFactory GetPluginFactory();
    }
}
