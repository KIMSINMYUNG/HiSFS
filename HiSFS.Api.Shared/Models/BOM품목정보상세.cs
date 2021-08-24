using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class BOM품목정보상세 : 공통정보
    {

        [MaxLength(30)]
        [Key, ForeignKey("품목")]
        public string 품목코드 { get; set; }

        [MaxLength(30)]
        [Key, ForeignKey("BOM품목정보")]
        public string BOM품목정보코드 { get; set; }

        [MaxLength(20)]
        //[ForeignKey("공정단위")]
        public string 공정단위코드 { get; set; }


        //[Column(TypeName = "decimal(7, 3)")]
        //public decimal 정미수량 { get; set; }
        //[Column(TypeName = "decimal(7, 3)")]
        //public decimal 로스율 { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 필요수량 { get; set; }

        [MaxLength(10)]
        public string 레벨 { get; set; }

        // ----------------------------
        public 품목정보 품목 { get; set; }

        public BOM품목정보 BOM품목정보 { get; set; }

        //public 공정단위정보 공정단위 { get; set; }

    }
}
