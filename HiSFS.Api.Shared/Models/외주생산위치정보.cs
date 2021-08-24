using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 외주생산위치정보 : 공통정보
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int 보유품목위치순번 { get; set; }

        [MaxLength(30)]
        public string 보유품목코드 { get; set; }

        [MaxLength(4)]
        public string 회사코드 { get; set; }

        [MaxLength(10)]

        public string 장소위치코드 { get; set; }

        [MaxLength(15)]

        public string 위치상세코드 { get; set; }


        [Column(TypeName = "decimal(7, 3)")]
        public decimal 수량 { get; set; }

        [MaxLength(20)]
        public string LOT번호 { get; set; }

        [MaxLength(50)]
        public string 품목_LOT번호 { get; set; }

        [MaxLength(20)]
        public string 사유 { get; set; }

        [MaxLength(20)]
        public string 지시서 { get; set; }

        [MaxLength(20)]
        public string 기타 { get; set; }


        [MaxLength(20)]
        public string 반입여부 { get; set; }


        [MaxLength(4)]

        public string 외주창고코드 { get; set; }

        [MaxLength(4)]

        public string 외주장소코드 { get; set; }

        [MaxLength(30)]

        public string 외주작업장명 { get; set; }


        [Column(TypeName = "decimal(7, 3)")]
        public decimal 불량수량 { get; set; }


        [Column(TypeName = "decimal(7, 3)")]
        public decimal 필요수량 { get; set; }

        [MaxLength(4)]

        public string 반입장소코드 { get; set; }
    }
}
