using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 물류담당자정보 : 공통정보
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int 물류담당자번호 { get; set; }

		[MaxLength(4)]
		public string 회사코드 { get; set; }
		[MaxLength(5)]
		public string 담당자코드 { get; set; }
		[MaxLength(20)]
		public string 담당자명 { get; set; }
		[MaxLength(10)]
		public string 사원코드 { get; set; }
		[MaxLength(30)]
		public string 사원명 { get; set; }
		[MaxLength(30)]
		public string 전화번호 { get; set; }
		[MaxLength(30)]
		public string 팩스번호 { get; set; }
		[MaxLength(30)]
		public string 핸드폰번호 { get; set; }
		[MaxLength(5)]
		public string 담당그룹코드 { get; set; }
		[MaxLength(20)]
		public string 담당그룹명 { get; set; }
		[MaxLength(8)]
		public string 시작일 { get; set; }
		[MaxLength(8)]
		public string 종료일 { get; set; }
		[MaxLength(1)]
		public string 사용여부 { get; set; }

	}
}
