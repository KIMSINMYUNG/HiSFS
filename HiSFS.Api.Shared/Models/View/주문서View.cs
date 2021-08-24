using System;
using System.Collections.Generic;
using System.Text;

namespace HiSFS.Api.Shared.Models.View
{
    public class 주문서View
    {
        public IEnumerable<주문서헤더정보> 주문서헤더정보 { get; set; }
        public IEnumerable<주문서정보> 주문서정보 { get; set; }
    }
}
