using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 외주작업지시헤더정보 : 공통정보
	{
		[Key]
		[MaxLength(4)]
		public string 회사코드 { get; set; }
		[Key]
		[MaxLength(12)]
		public string 지시번호 { get; set; }

		[MaxLength(30)]
		public string 품번 { get; set; }

		[MaxLength(40)]
		public string 품명 { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public decimal 전개순번 { get; set; }

		[MaxLength(4)]
		public string 공정 { get; set; }

		[MaxLength(40)]
		public string 공정명 { get; set; }


		[MaxLength(1)]
		public string 지시구분 { get; set; }

		[MaxLength(5)]
		public string 지시구분명 { get; set; }

		[MaxLength(1)]
		public string 생산외주구분 { get; set; }

		[MaxLength(4)]
		public string 생산외주구분명 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal 수량 { get; set; }



		public List<외주작업지시서정보> 외주작업지시서상세 { get; set; }
	}
}
