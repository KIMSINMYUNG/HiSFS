using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class VL_MES_DEPT
	{

		//아이큐브 물류             메 뉴  명 부서 정보
		//ZL_DMS_VL_SDEPT             작성 일자	2020년 04월 02일

		//CO_CD 회사코드    nvarchar	4	no YES
		//DEPT_CD 부서코드    nvarchar	4	no YES
		//DEPT_NM 부서명 nvarchar	20	yes
		//SECT_CD 부문코드 nvarchar	4	no
		//SECT_NM 부문명 nvarchar	20	yes
		//DIV_CD  사업장코드 nvarchar	4	no

		[MaxLength(4)]
		public string CO_CD { get; set; }

		[MaxLength(4)]
		public string DEPT_CD { get; set; }

		[MaxLength(20)]
		public string DEPT_NM { get; set; }

		[MaxLength(4)]
		public string SECT_CD { get; set; }

		[MaxLength(20)]
		public string SECT_NM { get; set; }

		[MaxLength(4)]
		public string DIV_CD { get; set; }





	}
}
