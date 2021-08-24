using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View
{
    public class BOM품목정보현황 : 공통정보
    {

        [Key]
        public string BOM품목정보코드 { get; set; }

        public string 품목구분코드 { get; set; }

        public string 품목구분명 { get; set; }

        public string 품목명 { get; set; }

        public string 규격 { get; set; }
        public string 단위명 { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal 필요수량 { get; set; }

        //// ----------------------------
        public 품목정보 품목 { get; set; }



    }
}
