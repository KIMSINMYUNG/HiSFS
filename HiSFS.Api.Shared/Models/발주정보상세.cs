using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 발주정보상세 : 공통정보
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int 발주상세순번 { get; set; }

        [MaxLength(30)]
        [ForeignKey("품목")]
        public string 품목코드 { get; set; }

       // [ForeignKey("발주")]
        public int 발주순번 { get; set; }

        [MaxLength(10)]
        public string 품목구분코드 { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal 발주수량 { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal 입고수량 { get; set; }


        // ----------------------------
        public 공통코드 품목구분 { get; set; }
        public 품목정보 품목 { get; set; }
       // public 발주정보 발주 { get; set; }





    }
}
