using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 재고이동상세정보 : 공통정보
	{
		[Key]
		[MaxLength(4)]
		public string 회사코드 { get; set; }
		[Key, ForeignKey("재고이동상헤더")]
		[MaxLength(12)]
		public string 작업번호 { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		[Key]
		public decimal 작업순번 { get; set; }
		[MaxLength(12)]
		public string 이동번호 { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public decimal 이동순번 { get; set; }

		//[Required(ErrorMessage = "필수사항입니다.")]
		[MaxLength(30)]
		public string 품번 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal 청구수량 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal 이동수량 { get; set; }

		[MaxLength(20)]
		public string LOT번호 { get; set; }

		[MaxLength(50)]
		public string 품목_LOT번호 { get; set; }


		[MaxLength(1)]
		public string 재공운영여부 { get; set; }

		[MaxLength(30)]
		public string 모품목코드 { get; set; }

		[MaxLength(10)]
		public string 관리구분코드 { get; set; }

		[MaxLength(10)]
		public string 프로젝트코드 { get; set; }

		[MaxLength(12)]
		public string 지시번호 { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public decimal 청구순번 { get; set; }


		[MaxLength(1)]
		public string APP_FG { get; set; } = "1";


		[MaxLength(30)]
		public string 상세_비고 { get; set; }

		[MaxLength(1)]
		public string 사용여부 { get; set; } = "1";

		[MaxLength(1)]
		public string 만료여부 { get; set; } = "1";

		public 재고이동헤더정보 재고이동헤더 { get; set; }

	}
}
