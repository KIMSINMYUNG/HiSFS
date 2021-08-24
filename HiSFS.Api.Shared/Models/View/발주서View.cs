using System;
using System.Collections.Generic;
using System.Text;

namespace HiSFS.Api.Shared.Models.View
{
    public class 발주서View
    {
        public IEnumerable<발주서헤더정보> 빌주서헤더정보 { get; set; }
        public IEnumerable<발주서정보> 발주서정보 { get; set; }
    }
}
