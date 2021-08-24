using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class BARPLUS_LSTKMOVE_D
	{
		[Key]
		public string CO_CD { get; set; }
		[Key]
		public string WORK_NB { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		[Key]
		public decimal WORK_SQ { get; set; }
		public string MOVE_NB { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public decimal MOVE_SQ { get; set; }
		public string ITEM_CD { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal REQ_QT { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal MOVE_QT { get; set; }
		public string LOT_NB { get; set; }
		public string WIP_YN { get; set; }
		public string ITEMPARENT_CD { get; set; }
		public string MGMT_CD { get; set; }
		public string PJT_CD { get; set; }
		public string WO_CD { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public decimal WOBOM_SQ { get; set; }

		public string APP_FG { get; set; } = "1";
		public string REMARK_DC { get; set; }

		
		public string USE_YN { get; set; } = "1";
		public string EXPIRE_YN { get; set; } = "1";


	}
}
