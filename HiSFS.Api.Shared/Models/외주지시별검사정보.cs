using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 외주지시별검사정보 : 공통정보
    {
        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(12)]
        [Key]
        public string 지시번호 { get; set; }

        [Column(TypeName = "decimal(5, 0)")]
        public decimal 전개순번 { get; set; }

        [Required(ErrorMessage = "품질검사는 필수입니다.")]

        [MaxLength(10)]
        [ForeignKey("품질검사")]
        public string 품질검사코드 { get; set; }

        [MaxLength(10)]
        [Required(ErrorMessage = "검사단위는 필수입니다.")]
        [ForeignKey("검사단위")]
        public string 검사단위코드 { get; set; }


        [Column(TypeName = "decimal(7, 3)")]
        [Required(ErrorMessage = "검사기준값은 필수입니다.")]
        public decimal 검사기준값 { get; set; }
        
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 오차범위 { get; set; }

        // 추가 2021.02.01  검사측정값 , 합격여부
        [Column(TypeName = "decimal(7, 3)")]
        public decimal? 검사측정값 { get; set; }

        [MaxLength(10)]
        public string 합격여부 { get; set; }

        // ---------------------------------
        public 품질검사정보 품질검사 { get; set; }
        public 공통코드 검사단위 { get; set; }

        public IList<외주지시별품질검사장비> 외주지시별품질검사장비목록 { get; set; }
        [Required(ErrorMessage = "오차범위는 필수입니다.")]
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 오차범위상한 { get; set; }

        [Required(ErrorMessage = "오차범위는 필수입니다.")]
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 오차범위하한 { get; set; }

        public 사업장 회사 { get; set; }
    }
}
