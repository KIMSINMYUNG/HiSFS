using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 주문서헤더정보 : 공통정보
    {
		[MaxLength(4)]
		[Key]
		public string 회사코드 { get; set; }
		[MaxLength(12)]
		[Key]
		public string 주문번호 { get; set; }

		[MaxLength(8)]
		public string 주문일자 { get; set; }
		[MaxLength(60)]
		public string 고객명 { get; set; }

		// 고객코드
		[MaxLength(10)]
		public string 거래처코드 { get; set; }

		[MaxLength(1)]
		public string 주문구분 { get; set; }
		[MaxLength(1)]
		public string 과세구분 { get; set; }
		[MaxLength(128)]
		public string 과세구분명 { get; set; }
		[MaxLength(40)]
		public string 납품처명 { get; set; }
		[MaxLength(20)]
		public string 담당자명 { get; set; }

		public List<주문서정보> 주문서정보상세 { get; set; }
	}
}
