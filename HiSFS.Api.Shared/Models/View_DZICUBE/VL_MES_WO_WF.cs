using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class VL_MES_WO_WF
	{
		/*
		아이큐브 물류             메 뉴  명 작업지시정보
		VL_MES_WO_WF 작성 일자	2020년 07월 21일
		CO_CD   회사코드 nvarchar	4	no
		WO_CD   지시번호 nvarchar	12	no
		ORD_DT  지시일 nvarchar	8	no
		COMP_DT 완료일 nvarchar	8	no
		ITEM_CD 품번 nvarchar	30	no		0. 부  1. 여
		ITEM_NM 품명 nvarchar	40	yes LS 고정값
		ITEM_DC 규격 nvarchar	40	yes
		UNIT_DC 관리단위 nvarchar	4	yes
		ITEM_QT 수량 numeric	17,6	no
		WOOP_SQ 전개순번 numeric	5,0	no
		BASELOC_CD  공정 nvarchar	4	no
		BASELOC_NM  공정명 nvarchar	40	yes
		LOC_CD  작업장 nvarchar	4	no
		LOC_NM  작업장명 nvarchar	40	yes
		LBR_UM  외주단가 numeric	17,6	no
		LBR_AM  외주금액 numeric	17,6	no
		EQUIP_CD    설비코드 nvarchar	10	yes
		EQUIP_NM    설비명 nvarchar	40	yes
		DOC_DC  비고 nvarchar	80	yes
		DOC_ST  지시상태 nvarchar	1	no
		DOC_ST_NM   지시상태명 nvarchar	2	no		0 계획, 1 확정, 2 마감
		WOC_FG  지시구분 nvarchar	1	yes
		WOC_FG_NM   지시구분명 nvarchar	5	no		0 생산지시, 1 재작업, 2 임가공, 4 외주, 5 작업지시
		DOC_FG  생산외주구분 nvarchar	1	no
		DOC_FG_NM   생산외주구분명 nvarchar	4	no		0.생산공정   1.외주공정
		WF_FG   처리구분 nvarchar	1	no
		WF_FG_NM    처리구분명 nvarchar	2	no		0.이동   1.입고
		QC_FG   검사구분 nvarchar	1	no
		QC_FG_NM    검사구분명 nvarchar	3	no		0.무검사   1.검사
		LOT_NB  LOT번호 nvarchar	20	yes
		TR_CD   거래처코드 nvarchar	10	yes
		TR_NM   거래처명 nvarchar	60	yes
		ATTR_NM 거래처약칭 nvarchar	60	yes
		SO_NB   주문번호 nvarchar	12	yes
		LN_SQ   주문순번 numeric	17,6	yes
		DIV_CD  사업장코드 nvarchar	4	no
		WTEAM_CD    작업팀 nvarchar	10	yes
		WTEAM_NM    작업팀명 nvarchar	40	yes
		WSHFT_CD    작업조 nvarchar	10	yes
		WSHFT_NM    작업조명 nvarchar	40	yes
		REMARK_DC   비고 nvarchar	60	yes
		*/
		[MaxLength(4)]
		public string CO_CD { get; set; }
		[MaxLength(12)]
		public string WO_CD { get; set; }
		[MaxLength(8)]
		public string ORD_DT { get; set; }
		[MaxLength(8)]
		public string COMP_DT { get; set; }
		[MaxLength(30)]
		public string ITEM_CD { get; set; }
		[MaxLength(40)]
		public string ITEM_NM { get; set; }
		[MaxLength(40)]
		public string ITEM_DC { get; set; }
		[MaxLength(4)]
		public string UNIT_DC { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal ITEM_QT { get; set; }
		[Column(TypeName = "decimal(5, 0)")]
		public decimal WOOP_SQ { get; set; }
		[MaxLength(4)]
		public string BASELOC_CD { get; set; }
		[MaxLength(40)]
		public string BASELOC_NM { get; set; }
		[MaxLength(4)]
		public string LOC_CD { get; set; }
		[MaxLength(40)]
		public string LOC_NM { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal LBR_UM { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal LBR_AM { get; set; }
		[MaxLength(10)]
		public string EQUIP_CD { get; set; }
		[MaxLength(40)]
		public string EQUIP_NM { get; set; }
		[MaxLength(80)]
		public string DOC_DC { get; set; }
		[MaxLength(1)]
		public string DOC_ST { get; set; }
		[MaxLength(2)]
		public string DOC_ST_NM { get; set; }
		[MaxLength(1)]
		public string WOC_FG { get; set; }
		[MaxLength(5)]
		public string WOC_FG_NM { get; set; }
		[MaxLength(1)]
		public string DOC_FG { get; set; }
		[MaxLength(4)]
		public string DOC_FG_NM { get; set; }
		[MaxLength(1)]
		public string WF_FG { get; set; }
		[MaxLength(2)]
		public string WF_FG_NM { get; set; }
		[MaxLength(1)]
		public string QC_FG { get; set; }
		[MaxLength(3)]
		public string QC_FG_NM { get; set; }
		[MaxLength(20)]
		public string LOT_NB { get; set; }
		[MaxLength(10)]
		public string TR_CD { get; set; }
		[MaxLength(60)]
		public string TR_NM { get; set; }
		[MaxLength(60)]
		public string ATTR_NM { get; set; }
		[MaxLength(12)]
		public string SO_NB { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal LN_SQ { get; set; }
		[MaxLength(4)]
		public string DIV_CD { get; set; }
		[MaxLength(10)]
		public string WTEAM_CD { get; set; }
		[MaxLength(40)]
		public string WTEAM_NM { get; set; }
		[MaxLength(10)]
		public string WSHFT_CD { get; set; }
		[MaxLength(40)]
		public string WSHFT_NM { get; set; }
		[MaxLength(60)]
		public string REMARK_DC { get; set; }



	}
}
