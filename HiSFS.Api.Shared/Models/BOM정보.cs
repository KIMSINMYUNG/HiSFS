using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class BOM정보 : 공통정보
    {
        [Key]
        public int BOM순번 { get; set; }

        [ForeignKey("품목")]
        public string 품목코드 { get; set; }
        [ForeignKey("상위BOM")]
        public int? 상위BOM순번 { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal 정미수량 { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 로스율 { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 필요수량 { get; set; }

        // ----------------------------
        public 품목정보 품목 { get; set; }
        public BOM정보 상위BOM { get; set; }

        public IList<BOM정보> 하위BOM목록 { get; set; }
    }
}
