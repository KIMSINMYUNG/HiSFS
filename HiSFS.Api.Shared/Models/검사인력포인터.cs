using System;
using System.Collections.Generic;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 검사인력포인터
    {
        public string 사번 { get; set; }
        public int pointer { get; set; }
        public string 품목코드 { get; set; }
        public int 수량 { get; set; }

        public int 검사항목수 { get; set; }
        public int 다음포인터 { get; set; }

        public bool 다음측정 { get; set; }
    }
}
