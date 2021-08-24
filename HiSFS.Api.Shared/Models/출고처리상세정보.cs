using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 출고처리상세정보 : 공통정보
	{
		[Key]
		[MaxLength(4)]
		public string 회사코드 { get; set; }

		[Key, ForeignKey("출고처리헤더")]
		[MaxLength(12)]
		public string 작업번호 { get; set; }
		[Key]
		[Column(TypeName = "decimal(5, 0)")]
		public decimal 작업순번 { get; set; }
		[MaxLength(30)]
		public string 품번 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal 출고수량_관리단위 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal 출고수량_재고단위 { get; set; }
		[MaxLength(12)]
		public string 주문번호 { get; set; }
		[Column(TypeName = "decimal(5, 0)")]
		public decimal 주문순번 { get; set; }
		[MaxLength(4)]
		public string 장소코드 { get; set; }
		[MaxLength(1)]
		public string 연동구분 { get; set; }
		[MaxLength(20)]
		public string LOT번호 { get; set; }

		[MaxLength(50)]
		public string 품목_LOT번호 { get; set; }


		public 출고처리헤더정보 출고처리헤더 { get; set; }

	}
}
