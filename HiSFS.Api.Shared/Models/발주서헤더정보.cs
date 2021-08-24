using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 발주서헤더정보 : 공통정보
    {
		[MaxLength(4)]
		[Key]
		public string 회사코드 { get; set; }
		[MaxLength(12)]
		[Key]
		public string 발주번호 { get; set; }
		
		[MaxLength(8)]
		public string 발주일 { get; set; }
		[MaxLength(10)]
		public string 거래처코드 { get; set; }
		[MaxLength(60)]
		public string 거래처명 { get; set; }
		[MaxLength(1)]
		public string 거래구분 { get; set; }
		[MaxLength(1)]
		public string 과세구분 { get; set; }
		[MaxLength(128)]
		public string 과세구분명 { get; set; }
		[MaxLength(20)]
		public string 담당자명 { get; set; }

		public List<발주서정보> 발주서정보상세 { get; set; }

	}
}
