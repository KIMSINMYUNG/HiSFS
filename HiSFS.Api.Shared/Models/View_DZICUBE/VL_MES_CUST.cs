using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HiSFS.Api.Shared.Models.View_DZICUBE
{
	public class VL_MES_CUST
	{

		//아이큐브 물류             메 뉴  명 거래처 정보
		//VL_MES_CUST             작성 일자	2020년 04월 02일

	   //CO_CD 회사코드    nvarchar	4	no PK
	   //TR_CD 거래처코드   nvarchar	10	no PK
	   //TR_NM 거래처명    nvarchar	60	yes
	   //ATTR_NM 거래처약칭 nvarchar	60	yes
	   //TR_FG   거래처구분 nvarchar	1	no
	   //TR_FG_NM    거래처구분명 nvarchar	128	yes
	   //USE_YN  사용여부 nvarchar	1	yes
	   //REG_NB  사업자등록번호 nvarchar	30	yes
	   //CEO_NM  대표자 nvarchar	30	yes
	   //BUSINESS    업태 nvarchar	40	yes
	   //JONGMOK 종목 nvarchar	40	yes
	   //ADDR1   주소1 nvarchar	150	yes
	   //ADDR2   주소2 nvarchar	150	yes


	    [MaxLength(4)]
		public string CO_CD { get; set; }

		[MaxLength(10)]
		public string TR_CD { get; set; }

		[MaxLength(60)]
		public string TR_NM { get; set; }

		[MaxLength(60)]
		public string ATTR_NM { get; set; }

		[MaxLength(1)]
		public string TR_FG { get; set; }

		[MaxLength(128)]
		public string TR_FG_NM { get; set; }


		[MaxLength(1)]
		public string USE_YN { get; set; }


		[MaxLength(30)]
		public string REG_NB { get; set; }

		[MaxLength(30)]
		public string CEO_NM { get; set; }

		[MaxLength(40)]
		public string BUSINESS { get; set; }


		[MaxLength(40)]
		public string JONGMOK { get; set; }

		[MaxLength(150)]
		public string ADDR1 { get; set; }

		[MaxLength(150)]
		public string ADDR2 { get; set; }


		

	}
}
