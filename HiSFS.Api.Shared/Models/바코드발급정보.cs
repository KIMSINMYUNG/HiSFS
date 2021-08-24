using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HiSFS.Api.Shared.Models
{
    public class 바코드발급정보
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int 발급순번 { get; set; }

        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(30)]
        [ForeignKey("품목")]
        public string 품목코드 { get; set; }

        [MaxLength(20)]
        public string LOT번호 { get; set; }

        [MaxLength(10)]
        public string 사원코드 { get; set; }

        [MaxLength(5)]
        public string 수량 { get; set; }

        [MaxLength(1)]
        public string 구분 { get; set; }

        [MaxLength(8)]
        public string 생성일자 { get; set; }

        public bool 입고유무 { get; set; }

        [MaxLength(8)]
        public string 입고일자 { get; set; }

        public 사업장 회사 { get; set; }
        public 품목정보 품목 { get; set; }
    }
}
