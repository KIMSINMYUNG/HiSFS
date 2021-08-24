using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class BOM_정보 : 공통정보
	{
		[Key]
		[MaxLength(4)]
		public string 회사코드 { get; set; }

		[Key]
		[MaxLength(30)]
		public string 모품번 { get; set; }

		[MaxLength(40)]
		public string 모품명 { get; set; }

		[MaxLength(40)]
		public string 모규격 { get; set; }

		[MaxLength(4)]
		public string 모품목재고단위 { get; set; }

		[Key]
		[Column(TypeName = "decimal(5, 0)")]
		public decimal 순번 { get; set; }

		[MaxLength(30)]
		public string 자품번 { get; set; }

		[MaxLength(40)]
		public string 자품명 { get; set; }

		[MaxLength(40)]
		public string 자규격 { get; set; }

		[MaxLength(4)]
		public string 자품목재고단위 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal 정미수량 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal LOSS율 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal 필요수량 { get; set; }

		[MaxLength(1)]
		public string 외주구분 { get; set; }

		[MaxLength(1)]
		public string 임가공구분 { get; set; }

		[MaxLength(10)]
		public string 주거래처코드 { get; set; }

		[MaxLength(60)]
		public string 주거래처명 { get; set; }

		[MaxLength(8)]
		public string 시작일자 { get; set; }

		[MaxLength(8)]
		public string 종료일자 { get; set; }

		[MaxLength(1)]
		public string 사용여부 { get; set; }

	}
}
