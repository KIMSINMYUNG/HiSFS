using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 일괄생산실적헤더정보 : 공통정보
	{
		[Key]
		[MaxLength(4)]
		public string 회사코드 { get; set; }
		[Key]
		[MaxLength(12)]
		public string 작업번호 { get; set; }
		public DateTime? 작업일자 { get; set; }
		[MaxLength(12)]
		public string 실적번호 { get; set; }

		public DateTime? 실적일자 { get; set; }
		[MaxLength(4)]
		public string 사업장코드 { get; set; }
		[MaxLength(4)]
		public string 부서코드 { get; set; }
		[MaxLength(10)]
		public string 사원코드 { get; set; }
		[MaxLength(4)]
		public string 실적공정코드_창고코드 { get; set; }
		[MaxLength(4)]
		public string 실적작업장코드_장소코드 { get; set; }
		[MaxLength(1)]
		public string 재작업여부 { get; set; }
		[MaxLength(30)]
		public string 실적품번 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal 실적수량 { get; set; }
		[MaxLength(20)]
		public string LOTNO { get; set; }
		[MaxLength(10)]
		public string 프로젝트코드 { get; set; }
		[MaxLength(10)]
		public string 관리구분 { get; set; }
		[MaxLength(10)]
		public string 설비코드 { get; set; }
		[MaxLength(60)]
		public string 비고 { get; set; }
		[MaxLength(1)]
		public string 실적구분 { get; set; }
		[MaxLength(4)]
		public string 실적공정코드 { get; set; }
		[MaxLength(4)]
		public string 실적작업장코드 { get; set; }
		[MaxLength(5)]
		public string 작업자코드 { get; set; }

	}
}
