using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class 더존품목정보
	{
		[MaxLength(4)]
		[Key]
		public string 회사코드 { get; set; }

		[MaxLength(30)]
        [Key]
        public string 품목코드 { get; set; }

        [MaxLength(30)]
        public string 원품목코드 { get; set; }


        public int 관리차수 { get; set; }

        [MaxLength(40)]
        public string 품목명 { get; set; }

        [MaxLength(40)]
        public string 품목영문명 { get; set; }


        [MaxLength(10)]
        public string 품목구분코드 { get; set; }


        [MaxLength(1)]
        public string 계정구분코드 { get; set; }

        // 더존 조달구분
        [MaxLength(1)]
        public string 조달분류 { get; set; }

        [MaxLength(10)]
        public string 조달구분코드 { get; set; }

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
        [MaxLength(1)]
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

    }
}
