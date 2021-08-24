using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class BARPLUS_LMTL_USE
	{
		[Key]
		public string CO_CD { get; set; } //회사코드
		[Key]
		public string WORK_NB { get; set; } //작업번호
		[Key]
		[Column(TypeName = "decimal(5, 0)")]
		public decimal WORK_SQ { get; set; } //작업순번
		public string WORK_DT { get; set; } //작업일자
		public string ITEM_CD { get; set; } //품번
		public string BASELOC_CD { get; set; } //사용공정
		public string LOC_CD { get; set; } //사용작업장
		public string WO_CD { get; set; } //지시번호
		public string WOC_FG { get; set; } //지시구분
		public string EMP_CD { get; set; } //사원코드
		public string DEPT_CD { get; set; } //부서코드
		public string USE_YN { get; set; } //사용여부
		public string UMU_FG { get; set; } //유무상구분
		public string EXPIRE_YN { get; set; } //유효여부
		public string WR_CD { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public decimal USE_SQ { get; set; }
		public string USE_DT { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal USE_QT { get; set; }
		public string LOT_NB { get; set; }
		public string PJT_CD { get; set; }
		public string DIV_CD { get; set; }
		public string REMARK_DC { get; set; }
		[Column(TypeName = "decimal(3, 0)")]
		public decimal WOOP_SQ { get; set; } //지시전개순번
		[Column(TypeName = "decimal(5, 0)")]
		public decimal WOBOM_SQ { get; set; } //소요자재순번
		public string MGMT_CD { get; set; }
		public string REMARK_DCK { get; set; }
		public string INSERT_ID { get; set; }
		public DateTime? INSERT_DT { get; set; }
		public string INSERT_IP { get; set; }
		public string MODIFY_ID { get; set; }
		public DateTime? MODIFY_DT { get; set; }
		public string MODIFY_IP { get; set; }
		public string EXWR_CD { get; set; }
		public string PDA_ID { get; set; }

	}
}
