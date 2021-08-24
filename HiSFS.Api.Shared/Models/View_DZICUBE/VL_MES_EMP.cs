using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class VL_MES_EMP 
	{
		//아이큐브 물류             메 뉴  명 사원 정보
		//ZL_DMS_VL_SEMP              작성 일자	2020년 04월 02일

		 //CO_CD 회사코드    nvarchar	4	no PK
		 //EMP_CD 사원코드    nvarchar	10	no PK
		 //KOR_NM 사원명 nvarchar	30	yes
		 //DEPT_CD 부서코드 nvarchar	4	yes
		 //DEPT_NM 부서명 nvarchar	20	yes
		 //JOIN_DT 입사일 nvarchar	8	yes
		 //RSR_DT  퇴사일 nvarchar	8	yes
		 //USR_YN  사용자여부 nvarchar	1	no

		[MaxLength(4)]
		public string CO_CD { get; set; }

		[MaxLength(10)]
		public string EMP_CD { get; set; }

		[MaxLength(30)]
		public string KOR_NM { get; set; }

		[MaxLength(4)]
		public string DEPT_CD { get; set; }

		[MaxLength(20)]
		public string DEPT_NM { get; set; }

		[MaxLength(8)]
		public string JOIN_DT { get; set; }

		[MaxLength(8)]
		public string RTR_DT { get; set; }
		
		[MaxLength(1)]
		public string USR_YN { get; set; }

	}
}
