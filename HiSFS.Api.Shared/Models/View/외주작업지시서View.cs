using System;
using System.Collections.Generic;
using System.Text;

namespace HiSFS.Api.Shared.Models.View
{
    public class 외주작업지시서View
    {
        public IEnumerable<외주작업지시헤더정보> 외주작업지시헤더정보 { get; set; }
        public IEnumerable<외주작업지시서정보> 외주작업지시서정보 { get; set; }
    }
}
