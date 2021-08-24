using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 사업장 : 공통정보
	{
		[MaxLength(4)]
		[Key]
		public string 회사코드 { get; set; }

		[MaxLength(4)]
		public string 사업장코드 { get; set; }

		[MaxLength(30)]
		public string 사업장명 { get; set; }

	}
}
