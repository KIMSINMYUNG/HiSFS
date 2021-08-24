using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View
{
    public class 생산계획작업지시현황 : 공통정보
    {
        [Key]
        public string 작업지시코드 { get; set; }

        public string 작업지시명 { get; set; }
        [ForeignKey("생산지시유형")]
        public string 생산지시유형코드 { get; set; }
        [ForeignKey("실행상태")]
        public string 실행상태코드 { get; set; }
        public DateTime? 시작일 { get; set; }
        public DateTime? 완료목표일 { get; set; }
        public decimal 목표수량 { get; set; }
        public decimal 양산수량 { get; set; }
        public decimal 불량수량 { get; set; }
        
        // ------
        public 공통코드 생산지시유형 { get; set; }
        public 공통코드 실행상태 { get; set; }
    }
}
