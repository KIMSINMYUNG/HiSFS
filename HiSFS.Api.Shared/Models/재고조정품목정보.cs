using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 재고조정품목정보 : 공통정보
    {
        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(30)]
        [Key]
        public string 품목코드 { get; set; }

        [MaxLength(30)]
        [Required(ErrorMessage = "품번은 필수입니다.")]
        public string 원품목코드 { get; set; }


        public int 관리차수 { get; set; }

        [MaxLength(40)]
        [Required(ErrorMessage = "품목명은 필수입니다.")]
        public string 품목명 { get; set; }

        [MaxLength(40)]
        public string 품목영문명 { get; set; }


        [MaxLength(10)]
        [ForeignKey("품목구분")]
        public string 품목구분코드 { get; set; }

        //품목구분코드
        [MaxLength(1)]
        public string 계정구분코드 { get; set; }

        // 더존 조달구분
        [MaxLength(1)]
        public string 조달분류 { get; set; }

        [MaxLength(10)]
        [ForeignKey("조달구분")]
        public string? 조달구분코드 { get; set; }

        [MaxLength(4)]
        public string 재고단위 { get; set; }

        [MaxLength(4)]
        public string 관리단위 { get; set; }

        [Column(TypeName = "decimal(17, 6)")]
        public decimal 환산계수 { get; set; }

        [MaxLength(10)]
        public string 대분류코드 { get; set; }

        [MaxLength(40)]
        public string 대분류명 { get; set; }

        [MaxLength(10)]
        public string 중분류코드 { get; set; }

        [MaxLength(40)]
        public string 중분류명 { get; set; }


        [MaxLength(10)]
        public string 소분류코드 { get; set; }

        [MaxLength(40)]
        public string 소분류명 { get; set; }


        [MaxLength(10)]
        [ForeignKey("품목유형")]
        public string 품목유형코드 { get; set; }

        [MaxLength(10)]
        [ForeignKey("소재")]
        public string 소재코드 { get; set; }


        [MaxLength(10)]
        [ForeignKey("규격종류")]
        public string 규격종류코드 { get; set; }

        [MaxLength(80)]
        public string 규격 { get; set; }

        [MaxLength(10)]
        [Required(ErrorMessage = "단위는 필수입니다.")]
        [ForeignKey("단위")]
        public string 단위코드 { get; set; }

        // 더존 LOT사용여부 nvarchar
        public bool LOT여부 { get; set; }

        public int LOT기본수량 { get; set; }
        //public string 주거래처 { get; set; }


        // 더존 주거래코드
        [MaxLength(10)]
        //[ForeignKey("거래처")]
        public string 거래처코드 { get; set; }


        [MaxLength(1)]
        public string 검사여부 { get; set; }


        [MaxLength(256)]
        public string 비고 { get; set; }

        [Column(TypeName = "decimal(17, 6)")]
        public decimal? 조정수량  { get; set; }

       

        public 사업장 회사 { get; set; }
        //------
        public 공통코드 품목구분 { get; set; }
        public 공통코드 품목유형 { get; set; }
        public 공통코드 소재 { get; set; }
        public 공통코드 규격종류 { get; set; }
        public 공통코드 조달구분 { get; set; }
        public 공통코드 단위 { get; set; }
        public 거래처정보 거래처 { get; set; }



        //public override bool Equals(object obj)
        //{
        //    if (obj == null)
        //        return false;

        //    var target = obj as 품목정보;
        //    if (target == null)
        //        return false;

        //    return 품목코드 == target.품목코드;
        //}


    }
}
