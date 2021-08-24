using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	//출고처리헤더정보
	public class BARPLUS_LDELIVER
	{
		[Key]
		public string CO_CD { get; set; }
		[Key]
		public string WORK_NB { get; set; }
		public string WORK_DT { get; set; }
		public string ISU_FG { get; set; }
		public string TR_CD { get; set; }
		public string ISU_DT { get; set; }
		public string WH_CD { get; set; }
		public string SO_FG { get; set; }
		public string EXCH_CD { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public Decimal EXCH_RT { get; set; }
		public string EMP_CD { get; set; }
		public string DEPT_CD { get; set; }
		public string DIV_CD { get; set; }
		public string VAT_FG { get; set; }
		public string UMVAT_FG { get; set; }
		public string APP_FG { get; set; }

		public string SHIP_CD { get; set; }
		public string REMARK_DC { get; set; }
		public string PLN_CD { get; set; }

	}
}
