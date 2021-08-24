using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 발주서별품질검사측정정보 : 공통정보
    {
        [Key]
        public int 시리얼넘버 { get; set; }

        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(20)]
        [Key]
        public string 발주번호 { get; set; }

        [Key]
        [Column(TypeName = "decimal(5, 0)")]
        public decimal 발주순번 { get; set; }


        [MaxLength(10)]
        [ForeignKey("품질검사")]
        public string 품질검사코드 { get; set; }

        [MaxLength(20)]
        //[ForeignKey("공정단위")]
        public string 공정단위코드 { get; set; }

        [MaxLength(10)]
        //[Required(ErrorMessage = "검사단위는 필수입니다.")]
        //[ForeignKey("검사단위")]
        public string 검사단위코드 { get; set; }

        [MaxLength(100)]
        //[Required(ErrorMessage = "생산품공정명은 필수입니다.")]
        public string 생산품공정명 { get; set; }

        [MaxLength(30)]
        //[Required(ErrorMessage = "생산품은 필수입니다.")]
        // [ForeignKey("생산품")]
        public string 생산품공정코드 { get; set; }

        [MaxLength(30)]
        public string? 보유품목코드 { get; set; }


        [Column(TypeName = "decimal(7, 3)")]
        [Required(ErrorMessage = "검사기준값은 필수입니다.")]
        public decimal 검사기준값 { get; set; }
        [Required(ErrorMessage = "오차범위는 필수입니다.")]
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 오차범위 { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal? 검사측정값 { get; set; }

        [MaxLength(10)]
        public string 합격여부 { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [MaxLength(20)]
        public string LOT번호 { get; set; }

        [MaxLength(50)]
        public string 품목_LOT번호 { get; set; }

        public 사업장 회사 { get; set; }
        public 보유품목정보 보유품목 { get; set; }
        public 품질검사정보 품질검사 { get; set; }
        public 공통코드 검사단위 { get; set; }

        //public IList<공정단위검사장비> 공정검사장비목록 { get; set; }

        //생산품코드 KMFA-518986:1
        //공정단위코드 PU0001:1
        //품질검사코드
        //검사단위코드
        //검사기준값
        //오차범위
        //합격여부
        //검사측정값
        //CreateTime
        //CreateId
        //UpdateTime
        //UpdateId
        //사용유무
        //삭제유무
        //상세JSON
    }
}
