using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View
{
    public class 공정별품질검사측정현황
    {


        [MaxLength(4)]
        public string 회사코드 { get; set; }

        [MaxLength(20)]
        [Key]
        public string 생산지시코드 { get; set; }

        [MaxLength(10)]
        public string 품질검사코드 { get; set; }


        public string 공정명 { get; set; }

        public string 공정코드 { get; set; }

        public string 공정품명 { get; set; }

        [MaxLength(20)]
        public string 공정단위코드 { get; set; }

        [MaxLength(10)]
        public string 검사단위코드 { get; set; }

        [MaxLength(100)]
        public string 생산품공정명 { get; set; }

        [MaxLength(30)]
        public string 생산품공정코드 { get; set; }

        [MaxLength(30)]
        public string 보유품목코드 { get; set; }


        [Column(TypeName = "decimal(7, 3)")]
        public decimal 불량수량 { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal 검사수량 { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal? 합격수량 { get; set; }


        public int 합격률 { get; set; }

        public int 불량률 { get; set; }

      



    }
}
