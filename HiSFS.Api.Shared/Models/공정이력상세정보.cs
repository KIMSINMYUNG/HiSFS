using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 공정이력상세정보 : 공통정보
	{
		[Key]
		public int 인덱스 { get; set; }

		[MaxLength(4)]
		[ForeignKey("회사")]
		public string 회사코드 { get; set; }


		[ForeignKey("공정이력")]
		public int 공정이력인덱스 { get; set; }


		[Column(TypeName = "decimal(7, 3)")]
		public decimal 생산수량 { get; set; }

		[Column(TypeName = "decimal(7, 3)")]
		public decimal 불량수량 { get; set; }

		[Column(TypeName = "decimal(7, 3)")]
		public decimal 자재불량수량 { get; set; }

		[MaxLength(20)]
		public string 공정상태 { get; set; }

		[MaxLength(10)]
		[ForeignKey("작업자")]
		public string 작업자사번 { get; set; }


		public DateTime? 시작타임 { get; set; }
		public DateTime? 종료타임 { get; set; }


		public 공정이력정보 공정이력 { get; set; }

		public 직원정보 작업자 { get; set; }

		public 사업장 회사 { get; set; }
	}
}
