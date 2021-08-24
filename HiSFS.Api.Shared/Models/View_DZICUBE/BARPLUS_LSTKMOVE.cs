using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class BARPLUS_LSTKMOVE
	{
		[Key]
		public string CO_CD { get; set; }
		[Key]
		public string WORK_NB { get; set; }
		public string WORK_DT { get; set; }
		public string MOVE_NB { get; set; }
		public string MOVE_DT { get; set; }
		public string GRP_FG { get; set; }
		public string IO_FG { get; set; } = "2";
		public string EMP_CD { get; set; }
		public string DEPT_CD { get; set; }
		public string DIV_CD { get; set; }
		public string FWH_CD { get; set; }
		public string FLC_CD { get; set; }
		public string TWH_CD { get; set; }
		public string TLC_CD { get; set; }
		public string PLN_CD { get; set; }
		public string REMARK_DC { get; set; }
		public string MOVE_FG { get; set; } = "1";
		public string APP_FG { get; set; } = "0";

	}
}
