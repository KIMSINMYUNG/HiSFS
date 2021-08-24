using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 수입실적등록정보
	{
		[MaxLength(4)]
		public string 회사코드 { get; set; } //CO_CD

		[MaxLength(30)]
		public string 품번 { get; set; }


		public string 발주번호 { get; set; } // WORK_NB

		[Column(TypeName = "decimal(3, 0)")]
		public decimal 발주순번 { get; set; } // WOOP_SQ

		public DateTime? 작업일자 { get; set; } // WORK_DT

		[MaxLength(12)]
		public string 실적번호 { get; set; }
		public DateTime? 실적일자 { get; set; } // DOC_DT

		[Column(TypeName = "decimal(17, 6)")]
		public decimal 실적수량 { get; set; } //ITEM_QT

		[Column(TypeName = "decimal(7, 3)")]
		public decimal 총수량 { get; set; }

		[Column(TypeName = "decimal(7, 3)")]
		public decimal 검사수량 { get; set; }

		[Column(TypeName = "decimal(7, 3)")]
		public decimal 합격수량 { get; set; }

		[Column(TypeName = "decimal(7, 3)")]
		public decimal 불량수량 { get; set; }

		[MaxLength(20)]
		public string LOT번호 { get; set; }

		[MaxLength(50)]
		public string 품목_LOT번호 { get; set; }

		[MaxLength(10)]
		public string 프로젝트코드 { get; set; }

		[MaxLength(1)]
		public string 처리구분 { get; set; }

		[MaxLength(4)]
		public string 이동공정_입고창고코드		{ get; set; } //MOVEBASELOC_CD

		[MaxLength(4)]
		public string 이동작업장_입고장소코드		{ get; set; } // MOVELOC_CD

		[MaxLength(1)]
		public string 검사구분 { get; set; }  // QC_FG

		[MaxLength(1)]
		public string 실적구분 { get; set; }

		[MaxLength(12)]
		public string 불량유형코드 { get; set; }

		[MaxLength(1)]
		public string 재작업여부 { get; set; }

		[MaxLength(5)]
		public string 작업자코드 { get; set; }

		[MaxLength(10)]
		public string 설비코드 { get; set; }
		[MaxLength(10)]
		public string 작업팀코드 { get; set; }
		public string 작업조코드 { get; set; }

		[MaxLength(4)]
		public string 사업장코드 { get; set; } //DIV_CD
		[MaxLength(4)]
		public string 부서코드 { get; set; } //DEPT_CD
		[MaxLength(10)]
		public string 사원코드 { get; set; } //EMP_CD
		[MaxLength(60)]
		public string 비고 { get; set; }
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

		[MaxLength(8)]
		public string PDA아이디 { get; set; }

		[MaxLength(12)]
		public string PDA번호 { get; set; }

		
	}
}
