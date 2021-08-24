using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 작업자생산실적정보 : 공통정보
	{

		[Required(ErrorMessage = "필수 입니다.")]
		[Key]
		[MaxLength(4)]
		public string 회사코드 { get; set; }

		[MaxLength(20)]
		[Key]
		public string 생산지시코드 { get; set; }

		[Key]
		public int 작업순번 { get; set; }

		[MaxLength(20)]
		public string 공정단위코드 { get; set; }


		[MaxLength(10)]
		[ForeignKey("작업자")]
		public string 작업자사번 { get; set; }

		[MaxLength(200)]
		public string 생산지시명 { get; set; }

		[MaxLength(30)]
        public string 생산품코드 { get; set; }

        [Column(TypeName = "decimal(17, 6)")]
		public decimal 실적수량 { get; set; }

		[Column(TypeName = "decimal(7, 3)")]
		public decimal 불량수량 { get; set; }

		[MaxLength(30)]
		public string 사용품번 { get; set; }


		[Column(TypeName = "decimal(17, 6)")]
		public decimal 사용수량 { get; set; }


		[MaxLength(4)]
		public string 실적공정코드_창고코드 { get; set; }
		[MaxLength(4)]
		public string 실적작업장코드_장소코드 { get; set; }

		public DateTime? 실적등록일 { get; set; }

		[MaxLength(300)]
		public string 비고 { get; set; }

		[MaxLength(1)]
		public string 일괄생산등록유무 { get; set; }

		[MaxLength(20)]
		public string LOT번호 { get; set; }

		[MaxLength(50)]
		public string 품목_LOT번호 { get; set; }

		

		//public 사업장 회사 { get; set; }

		public 직원정보 작업자 { get; set; }

		

	}
}
