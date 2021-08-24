using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 보유품목불량정보 : 공통정보
    {
        [MaxLength(30)]
        [Key]
        [ForeignKey("보유품목")]
        public string 보유품목코드 { get; set; }

        [MaxLength(10)]
        [Key]
        [ForeignKey("불량유형")]
        public string 불량유형코드 { get; set; }

        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        public DateTime? 불량등록일시 { get; set; }
        public DateTime? 불량변경일시 { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal 수량 { get; set; }

        [MaxLength(20)]
        public string LOT번호 { get; set; }

        [MaxLength(50)]
        public string 품목_LOT번호 { get; set; }

        // ---------------------------------------------
        public 보유품목정보 보유품목 { get; set; }
        public 사업장 회사 { get; set; }
        public 공통코드 불량유형 { get; set; }
    }
}
