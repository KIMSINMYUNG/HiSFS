using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 재고이동헤더정보 : 공통정보
	{
		[MaxLength(4)]
		[Key]
		public string 회사코드 { get; set; }
		[MaxLength(12)]
		[Key]
		public string 작업번호 { get; set; }

		[Required(ErrorMessage = "필수사항 입니다.")]
		public DateTime? 작업일자 { get; set; }
		[MaxLength(12)]
		public string 이동번호 { get; set; }

		[Required(ErrorMessage = "필수사항 입니다.")]
		public DateTime? 이동일자 { get; set; }

		[Required(ErrorMessage = "필수사항 입니다.")]
		[MaxLength(1)]
		public string 이동구분 { get; set; }
		[MaxLength(1)]
		public string 입출고구분 { get; set; } = "2";

		[Required(ErrorMessage = "필수사항 입니다.")]
		[MaxLength(10)]
		public string 사원코드 { get; set; }

		[Required(ErrorMessage = "필수사항 입니다.")]
		public string 부서코드 { get; set; }


		[MaxLength(4)]
		public string 사업장코드 { get; set; }

		[Required(ErrorMessage = "필수사항 입니다.")]
		[MaxLength(4)]
		public string 출고창고코드 { get; set; }

		[Required(ErrorMessage = "필수사항 입니다.")]
		[MaxLength(4)]
		public string 출고장소코드 { get; set; }

		
		[MaxLength(15)]
		public string 출고장소위치상세코드 { get; set; }


		[Required(ErrorMessage = "필수사항 입니다.")]
		[MaxLength(4)]
		public string 입고공정_창고코드		 { get; set; }

		[Required(ErrorMessage = "필수사항 입니다.")]
		[MaxLength(4)]
		public string 입고작업장_장소코드		 { get; set; }

		[MaxLength(15)]
		public string 입고장소위치상세코드 { get; set; }

		[MaxLength(10)]
		public string 담당자코드 { get; set; }
		[MaxLength(60)]
		public string 헤더비고 { get; set; }
		[MaxLength(1)]
		public string 처리구분 { get; set; } = "1";
		[MaxLength(1)]
		public string APP_FG { get; set; } = "0";

	}
}
