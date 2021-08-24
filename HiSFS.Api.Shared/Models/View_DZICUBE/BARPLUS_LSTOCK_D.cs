using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class BARPLUS_LSTOCK_D
	{ 

		[Key]
		public string CO_CD { get; set; }
		[Key]
		public string WORK_NB { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		[Key]
		public Decimal WORK_SQ { get; set; }
		public string RCV_NB { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public Decimal RCV_SQ { get; set; }
		public string ITEM_CD { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public Decimal PO_QT { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public Decimal RCV_QT { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public Decimal RCV_UM { get; set; }

		[Column(TypeName = "decimal(17, 4)")]
		public Decimal RCVG_AM { get; set; }

		[Column(TypeName = "decimal(17, 4)")]
		public Decimal RCVV_AM { get; set; }

		[Column(TypeName = "decimal(17, 4)")]
		public Decimal RCVH_AM { get; set; }
		public string EXCH_CD { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public Decimal EXCH_RT { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public Decimal EXCH_UM { get; set; }

		[Column(TypeName = "decimal(17, 4)")]
		public Decimal EXCH_AM { get; set; }
		public string LOT_NB { get; set; }
		public string PO_NB { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public Decimal PO_SQ { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public Decimal REQ_NB { get; set; }
		public string REQ_SQ { get; set; }
		public string IBL_NB { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public Decimal IBL_SQ { get; set; }
		public string USE_YN { get; set; }
		public string EXPIRE_YN { get; set; }
		public string UM_FG { get; set; }
		public string LC_CD { get; set; }

		[Column(TypeName = "decimal(15, 4)")]
		public Decimal CONF_NB3 { get; set; }
		public string SPEC_DC3 { get; set; }
		public string REMARK_DC { get; set; }
		public string BAR_DT { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public Decimal BAR_SQ { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public Decimal CLS_QT { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public Decimal VAT_UM { get; set; }
		public string QC_NB { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public Decimal QC_SQ { get; set; }
		public string RCV_NB_ORG { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public Decimal RCV_SQ_ORG { get; set; }
		public string MGMT_CD { get; set; }
		public string PJT_CD { get; set; }
		public string ID_ID { get; set; }
		public string EXRCV_NB { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public Decimal EXRCV_SQ { get; set; }
		public string INSERT_ID { get; set; }
		public DateTime? INSERT_DT { get; set; }
		public string INSERT_IP { get; set; }
		public string MODIFY_ID { get; set; }
		public DateTime? MODIFY_DT { get; set; }
		public string MODIFY_IP { get; set; }
		public string DUMMY1 { get; set; }
		public string DUMMY2 { get; set; }
		public string DUMMY3 { get; set; }
		public string APP_FG { get; set; }

	}
}
