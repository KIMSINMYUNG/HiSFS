using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 보유품목위치정보:공통정보
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int 보유품목위치순번 { get; set; }

        [MaxLength(30)]
        [ForeignKey("보유품목")]
        public string 보유품목코드 { get; set; }

        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(10)]

        [ForeignKey("장소위치")]
        public string 장소위치코드 { get; set; }

        [MaxLength(15)]
       
        public string 위치상세코드 { get; set; }


        [Column(TypeName = "decimal(17, 6)")]
        public decimal 수량 { get; set; }

        [MaxLength(20)]
        public string LOT번호 { get; set; }

        [MaxLength(50)]
        public string 품목_LOT번호 { get; set; }


        // ------
        public 사업장 회사 { get; set; }
        public 보유품목정보 보유품목 { get; set; }
        public 장소위치정보 장소위치 { get; set; }
    }
}
