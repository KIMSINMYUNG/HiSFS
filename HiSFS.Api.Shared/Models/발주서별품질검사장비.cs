using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace HiSFS.Api.Shared.Models
{

    public class 발주서별품질검사장비 : 공통정보
    {

        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [Key]
        [MaxLength(12)]
        public string 발주번호 { get; set; }

        [Column(TypeName = "decimal(5, 0)")]
        public decimal 발주순번 { get; set; }

        [MaxLength(10)]
        //[Key]
        // [Required(ErrorMessage = "검사정보는 필수입니다.")]
        public string? 품질검사코드 { get; set; }

        [Required(ErrorMessage = "연동장비는 필수입니다.")]
        //[Key]
        [ForeignKey("검사장비")]
        public int? 검사장비식별번호 { get; set; }

        // ---------------------------------
        //[ForeignKey("발주번호")]
        //public 발주서별품질검사정보 검사정보 { get; set; }

        public 연동장비정보 검사장비 { get; set; }

        public 사업장 회사 { get; set; }
    }
}
