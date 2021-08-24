using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 보유품목일지 : 공통정보
	{
		[MaxLength(100)]
		[Key]
		public string 보유품목일지코드 { get; set; }
		
		public int 순번 { get; set; }

		[MaxLength(4)]
		[ForeignKey("회사")]
		public string 회사코드 { get; set; }

		[MaxLength(30)]
		[ForeignKey("보유품목")]
		public string 보유품목코드 { get; set; }

		[MaxLength(30)]
		[ForeignKey("품목")]
		public string 품목코드 { get; set; }


		[Column(TypeName = "decimal(17, 6)")]
		public decimal 수량 { get; set; }


		[Required]
		[MaxLength(8)]
		public string 보유년월일 { get; set; }

		[MaxLength(8)]
		public string? 생산년월일 { get; set; }


		[MaxLength(8)]
		public string? 출고년월일 { get; set; }

		public DateTime? 보유일 { get; set; }

		[MaxLength(20)]
		public string LOT번호 { get; set; }

		[MaxLength(50)]
		public string 품목_LOT번호 { get; set; }




		//[ForeignKey("거래처")]
		[MaxLength(10)]
		public string? 거래처코드 { get; set; }

		
		public 보유품목정보 보유품목 { get; set; }
		public 품목정보 품목 { get; set; }

		public 거래처정보 거래처 { get; set; }
		public 사업장 회사 { get; set; }
	}
}
