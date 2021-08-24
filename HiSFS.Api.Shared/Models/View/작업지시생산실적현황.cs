using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View
{
    public class 작업지시생산실적현황
    {

		public string 회사코드 { get; set; }

		public string 생산지시코드 { get; set; }


		public string 생산품코드 { get; set; }

		public string 공정단위코드 { get; set; }


		public string 생산품공정코드 { get; set; }

		public string 생산지시명 { get; set; }

		public string 사업장코드 { get; set; }

		public string 실적공정코드_창고코드 { get; set; }
		public string 실적작업장코드_장소코드 { get; set; }
		public string 재작업여부 { get; set; }

		public string LOTNO { get; set; }

		public string 일괄생산등록유무 { get; set; }

		public string 작업번호 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal 목표수량 { get; set; }


		[Column(TypeName = "decimal(17, 6)")]
		public decimal 실적수량 { get; set; }

		[Column(TypeName = "decimal(7, 3)")]
		public decimal 불량수량 { get; set; }
	}
}
