using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class VL_MES_ADJUST
	{
		/*
		아이큐브 물류             메 뉴  명 재고조정
		VL_MES_ADJUST 작성 일자	2020년 07월 21일
		CO_CD   회사코드 nvarchar	4	no
		ADJUST_NB   조정번호 nvarchar	12	no
		ADJUST_SQ   조정순번 numeric	5	yes
		ADJUST_FG   조정구분 nvarchar	1	no
		ADJUST_FG_NM    조정구분명 nvarchar	4	yes
		ADJUST_DT   조정일자 nvarchar	8	no
		WH_CD   창고코드 nvarchar	4	no
		WH_NM   창고명 nvarchar	40	yes
		LC_CD   장소코드 nvarchar	4	no
		LC_NM   장소명 nvarchar	40	yes
		PLN_CD  담당자코드 nvarchar	5	yes
		PLN_NM  담당자명 nvarchar	20	yes
		ITEM_CD 품번 nvarchar	30	yes
		ITEM_NM 품명 nvarchar	40	yes
		ITEM_DC 규격 nvarchar	40	yes
		UNIT_DC 단위 nvarchar	4	yes
		UNITMANG_DC 관리단위 nvarchar	4	yes
		UNITCHNG_NB 환산계수 numeric	17,6	yes
		QT  조정수량 numeric	17,6	yes
		ADJUST_UM   단가 numeric	17,6	yes
		ADJUST_AM   조정금액 numeric	17,4	yes
		LOT_NB  LOT번호 nvarchar	20	yes
		MGMT_CD 관리구분 nvarchar	10	yes
		MGM_NM  관리구분명 nvarchar	40	yes
		PJT_CD  프로젝트코드 nvarchar	10	yes
		PJT_NM  프로젝트명 nvarchar	30	yes
		REMARK_DC_H 비고(건)   nvarchar	60	yes
		REMARK_DC_D 비고(내역)  nvarchar	60	yes
		TR_CD   거래처 nvarchar	10	yes
		TR_NM   거래처명 nvarchar	60	yes
		*/
		[MaxLength(4)]
		public string? CO_CD { get; set; }
		[MaxLength(12)]
		public string? ADJUST_NB { get; set; }
		[Column(TypeName = "decimal(5, 0)")]
		public decimal? ADJUST_SQ { get; set; }
		[MaxLength(1)]
		public string? ADJUST_FG { get; set; }
		[MaxLength(4)]
		public string? ADJUST_FG_NM { get; set; }
		[MaxLength(8)]
		public string? ADJUST_DT { get; set; }
		[MaxLength(4)]
		public string? WH_CD { get; set; }
		[MaxLength(40)]
		public string? WH_NM { get; set; }
		[MaxLength(4)]
		public string? LC_CD { get; set; }
		[MaxLength(40)]
		public string? LC_NM { get; set; }
		[MaxLength(5)]
		public string PLN_CD { get; set; }
		[MaxLength(20)]
		public string PLN_NM { get; set; }
		[MaxLength(30)]
		public string? ITEM_CD { get; set; }
		[MaxLength(40)]
		public string? ITEM_NM { get; set; }
		[MaxLength(40)]
		public string? ITEM_DC { get; set; }
		[MaxLength(4)]
		public string? UNIT_DC { get; set; }
		[MaxLength(4)]
		public string? UNITMANG_DC { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal? UNITCHNG_NB { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal? QT { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal? ADJUST_UM { get; set; }
		[Column(TypeName = "decimal(17, 4)")]
		public decimal? ADJUST_AM { get; set; }
		[MaxLength(20)]
		public string? LOT_NB { get; set; }
		[MaxLength(10)]
		public string? MGMT_CD { get; set; }
		[MaxLength(40)]
		public string? MGM_NM { get; set; }
		[MaxLength(10)]
		public string? PJT_CD { get; set; }
		[MaxLength(30)]
		public string? PJT_NM { get; set; }
		[MaxLength(60)]
		public string? REMARK_DC_H { get; set; }
		[MaxLength(60)]
		public string? REMARK_DC_D { get; set; }
		[MaxLength(10)]
		public string? TR_CD { get; set; }
		[MaxLength(60)]
		public string? TR_NM { get; set; }


	}
}
