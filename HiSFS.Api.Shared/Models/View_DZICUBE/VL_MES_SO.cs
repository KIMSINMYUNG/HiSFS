﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class VL_MES_SO
	{
		/*
		아이큐브 물류             메 뉴  명 주문정보
		VL_MES_SO 작성 일자	2020년 04월 02일
		CO_CD   회사코드 nvarchar	4	no
	DIV_CD  사업장코드 nvarchar	4	no
	DEPT_CD 부서코드 nvarchar	4	no
	EMP_CD  사원코드 nvarchar	10	no
	SO_NB   주문번호 nvarchar	12	no
	SO_DT   주문일자 nvarchar	8	no
	TR_CD   고객코드 nvarchar	10	no
	TR_NM   고객명 nvarchar	60	yes
	SO_FG   주문구분 nvarchar	1	no		0.DOMESTIC 1.LOCAL L/C 2.구매승인서 3.MASTER L/C 4.T/T 5.D/A 6.D/P 7.유상사급
	VAT_FG  과세구분 nvarchar	1	no		0.과세 1.영세 2.면세 3.기타
	VAT_NM  과세구분명 nvarchar	128	yes
	UMVAT_FG    단가구분 nvarchar	1	no		0.부가세미포함 1.부가세 포함
	UMVAT_NM 단가구분명   nvarchar	128	yes
	SHIP_CD 납품처코드 nvarchar	5	yes
	SHIP_NM 납품처명 nvarchar	40	yes
	PLN_CD  담당자코드 nvarchar	5	yes
	PLN_NM  담당자명 nvarchar	20	yes
	DUMMY1  관리번호 nvarchar	20	yes
	REMARK_DC   헤더비고 nvarchar	60	yes
	SO_SQ   순번 numeric	5,0	no
	ITEM_CD 품목코드 nvarchar	30	yes
	ITEM_NM 품목명 nvarchar	40	yes
	ITEM_DC 규격 nvarchar	40	yes
	UNITMANG_DC 관리단위 nvarchar	4	yes
	DUE_DT  납기일 nvarchar	8	no
	SHIPREQ_DT  출하예정일 nvarchar	8	yes
	SO_QT   수량 numeric	17,6	yes
	SO_UM   단가 numeric	17,6	yes
	VAT_UM  부가세단가 numeric	17,6	yes
	SOG_AM  공급가 numeric	17,4	yes
	SOV_AM  부가세단가 numeric	17,4	yes
	SOH_AM  합계액 numeric	17,4	yes
	MGMT_CD 관리구분코드 nvarchar	10	yes
	MGM_NM  관리구분명 nvarchar	40	yes
	PJT_CD  프로젝트코드 nvarchar	10	yes
	PJT_NM  프로젝트명 nvarchar	30	yes
	REMARK_DC_D 디테일비고 nvarchar	60	yes
	EXPIRE_YN   마감여부 nvarchar	1	no		0.마감 1.진행
	QC_FG   검사구분 nvarchar	1	yes		0.미검사 1.검사
	EXCH_CD 환종 nvarchar	4	no
	EXCH_UM 외화단가 numeric	17,6	yes
	EXCH_AM 외화금액 numeric	17,4	yes
		*/

		[MaxLength(4)]
		public string CO_CD { get; set; }
		[MaxLength(4)]
		public string DIV_CD { get; set; }
		[MaxLength(4)]
		public string DEPT_CD { get; set; }
		[MaxLength(10)]
		public string EMP_CD { get; set; }
		[MaxLength(12)]
		public string SO_NB { get; set; }
		[MaxLength(8)]
		public string SO_DT { get; set; }
		[MaxLength(10)]
		public string TR_CD { get; set; }
		[MaxLength(60)]
		public string TR_NM { get; set; }
		[MaxLength(1)]
		public string SO_FG { get; set; }
		[MaxLength(1)]
		public string VAT_FG { get; set; }
		[MaxLength(128)]
		public string VAT_NM { get; set; }
		[MaxLength(1)]
		public string UMVAT_FG { get; set; }
		[MaxLength(128)]
		public string UMVAT_NM { get; set; }
		[MaxLength(5)]
		public string SHIP_CD { get; set; }
		[MaxLength(40)]
		public string SHIP_NM { get; set; }
		[MaxLength(5)]
		public string PLN_CD { get; set; }
		[MaxLength(20)]
		public string PLN_NM { get; set; }
		[MaxLength(20)]
		public string DUMMY1 { get; set; }
		[MaxLength(60)]
		public string REMARK_DC { get; set; }
		[Column(TypeName = "decimal(5, 0)")]
		public decimal SO_SQ { get; set; }
		[MaxLength(30)]
		public string ITEM_CD { get; set; }
		[MaxLength(40)]
		public string ITEM_NM { get; set; }
		[MaxLength(40)]
		public string ITEM_DC { get; set; }
		[MaxLength(4)]
		public string UNITMANG_DC { get; set; }
		[MaxLength(8)]
		public string DUE_DT { get; set; }
		[MaxLength(8)]
		public string SHIPREQ_DT { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal SO_QT { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal SO_UM { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal VAT_UM { get; set; }
		[Column(TypeName = "decimal(17, 4)")]
		public decimal SOG_AM { get; set; }
		[Column(TypeName = "decimal(17, 4)")]
		public decimal SOV_AM { get; set; }
		[Column(TypeName = "decimal(17, 4)")]
		public decimal SOH_AM { get; set; }
		[MaxLength(10)]
		public string MGMT_CD { get; set; }
		[MaxLength(40)]
		public string MGM_NM { get; set; }
		[MaxLength(10)]
		public string PJT_CD { get; set; }
		[MaxLength(30)]
		public string PJT_NM { get; set; }
		[MaxLength(60)]
		public string REMARK_DC_D { get; set; }
		[MaxLength(1)]
		public string EXPIRE_YN { get; set; }
		[MaxLength(1)]
		public string QC_FG { get; set; }
		[MaxLength(4)]
		public string EXCH_CD { get; set; }
		//[Column(TypeName = "decimal(17, 6)")]
		//public decimal EXCH_UM { get; set; }
		//[Column(TypeName = "decimal(17, 4)")]
		//public decimal EXCH_AM { get; set; }

	}
}
