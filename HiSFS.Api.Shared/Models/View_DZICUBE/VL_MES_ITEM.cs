using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class VL_MES_ITEM
	{
		/*
		아이큐브 물류             메 뉴  명 품목 정보
		VL_MES_ITEM             작성 일자	2020년 04월 02일

	     CO_CD 회사코드    nvarchar	4	no PK
		ITEM_CD 품번  nvarchar	30	no PK
		ITEM_NM 품명  nvarchar	40	yes
		ITEM_DC 규격 nvarchar	40	yes
		ACCT_FG 계정구분 nvarchar	1	no		0.원재료 1.부재료 2.제품 4.반제품 5.상품 6.저장품 7.비용 8.수익
		ACCT_FG_NM  계정구분명 nvarchar	128	yes
		ODR_FG  조달구분 nvarchar	1	no		0.구매 1.생산 8.Phantom
		ODR_FG_NM   조달구분명 nvarchar	128	yes		0.부 1.여
		UNIT_DC 재고단위 nvarchar	4	no
		UNITMANG_DC 관리단위 nvarchar	4	yes
		UNITCHNG_NB 환산계수 numeric	17,6	yes
		ITEMGRP_CD  품목군코드 nvarchar	10	yes
		ITEMGRP_NM  품목군명 nvarchar	40	yes
		L_CD    대분류코드 nvarchar	10	yes
		L_NM    대분류명 nvarchar	40	yes
		M_CD    중분류코드 nvarchar	10	yes
		M_NM    중분류명 nvarchar	40	yes
		S_CD    소분류코드 nvarchar	10	yes
		S_NM    소분류명 nvarchar	40	yes
		LOT_FG  LOT사용여부 nvarchar	1	yes		0.미사용 1.사용
		USE_YN  사용여부 nvarchar	1	no		0.미사용 1.사용
		TRMAIN_CD   주거래처코드 nvarchar	10	yes
		PACK_PO_QT  구매포장단위수량 numeric	17,6	yes
		PACK_PO_RT  구매포장단위여유수량 numeric	17,6	yes
		PACK_SO_QT  판매포장단위수량 numeric	17,6	yes
		PACK_SO_RT  판매포장단위여유수량 numeric	17,6	yes
		QC_FG   검사여부 nvarchar	1	yes		0.무검사 1.검사
		*/

		[MaxLength(4)]
		public string CO_CD { get; set; }

		[MaxLength(30)]
		public string ITEM_CD { get; set; }
		[MaxLength(40)]
		public string ITEM_NM { get; set; }
		[MaxLength(40)]
		public string ITEM_DC { get; set; }
		[MaxLength(1)]
		public string ACCT_FG { get; set; }
		[MaxLength(128)]
		public string ACCT_FG_NM { get; set; }
		[MaxLength(1)]
		public string ODR_FG { get; set; }
		[MaxLength(128)]
		public string ODR_FG_NM { get; set; }
		[MaxLength(4)]
		public string UNIT_DC { get; set; }

		[MaxLength(1)]
		public string QC_FG { get; set; }
		[MaxLength(1)]
		public string LOT_FG { get; set; }
		[MaxLength(1)]
		public string USE_YN { get; set; }

		[MaxLength(10)]
		public string TRMAIN_CD { get; set; }
		/*
		[MaxLength(4)]
		public string UNITMANG_DC { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public decimal UNITCHNG_NB { get; set; }

		[MaxLength(10)]
		public string ITEMGRP_CD { get; set; }
		[MaxLength(40)]
		public string ITEMGRP_NM { get; set; }
		[MaxLength(10)]
		public string L_CD { get; set; }
		[MaxLength(40)]
		public string L_NM { get; set; }
		[MaxLength(10)]
		public string M_CD { get; set; }
		[MaxLength(40)]
		public string M_NM { get; set; }
		[MaxLength(10)]
		public string S_CD { get; set; }
		[MaxLength(40)]
		public string S_NM { get; set; }
		[MaxLength(1)]
		public string LOT_FG { get; set; }
		[MaxLength(1)]
		public string USE_YN { get; set; }
		[MaxLength(10)]
		public decimal TRMAIN_CD { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal PACK_PO_QT { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal PACK_PO_RT { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal PACK_SO_QT { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal PACK_SO_RT { get; set; }
		[MaxLength(1)]
		public string QC_FG { get; set; }
		*/


	}
}
