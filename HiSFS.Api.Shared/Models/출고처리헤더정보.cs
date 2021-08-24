using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 출고처리헤더정보 : 공통정보
	{
		[MaxLength(4)]
		[Key]
		public string 회사코드 { get; set; }

		[MaxLength(12)]
		[Key]
		public string 작업번호 { get; set; }

		[Required(ErrorMessage = "필수항목입니다.")]
		public DateTime? 작업일자 { get; set; }


		[MaxLength(1)]
		public string 출고구분 { get; set; }

		[MaxLength(10)]
		//[Required(ErrorMessage = "필수항목입니다.")]
		public string 거래처코드 { get; set; }

		[Required(ErrorMessage = "필수항목입니다.")]
		public DateTime? 출고일자 { get; set; }

		[MaxLength(12)]
		public string 주문번호 { get; set; }

		[MaxLength(4)]
		//[Required(ErrorMessage = "필수항목입니다.")]
		public string 창고코드 { get; set; }
		[MaxLength(1)]
		public string 거래구분 { get; set; }

		[MaxLength(4)]
		public string 환종 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal 환율 { get; set; }
		[MaxLength(10)]
		public string 사원코드 { get; set; }
		[MaxLength(4)]
		public string 부서코드 { get; set; }
		[MaxLength(4)]
		public string 사업장코드 { get; set; }
		[MaxLength(1)]
		//[Required(ErrorMessage = "필수항목입니다.")]
		public string 과세구분 { get; set; }
		[MaxLength(1)]
		//[Required(ErrorMessage = "필수항목입니다.")]
		public string 단가구분 { get; set; }
		[MaxLength(1)]
		public string 연동구분 { get; set; }

		[MaxLength(12)]
		public string 납품처코드 { get; set; }

		[MaxLength(60)]
		public string 비고 { get; set; }

		[MaxLength(5)]
		public string 담당자코드 { get; set; }


	}
}
