using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 생산계획공통 : 공통정보
    {
        [MaxLength(30)]
        public string 계획기록 { get; set; }
        public DateTime? 계획기록일 { get; set; }

        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(10)]
        [ForeignKey("계획자")]
        public string 계획자사번 { get; set; }

        [MaxLength(100)]
        public string 검토기록 { get; set; }

        [MaxLength(10)]
        [ForeignKey("검토자")]
        public string 검토자사번 { get; set; }
        public DateTime? 검토기록일 { get; set; }

        public string 전달사항 { get; set; }
        //------
        public 직원정보 계획자 { get; set; }
        public 직원정보 검토자 { get; set; }
        public 사업장 회사 { get; set; }
    }
}
