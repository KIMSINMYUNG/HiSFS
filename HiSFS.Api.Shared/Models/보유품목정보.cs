using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 보유품목정보 : 공통정보
    {
		[MaxLength(4)]
		[Key, ForeignKey("회사")]
		public string 회사코드 { get; set; }

		[MaxLength(30)]
        [Key]
        public string 보유품목코드 { get; set; }

        [MaxLength(30)]
        [ForeignKey("품목")]
        public string 품목코드 { get; set; }


        [MaxLength(8)]
        public string 보유년월일 { get; set; }

        [MaxLength(8)]
        public string 조정년월일 { get; set; }

        public int 순번 { get; set; }

        //[MaxLength(30)]
        //[ForeignKey("원보유품목")]
        //public string 원보유품목코드 { get; set; }

        [MaxLength(10)]
        [ForeignKey("품목구분")]
        public string 품목구분코드 { get; set; }

        [MaxLength(10)]
        [ForeignKey("장소")]
        public string 장소코드 { get; set; }

        [MaxLength(10)]
        [ForeignKey("장소위치")]
        public string 장소위치코드 { get; set; }
        [Column(TypeName = "decimal(17,6)")]
        public decimal 수량 { get; set; }

        [Column(TypeName = "decimal(17,6)")]
        public decimal 실제수량 { get; set; }

        [MaxLength(30)]
        public string 보유명 { get; set; }
        public DateTime? 보유일 { get; set; }


        [MaxLength(20)]
        public string LOT번호 { get; set; }

        [MaxLength(50)]
        public string 품목_LOT번호 { get; set; }

        // ------
        public 사업장 회사 { get; set; }
		public 공통코드 품목구분 { get; set; }
        //public 보유품목정보 원보유품목 { get; set; }
        public 품목정보 품목 { get; set; }
        public 장소정보 장소 { get; set; }
        public 장소위치정보 장소위치 { get; set; }
        public IEnumerable<보유품목이력> 보유품목이력 { get; set; }
        // 2021.04.20
        public 설비가동현황정보 설비가동현황 { get; set; }
        // 20210210 추가
        public ICollection<보유품목위치정보> 보유품목위치모두 { get; set; }
    }
}
