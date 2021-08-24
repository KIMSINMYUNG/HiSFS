using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 일괄생산실적상세정보 : 공통정보
	{
		[Key]
		[MaxLength(4)]
		public string 회사코드 { get; set; }
		
		[MaxLength(12)]
		[Key, ForeignKey("일괄생산실적헤더")]
		public string 작업번호 { get; set; }
		[MaxLength(5)]
		[Key]
		public string 작업순번 { get; set; }
		[MaxLength(12)]
		public string 실적번호 { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public decimal 실적순번 { get; set; }
		[MaxLength(4)]
		public string 사용공정_사용창고 { get; set; }
		[MaxLength(4)]
		public string 사용작업장_사용장소 { get; set; }
		[MaxLength(30)]
		public string 사용품번 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal 사용수량 { get; set; }
		[MaxLength(20)]
		public string LOTNO { get; set; }

		[MaxLength(50)]
		public string 품목_LOT번호 { get; set; }


		[MaxLength(60)]
		public string 비고 { get; set; }
		[MaxLength(1)]
		public string 창고구분 { get; set; }

		public 일괄생산실적헤더정보 일괄생산실적헤더 { get; set; }


}
}
