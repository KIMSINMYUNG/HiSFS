using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class BARPLUS_LSTOCK
	{
		[Key]
		public string CO_CD { get; set; }
		[Key]
		public string WORK_NB { get; set; }
		public string WORK_DT { get; set; }
		public string RCV_FG { get; set; }
		public string RCV_NB { get; set; }
		public string TR_CD { get; set; }
		public string RCV_DT { get; set; }
		public string WH_CD { get; set; }
		public string LC_CD { get; set; }
		public string PO_NB { get; set; }
		public string PO_FG { get; set; }
		public string EXCH_CD { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public Decimal EXCH_RT { get; set; }
		public string LC_YN { get; set; }
		public string EMP_CD { get; set; }
		public string DEPT_CD { get; set; }
		public string DIV_CD { get; set; }
		public string PJT_CD { get; set; }
		public string VAT_FG { get; set; }
		public string MAP_FG { get; set; }
		public string MGMT_CD { get; set; }
		public string EXCST_NB { get; set; }
		public string DIST_YN { get; set; }
		public string REMARK_DC { get; set; }
		public string INSERT_ID { get; set; }
		public DateTime? INSERT_DT { get; set; }
		public string INSERT_IP { get; set; }
		public string MODIFY_ID { get; set; }
		public DateTime? MODIFY_DT { get; set; }
		public string MODIFY_IP { get; set; }
		public string DUMMY1 { get; set; }
		public string DUMMY2 { get; set; }
		public string DUMMY3 { get; set; }
		public string PLN_CD { get; set; }
		public string SO_NB3 { get; set; }
		public string UMVAT_FG { get; set; }
		public string APP_FG { get; set; }

	}
}
