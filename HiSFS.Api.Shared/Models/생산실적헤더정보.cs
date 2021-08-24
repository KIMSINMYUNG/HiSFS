using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 생산실적헤더정보 : 공통정보
	{

		[MaxLength(4)]
		[Key]
		public string 회사코드 { get; set; }

		[MaxLength(20)]
		[Key]
		public string 생산지시코드 { get; set; }



		[MaxLength(30)]
		//[Required(ErrorMessage = "생산품은 필수입니다.")]
		[ForeignKey("생산품")]
		public string 생산품코드 { get; set; }

		[MaxLength(20)]
		//[Required(ErrorMessage = "공정단위는 필수입니다.")]
		[ForeignKey("공정단위")]
		public string 공정단위코드 { get; set; }


		[MaxLength(30)]
		//[Key]
		[ForeignKey("생산품공정")]
		public string 생산품공정코드 { get; set; }

		[MaxLength(200)]
		public string 생산지시명 { get; set; }

		[MaxLength(4)]
		public string 사업장코드 { get; set; }

		[MaxLength(4)]
		public string 실적공정코드_창고코드 { get; set; }
		[MaxLength(4)]
		public string 실적작업장코드_장소코드 { get; set; }
		[MaxLength(1)]
		public string 재작업여부 { get; set; }

		[MaxLength(20)]
		public string LOTNO { get; set; }



		[MaxLength(1)]
		public string 일괄생산등록유무 { get; set; }



		[MaxLength(12)]
		public string 작업번호 { get; set; }


		[Column(TypeName = "decimal(17, 6)")]
		public decimal 실적수량 { get; set; }

		[Column(TypeName = "decimal(7, 3)")]
		public decimal 불량수량 { get; set; }


		public 품목정보 생산품 { get; set; }
		public 사업장 회사 { get; set; }

		public 공정단위정보 공정단위 { get; set; }

		public 생산품공정정보 생산품공정 { get; set; }


		[MaxLength(12)]
		public string 주문번호 { get; set; }

		[MaxLength(1)]
		public string 실적구분 { get; set; }

		[MaxLength(4)]
		public string 실적공정코드 { get; set; }
		[MaxLength(4)]
		public string 실적작업장코드 { get; set; }


	}
}
