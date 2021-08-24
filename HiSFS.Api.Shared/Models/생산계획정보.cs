using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 생산계획정보 : 공통정보
    {
        [Key]
        [MaxLength(20)]
        public string 생산계획코드 { get; set; }
        [Required(ErrorMessage = "생산계획명은 필수입니다.")]

        [MaxLength(4)]
        [Key]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(200)]
        public string 생산계획명 { get; set; }
        //[Required(ErrorMessage = "생산품은 필수입니다.")]

        [MaxLength(30)]
        [ForeignKey("생산품")]
        public string 생산품코드 { get; set; }

        [MaxLength(30)]
        [Required(ErrorMessage = "생산품공정 필수입니다.")]
        [ForeignKey("생산품공정")]
        public string 생산품공정코드 { get; set; }

        [MaxLength(10)]
        //[ForeignKey("발주처")]
        public string 발주처코드 { get; set; }
        public DateTime? 발주일 { get; set; }
        public DateTime? 납품일 { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 발주수량 { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 계획수량 { get; set; }
        public DateTime? 실행일시 { get; set; }
        public DateTime? 종료일시 { get; set; }

        [MaxLength(10)]
        [ForeignKey("생산책임자")]
        public string 생산책임자사번 { get; set; }

        [MaxLength(10)]
        [ForeignKey("생산유형")]
        public string 생산유형코드 { get; set; }

        [MaxLength(10)]
        [ForeignKey("생산계획상태")]
        public string 생산계획상태코드 { get; set; }

        [NotMapped]
        public string 상세 { get; set; }


        [MaxLength(4)]
        public string 부서코드 { get; set; }


        [MaxLength(12)]
        public string 주문번호 { get; set; }

        // -------------------------------------------
        public 품목정보 생산품 { get; set; }
        public 생산품공정정보 생산품공정 { get; set; }
        public 거래처정보 발주처 { get; set; }
        public 직원정보 생산책임자 { get; set; }
        public 공통코드 생산유형 { get; set; }
        public 공통코드 생산계획상태 { get; set; }

        public 생산계획기본정보 생산계획기본 { get; set; }
        public 생산계획영업정보 생산계획영업 { get; set; }
        public 생산계획연구소정보 생산계획연구소 { get; set; }
        public 생산계획구매정보 생산계획구매 { get; set; }
        public 생산계획생산정보 생산계획생산 { get; set; }
        public 생산계획품질정보 생산계획품질 { get; set; }
        public 생산계획생산관리정보 생산계획생산관리 { get; set; }

        public IList<생산지시정보> 생산지시목록 { get; set; }
        public 사업장 회사 { get; set; }
    }
}
