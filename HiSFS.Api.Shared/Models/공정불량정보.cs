using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 공정불량정보 : 공통정보
    {
        [Key]
        public int 인덱스 { get; set; }

        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }


        [MaxLength(20)]
        [ForeignKey("생산지시")]
        public string 생산지시코드 { get; set; }

        [MaxLength(100)]
        public string 생산지시명 { get; set; }

        [MaxLength(20)]
        [ForeignKey("공정단위")]
        public string 공정단위코드 { get; set; }

        [MaxLength(30)]
        [ForeignKey("보유품목")]
        public string 설비코드 { get; set; }

        [MaxLength(30)]
        [ForeignKey("생산품공정")]
        public string 생산품공정코드 { get; set; }


        [MaxLength(10)]
        [ForeignKey("작업자")]
        public string 작업자사번 { get; set; }

        [MaxLength(30)]
        [ForeignKey("생산품")]
        public string 생산품코드 { get; set; }

        
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 불량수량 { get; set; }

        [MaxLength(10)]
        [ForeignKey("공통")]
        public string 불량유형 { get; set; }

        [MaxLength(4)]
        public string 공정차수 { get; set; }


        [MaxLength(8)]
        public string 작업일 { get; set; }

        public 보유품목정보 보유품목 { get; set; }
        public 생산품공정정보 생산품공정 { get; set; }

        public 품목정보 생산품 { get; set; }
        //public 공정단위정보 공정단위 { get; set; }

        public 생산지시정보 생산지시 { get; set; }

        public 직원정보 작업자 { get; set; }

        public 사업장 회사 { get; set; }

        public 공정단위정보 공정단위 { get; set; }

        public 공통코드 공통 { get; set; }
    }
}
