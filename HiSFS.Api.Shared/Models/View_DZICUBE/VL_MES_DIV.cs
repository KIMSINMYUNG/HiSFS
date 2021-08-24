using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class VL_MES_DIV
	{

		//아이큐브 물류             메 뉴  명 사업장정보
		//ZL_DMS_VL_SDIV 작성 일자	2020년 04월 02일

		//CO_CD 회사코드    nvarchar	4		PK
		//DIV_CD  사업장코드 nvarchar	4		PK
		//DIV_NM  사업장명 nvarchar	30		


		[MaxLength(4)]
		public string CO_CD { get; set; }

		[MaxLength(4)]
		public string DIV_CD { get; set; }

		[MaxLength(30)]
		public string DIV_NM { get; set; }
	}
}
