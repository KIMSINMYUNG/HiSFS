using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class BARPLUS_LPRODUCTION
	{
		[Key]
		public string CO_CD { get; set; }
		[Key]
		public string WORK_NB { get; set; }
		public string WORK_DT { get; set; }
		public string DOC_CD { get; set; }
		public string DOC_DT { get; set; }
		public string DIV_CD { get; set; }
		public string DEPT_CD { get; set; }
		public string EMP_CD { get; set; }
		public string BASELOC_CD { get; set; }
		public string LOC_CD { get; set; }
		public string REWORK_YN { get; set; }
		public string PITEM_CD { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal ITEM_QT { get; set; }
		public string LOT_NB { get; set; }
		public string PJT_CD { get; set; }
		public string MGMT_CD { get; set; }
		public string EQUIP_CD { get; set; }
		public string REMARK_DC { get; set; }
		public string BASELOC_FG { get; set; }
		public string WR_WH_CD { get; set; }
		public string WR_LC_CD { get; set; }
		public string PLN_CD { get; set; }

	}
}
