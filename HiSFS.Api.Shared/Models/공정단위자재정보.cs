using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 공정단위자재정보 : 공통정보
    {
        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(20)]
        [Key, ForeignKey("공정단위")]
        public string 공정단위코드 { get; set; }
        [Required(ErrorMessage = "자재/부품은 필수 입니다.")]
        
        [MaxLength(30)]
        [Key, ForeignKey("자재")]
        public string 자재코드 { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 수량 { get; set; }

        // ------

        public 공정단위정보 공정단위 { get; set; }
        public 품목정보 자재 { get; set; }

        public 사업장 회사 { get; set; }
    }
}
