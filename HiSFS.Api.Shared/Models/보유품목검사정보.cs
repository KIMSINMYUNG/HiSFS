using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 보유품목검사정보 : 공통정보
    {
        /// <summary>
        /// 보유품목코드는 제품검사 시 발행한 완성품의 보유품목코드가 되므로 유일한 키임
        /// </summary>
        //[Key]
        [MaxLength(30)]
        [ForeignKey("보유품목")]
        public string 보유품목코드 { get; set; }
        //[Key]
        [MaxLength(10)]
        [ForeignKey("품질검사")]
        public string 품질검사코드 { get; set; }

        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(20)]
        public string 공정단위코드 { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal? 측정값 { get; set; }
        public bool 측정유무 { get; set; }
        /// <summary>
        /// S9212 01 - 양품, 02 - 불량
        /// </summary>
        /// 
        [MaxLength(10)]
        [ForeignKey("검사결과")]
        public string 검사결과코드 { get; set; }

        [MaxLength(10)]
        [ForeignKey("불량유형")]
        public string 불량유형코드 { get; set; }

        [MaxLength(20)]
        public string LOT번호 { get; set; }

        [MaxLength(50)]
        public string 품목_LOT번호 { get; set; }
        // -----------------------------
        public 보유품목정보 보유품목 { get; set; }
        public 품질검사정보 품질검사 { get; set; }
        [ForeignKey("공정단위코드, 품질검사코드")]
        public 공정단위검사정보 검사정보 { get; set; }
        public 공통코드 검사결과 { get; set; }
        public 공통코드 불량유형 { get; set; }
        public 사업장 회사 { get; set; }
    }
}
