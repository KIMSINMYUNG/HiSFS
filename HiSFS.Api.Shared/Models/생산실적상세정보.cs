using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 생산실적상세정보 : 공통정보
	{


		[Required(ErrorMessage = "필수 입니다.")]
		[Key, ForeignKey("회사")]
		[MaxLength(4)]
		public string 회사코드 { get; set; }

		[MaxLength(20)]
		[ForeignKey("생산실적헤더")]
		public string 생산지시코드 { get; set; }

		[Key]
		public int 작업순번 { get; set; }



		[MaxLength(10)]
		[ForeignKey("작업자")]
		public string 작업자사번 { get; set; }


		[MaxLength(4)]
		public string 부서코드 { get; set; }


		[MaxLength(30)]
		public string 사용품번 { get; set; }



		[Column(TypeName = "decimal(17, 6)")]
		public decimal 사용수량 { get; set; }

		[Column(TypeName = "decimal(7, 3)")]
		public decimal 불량수량 { get; set; }

		public DateTime? 실적등록일 { get; set; }

		[MaxLength(300)]
		public string 비고 { get; set; }

		[MaxLength(1)]
		public string 일괄생산등록유무 { get; set; }

		[MaxLength(20)]
		public string LOT번호 { get; set; }

		[MaxLength(50)]
		public string 품목_LOT번호 { get; set; }

		public 생산실적헤더정보 생산실적헤더 { get; set; }

		

		public 사업장 회사 { get; set; }

		public 직원정보 작업자 { get; set; }



		[MaxLength(12)]
		public string 주문번호 { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public decimal 주문순번 { get; set; }


		[MaxLength(4)]
		public string 사용공정_사용창고 { get; set; }
		[MaxLength(4)]
		public string 사용작업장_사용장소 { get; set; }



	}
}
