using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 입고처리상세정보 : 공통정보
	{
		[Key]
		[MaxLength(4)]
		public string 회사코드 { get; set; }
		[Key, ForeignKey("입고처리헤더")]
		[MaxLength(12)]
		public string 작업번호 { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		[Key]
		public Decimal 작업순번 { get; set; }

		[MaxLength(12)]
		public string? 입고번호 { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public Decimal? 입고순번 { get; set; }

		[MaxLength(30)]
		public string 품번 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public Decimal 입고수량_관리단위 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public Decimal 입고수량_재고단위 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public Decimal? 입고단가 { get; set; }
		[Column(TypeName = "decimal(17, 4)")]
		public Decimal? 공급가 { get; set; }
		[Column(TypeName = "decimal(17, 4)")]
		public Decimal? 부가세 { get; set; }

		[Column(TypeName = "decimal(17, 4)")]
		public Decimal? 합계액 { get; set; }
		[MaxLength(4)]
		public string? 환종 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public Decimal? 환율 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public Decimal? 외화단가 { get; set; }
		[Column(TypeName = "decimal(17, 4)")]
		public Decimal? 외화금액 { get; set; }

		[MaxLength(20)]
		public string? LOT번호 { get; set; }
		[MaxLength(12)]
		public string 발주번호 { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public Decimal? 발주순번 { get; set; }

		[MaxLength(12)]
		public string? 입고의뢰번호 { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public Decimal? 입고의뢰순번 { get; set; }

		[MaxLength(20)]
		public string? 선적번호 { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public Decimal? 선적순번 { get; set; }

		[MaxLength(1)]
		public string? 사용여부 { get; set; }

		[MaxLength(1)]
		public string? 유효여부 { get; set; }

		[MaxLength(10)]
		public string 단가구분 { get; set; }

		[MaxLength(4)]
		public string 입고장소코드 { get; set; }
		public string? 비고 { get; set; }

		public 입고처리헤더정보 입고처리헤더 { get; set;}

	}
}
