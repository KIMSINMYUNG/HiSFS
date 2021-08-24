using System;
using System.Collections.Generic;
using System.Text;

namespace HiSFS.Api.Shared.Models.Parameters
{
    public class 검색정보 : Dictionary<검색대상, string>
    {
        public bool 유무(검색대상 검색대상)
        {
            return this.ContainsKey(검색대상);
        }
    }

    public enum 검색대상
    {
        사용,
        상태
    }
}
