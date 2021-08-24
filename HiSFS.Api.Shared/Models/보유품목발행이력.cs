using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 보유품목발행이력 : 공통정보
    {
        [MaxLength(30)]
        //[Key]
        [ForeignKey("보유품목")]
        public string 보유품목코드 { get; set; }

        [MaxLength(4)]
        //[Key]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(20)]
        public string 작업지시코드 { get; set; }
        public int 공정차수 { get; set; }

        [MaxLength(20)]
        public string LOT번호 { get; set; }

        [MaxLength(50)]
        public string 품목_LOT번호 { get; set; }

        // --------------------------------------

        public 보유품목정보 보유품목 { get; set; }

        public 사업장 회사 { get; set; }

        [ForeignKey("작업지시코드, 공정차수")]
        public 생산지시공정차수정보 생산지시공정차수 { get; set; }
    }
}
