using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View
{
    public class 공정단위자재현황 
    {
        
        public string 회사코드 { get; set; }

        public string 공정단위코드 { get; set; }
      
        public string 자재코드 { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal 필요수량 { get; set; }
        
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 사용수량 { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal 실적수량 { get; set; }


        [Column(TypeName = "decimal(7, 3)")]
        public decimal 불량수량 { get; set; }

    }
}
