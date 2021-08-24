using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class VL_MES_BOM
	{
		/*
		아이큐브 물류             메 뉴  명 BOM 정보
		ZL_DMS_VL_SBOM_WF 작성 일자	2020년 04월 02일
		컬럼명 타입 크기  NULL 허용 PK 여부   비고
		회사코드    nvarchar	4	no PK
		모품번 nvarchar	30	no PK
		모품명 nvarchar	40	yes
		모규격 nvarchar	40	yes
		모품목재고단위 nvarchar	4	yes
		순번  numeric	5,0	no PK
		자품번 nvarchar	30	no
		자품명 nvarchar	40	yes
		자규격 nvarchar	40	yes
		자품목재고단위 nvarchar	4	yes
		정미수량    numeric	17,6	no
		LOSS율(%)    numeric	17,6	no
		필요수량    numeric	17,6	no LOSS율 감안 수량
		외주구분 nvarchar	1	yes		0.무상 1.유상
		임가공구분   nvarchar	1	yes		0.자재 1.사급
		주거래처코드  nvarchar	10	yes
		주거래처명   nvarchar	60	yes
		시작일자    nvarchar	8	no
		종료일자    nvarchar	8	no
		사용여부    nvarchar	1	yes		0.미사용 1.사용
		*/

		[MaxLength(4)]
		public string CO_CD { get; set; }
		[MaxLength(30)]
		public string ITEMPARENT_CD { get; set; }
		[MaxLength(40)]
		public string ITEMPARENT_NM { get; set; }
		[MaxLength(40)]
		public string ITEMPARENT_DC { get; set; }
		[MaxLength(4)]
		public string ITEMPARENT_UNIT_DC { get; set; }

		[Column(TypeName = "decimal(5, 0)")]
		public decimal BOM_SQ { get; set; }
		[MaxLength(30)]
		public string ITEMCHILD_CD { get; set; }
		[MaxLength(40)]
		public string ITEMCHILD_NM { get; set; }
		[MaxLength(40)]
		public string ITEMCHILD_DC { get; set; }
		[MaxLength(4)]
		public string ITEMCHILD_UNIT_DC { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal JUST_QT { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal LOSS_RT { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal REAL_QT { get; set; }
		[MaxLength(1)]
		public string OUT_FG { get; set; }
		[MaxLength(1)]
		public string ODR_FG { get; set; }
		[MaxLength(10)]
		public string TRMAIN_CD { get; set; }
		[MaxLength(60)]
		public string ATTR_NM { get; set; }
		[MaxLength(8)]
		public string START_DT { get; set; }
		[MaxLength(8)]
		public string END_DT { get; set; }
		[MaxLength(1)]
		public string USE_YN { get; set; }


	}
}
