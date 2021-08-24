using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class BARPLUS_LPRODUCTION_D
	{
		[Key]
		public string CO_CD { get; set; }
		[Key]
		public string WORK_NB { get; set; }
		[Key]
		public string WORK_SQ { get; set; }
		public string DOC_CD { get; set; }
		[Column(TypeName = "decimal(5, 0)")]
		public decimal USE_SQ { get; set; }
		public string BASELOC_CD { get; set; }
		public string LOC_CD { get; set; }
		public string CITEM_CD { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal USE_QT { get; set; }
		public string LOT_NB { get; set; }
		public string REMARK_DC { get; set; }
		public string BASELOC_FG { get; set; }

	}
}
