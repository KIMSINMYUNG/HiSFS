using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class BOM품목정보 : 공통정보
    {
        [MaxLength(30)]
        [Key]
        public string BOM품목정보코드 { get; set; }

        [MaxLength(30)]
        public string 품목구분코드 { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal 정미수량 { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 로스율 { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 필요수량 { get; set; }

        //// ----------------------------
        //public 품목정보 품목 { get; set; }

        public List<BOM품목정보상세> S_BOM정보상세 { get; set; }


    }
}
