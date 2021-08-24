using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 외주작업지시서정보 : 공통정보
	{
		[Key, ForeignKey("회사")]
		[MaxLength(4)]
		public string 회사코드 { get; set; }
		[Key, ForeignKey("외주작업지시헤더")]
		[MaxLength(12)]
		public string 지시번호 { get; set; }

		public DateTime? 지시일 { get; set; }
		public DateTime? 완료일 { get; set; }

		[MaxLength(30)]
		public string 품번 { get; set; }

		[MaxLength(40)]
		public string 품명 { get; set; }

		[MaxLength(40)]
		public string 규격 { get; set; }

		[MaxLength(4)]
		public string 관리단위 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal 수량 { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		[Key]
		public decimal 전개순번 { get; set; }

		[MaxLength(4)]
		public string 공정 { get; set; }

		[MaxLength(40)]
		public string 공정명 { get; set; }

		[MaxLength(4)]
		public string 작업장 { get; set; }

		[MaxLength(40)]
		public string 작업장명 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal 외주단가 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal 외주금액 { get; set; }

		[MaxLength(10)]
		public string 설비코드 { get; set; }

		[MaxLength(40)]
		public string 설비명 { get; set; }

		[MaxLength(80)]
		public string 비고_DOC_DC { get; set; }

		[MaxLength(1)]
		public string 지시상태 { get; set; }

		[MaxLength(2)]
		public string 지시상태명 { get; set; }

		[MaxLength(1)]
		public string 지시구분 { get; set; }

		[MaxLength(5)]
		public string 지시구분명 { get; set; }

		[MaxLength(1)]
		public string 생산외주구분 { get; set; }

		[MaxLength(4)]
		public string 생산외주구분명 { get; set; }

		[MaxLength(1)]
		public string 처리구분 { get; set; }

		[MaxLength(2)]
		public string 처리구분명 { get; set; }

		[MaxLength(1)]
		public string 검사구분 { get; set; }

		[MaxLength(3)]
		public string 검사구분명 { get; set; }

		[MaxLength(20)]
		public string LOT번호 { get; set; }

		[MaxLength(50)]
		public string 품목_LOT번호 { get; set; }

		[MaxLength(10)]
		public string 거래처코드 { get; set; }

		[MaxLength(60)]
		public string 거래처명 { get; set; }

		[MaxLength(60)]
		public string 거래처약칭 { get; set; }

		[MaxLength(12)]
		public string 주문번호 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal 주문순번 { get; set; }

		[MaxLength(4)]
		public string 사업장코드 { get; set; }

		[MaxLength(10)]
		public string 작업팀 { get; set; }

		[MaxLength(40)]
		public string 작업팀명 { get; set; }

		[MaxLength(10)]
		public string 작업조 { get; set; }

		[MaxLength(40)]
		public string 작업조명 { get; set; }

		[MaxLength(60)]
		public string 비고 { get; set; }

		public 외주작업지시헤더정보 외주작업지시헤더 { get; set; }

		public 사업장 회사 { get; set; }



	}
}
