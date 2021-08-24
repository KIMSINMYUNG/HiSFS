using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 생산지시정보 : 공통정보
    {
        [MaxLength(20)]
        [Key]
        public string 생산지시코드 { get; set; }

        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(10)]
        [ForeignKey("실행상태")]
        public string 실행상태코드 { get; set; }

        [MaxLength(20)]
        [ForeignKey("생산계획")]
        public string 생산계획코드 { get; set; }
        public int 순번 { get; set; }
        [Required(ErrorMessage = "생산지시명은 필수입니다.")]

        [MaxLength(200)]
        public string 생산지시명 { get; set; }

        [MaxLength(10)]
        [ForeignKey("생산지시유형")]
        [Required(ErrorMessage = "생산지시유형은 필수입니다.")]
        public string 생산지시유형코드 { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 생산수량 { get; set; }
       
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 실생산량 { get; set; }
        
        public DateTime? 시작일 { get; set; }
        public DateTime? 완료목표일 { get; set; }
        public string 비고 { get; set; }

        [NotMapped]
        public string 상세 { get; set; }

        // -----------------------------------------------
        /// <summary>
        /// B20
        /// </summary>
        public 공통코드 실행상태 { get; set; }
        public 생산계획정보 생산계획 { get; set; }
        /// <summary>
        /// B21
        /// </summary>
        public 공통코드 생산지시유형 { get; set; }
        public IList<생산지시공정차수정보> 생산지시공정차수목록 { get; set; }

        ////////////////// 2021.02.17
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 검사수량 { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal 합격수량 { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal 불량수량 { get; set; }


        [MaxLength(1)]
        public string 재작업여부 { get; set; }

        public 사업장 회사 { get; set; }


    }
}
