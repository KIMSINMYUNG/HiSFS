using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HiSFS.Api.Shared.Models.View
{
    public class 작업지시공정현황 : 공통정보
    {
        public string 생산지시코드 { get; set; }

        public string 생산지시명 { get; set; }

        public int 공정차수 { get; set; }

        public string 공정명 { get; set; }
        public string 공정품명 { get; set; }
        public DateTime? 시작일 { get; set; }
        public DateTime? 완료목표일 { get; set; }
        public decimal 목표수량 { get; set; }
        public decimal 양산수량 { get; set; }
        public decimal 불량수량 { get; set; }

        public decimal 검사수량 { get; set; }
        public decimal 합격수량 { get; set; }
        public int 합격률 { get; set; }

        public int 불량률 { get; set; }

        public string 공정단위코드 { get; set; }

        public string 공정코드 { get; set; }


    }
}
