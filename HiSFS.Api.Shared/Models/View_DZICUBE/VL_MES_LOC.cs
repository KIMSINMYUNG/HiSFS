using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class VL_MES_LOC
	{
		/*
		아이큐브 물류             메 뉴  명 창고/공정/외주 정보
		ZL_DMS_VL_SLOC 작성 일자	2020년 04월 02일
		CO_CD   회사코드 nvarchar	4	no YES
		BASELOC_FG 공정구분    nvarchar	1	no		0.창고 1.생산 2.외주
		BASELOC_FG_NM   공정구분명 nvarchar	128	yes
		BASELOC_CD  창고/공정/외주코드 nvarchar	4	no YES
		BASELOC_NM 창고/공정/외주명 nvarchar	40	yes
		BASELOC_USE_YN  창고/공정/외주사용여부 nvarchar	1	no		0.미사용 1.사용
		LOC_CD  장소/작업장/외주처코드 nvarchar	4	no YES
		LOC_NM 장소/작업장/외주처명 nvarchar	40	yes
		LOC_USE_YN  장소/작업장/외주처사용여부 nvarchar	1	no		0.미사용 1.사용
		DIV_CD  사업장코드 nvarchar	4	no
		*/
		[MaxLength(4)]
		public string CO_CD { get; set; }
		[MaxLength(1)]
		public string BASELOC_FG { get; set; }
		[MaxLength(128)]
		public string BASELOC_FG_NM { get; set; }
		[MaxLength(4)]
		public string BASELOC_CD { get; set; }
		[MaxLength(40)]
		public string BASELOC_NM { get; set; }
		[MaxLength(1)]
		public string BASELOC_USE_YN { get; set; }
		[MaxLength(4)]
		public string LOC_CD { get; set; }
		[MaxLength(40)]
		public string LOC_NM { get; set; }
		[MaxLength(1)]
		public string LOC_USE_YN { get; set; }
		[MaxLength(4)]
		public string DIV_CD { get; set; }



	}
}
