using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 사용자재보고정보 : 공통정보
	{
		[Key]
		[MaxLength(4)]
		public string 회사코드 { get; set; }

		[Key]
		[MaxLength(12)]
		public string 작업번호 { get; set; }

		[Key]
		[Column(TypeName = "decimal(3, 0)")]
		public decimal 작업순번 { get; set; }

		public DateTime? 작업일자 { get; set; }

		[MaxLength(30)]
		public string 품번 { get; set; }

		[MaxLength(4)]
		public string 사용공정 { get; set; }

		[MaxLength(4)]
		public string 사용작업장 { get; set; }

		[MaxLength(12)]
		public string 지시번호 { get; set; }

		[MaxLength(1)]
		public string 지시구분 { get; set; }

		[MaxLength(10)]
		public string 사원코드 { get; set; }

		[MaxLength(10)]
		public string 부서코드 { get; set; }

		[MaxLength(1)]
		public string 사용여부 { get; set; }

		[MaxLength(1)]
		public string 유무상구분 { get; set; }

		[MaxLength(1)]
		public string 유효여부 { get; set; }

		[MaxLength(12)]
		public string 실적번호 { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public decimal 사용순번 { get; set; }

		public DateTime? 사용일자 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal 사용수량 { get; set; }

		[MaxLength(20)]
		public string LOT번호		{ get; set; }

		[MaxLength(10)]
		public string 프로젝트코드 { get; set; }

		[MaxLength(4)]
		public string 사업장코드 { get; set; }

		[MaxLength(60)]
		public string 비고 { get; set; }

		[Column(TypeName = "decimal(3, 0)")]
		public decimal 지시전개순번 { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public decimal 소요자재순번 { get; set; }

		[MaxLength(10)]
		public string 관리내역코드 { get; set; }

		[MaxLength(60)]
		public string 비고_보조언어 { get; set; }

		[MaxLength(10)]
		public string 최초입력사원코드 { get; set; }
		public DateTime? 최초입력일 { get; set; }

		[MaxLength(15)]
		public string 최초입력IP { get; set; }

		[MaxLength(10)]
		public string 수정사원코드 { get; set; }
		public DateTime? 수정일 { get; set; }

		[MaxLength(15)]
		public string 수정IP { get; set; }

		[MaxLength(12)]
		public string 외부연동작업번호 { get; set; }

		[MaxLength(16)]
		public string PDA번호 { get; set; }

}
}
