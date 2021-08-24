using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class BARPLUS_LORCV_H
	{
		[Key]
		public string CO_CD { get; set; }//회사코드
		[Key]
		public string WORK_NB { get; set; }//작업번호
		public string WORK_DT { get; set; }//작업일자
		public string DOC_CD { get; set; }
		public string DOC_DT { get; set; }//실적일자
		public string WO_CD { get; set; }//지시번호

		[Column(TypeName = "decimal(3, 0)")]
		public decimal WOOP_SQ { get; set; }//지시전개순번
		
		[Column(TypeName = "decimal(17, 6)")]
		public decimal ITEM_QT { get; set; }//실적수량
		public string LOT_NB { get; set; }
		public string PJT_CD { get; set; }
		public string BASELOC_CD { get; set; }
		public string LOC_CD { get; set; }
		public string WF_FG { get; set; }
		public string MOVEBASELOC_CD { get; set; }//이동공정/입고창고코드
		public string MOVELOC_CD { get; set; }//이동작업장/입고장소코드
		public string QC_FG { get; set; }//검사구분
		public string BAD_YN { get; set; }
		public string BAD_CD { get; set; }
		public string REWORK_YN { get; set; }
		public string PLN_CD { get; set; }
		public string EQUIP_CD { get; set; }
		public string WTEAM_CD { get; set; }
		public string WSHFT_CD { get; set; }
		public string SUB_TP { get; set; }
		public string ITEM_CD { get; set; }
		public string M_DOC_CD { get; set; }
		public string DIV_CD { get; set; }//사업장코드
		public string DEPT_CD { get; set; }//부서코드
		public string EMP_CD { get; set; }//사원코드
		public string DOC_DC { get; set; }
		public string INSERT_ID { get; set; }
		public DateTime? INSERT_DT { get; set; }
		public string INSERT_IP { get; set; }
		public string MODIFY_ID { get; set; }
		public DateTime? MODIFY_DT { get; set; }
		public string MODIFY_IP { get; set; }
		public string PDA_ID { get; set; }
		public string PDA_NB { get; set; }


	}
}
