using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class BARPLUS_LDELIVER_D
	{
		[Key]
		public string CO_CD { get; set; }
		[Key]
		public string WORK_NB { get; set; }
		[Key]
		[Column(TypeName = "decimal(5, 0)")]
		public decimal WORK_SQ { get; set; }
		public string ITEM_CD { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal SO_QT { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal ISU_QT { get; set; }
		
		public string SO_NB { get; set; }

		[Column(TypeName = "decimal(5, 0)")]

		public decimal SO_SQ { get; set; }
		public string LC_CD { get; set; }
	
		public string APP_FG { get; set; } = "0";


		public string LOT_NB { get; set; }
	}
}
