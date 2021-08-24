using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class VL_MES_PLN
	{
		/*
		아이큐브 물류             메 뉴  명 물류담당자
		VL_MES_PLN 작성 일자	2020년 07월 21일
		CO_CD   회사코드 nvarchar	4	no
		PLN_CD  담당자코드 nvarchar	5	no
		PLN_NM  담당자명 nvarchar	20	yes
		EMP_CD  사원코드 nvarchar	10	yes
		EMP_NM  사원명 nvarchar	30	yes
		PLN_TEL 전화번호 nvarchar	30	yes
		PLN_FAX 팩스번호 nvarchar	30	yes
		PLN_CP  핸드폰번호 nvarchar	30	yes
		PLNS_CD 담당그룹코드 nvarchar	5	yes
		PLNS_NM 담당그룹명 nvarchar	20	yes
		FROM_DT 시작일 nvarchar	8	yes
		TO_DT   종료일 nvarchar	8	yes
		USE_YN  사용여부 nvarchar	1	no		0.미사용  1.사용  */

		[MaxLength(4)]
		public string CO_CD { get; set; }
		[MaxLength(5)]
		public string PLN_CD { get; set; }
		[MaxLength(20)]
		public string PLN_NM { get; set; }
		[MaxLength(10)]
		public string EMP_CD { get; set; }
		[MaxLength(30)]
		public string EMP_NM { get; set; }
		[MaxLength(30)]
		public string PLN_TEL { get; set; }
		[MaxLength(30)]
		public string PLN_FAX { get; set; }
		[MaxLength(30)]
		public string PLN_CP { get; set; }
		[MaxLength(5)]
		public string PLNS_CD { get; set; }
		[MaxLength(20)]
		public string PLNS_NM { get; set; }
		[MaxLength(8)]
		public string FROM_DT { get; set; }
		[MaxLength(8)]
		public string TO_DT { get; set; }
		[MaxLength(1)]
		public string USE_YN { get; set; }


	}
}
