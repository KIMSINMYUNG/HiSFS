using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 주문서정보 : 공통정보
	{
		//[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int 주문서번호 { get; set; }

		[MaxLength(4)]
		[Key, ForeignKey("회사")]
		public string 회사코드 { get; set; }
		[MaxLength(4)]
		public string 사업장코드 { get; set; }
		[MaxLength(4)]
		public string 부서코드 { get; set; }
		[MaxLength(10)]
		public string 사원코드 { get; set; }
		[MaxLength(12)]
		[Key, ForeignKey("헤더정보")]
		public string 주문번호 { get; set; }
		[MaxLength(8)]
		public string 주문일자 { get; set; }
		[MaxLength(10)]
		public string 고객코드 { get; set; }
		[MaxLength(60)]
		public string 고객명 { get; set; }
		[MaxLength(1)]
		public string 주문구분 { get; set; }
		[MaxLength(1)]
		public string 과세구분 { get; set; }
		[MaxLength(128)]
		public string 과세구분명 { get; set; }
		[MaxLength(1)]
		public string 단가구분 { get; set; }
		[MaxLength(128)]
		public string 단가구분명 { get; set; }
		[MaxLength(5)]
		public string 납품처코드 { get; set; }
		[MaxLength(40)]
		public string 납품처명 { get; set; }
		[MaxLength(5)]
		public string 담당자코드 { get; set; }
		[MaxLength(20)]
		public string 담당자명 { get; set; }
		[MaxLength(20)]
		public string 관리번호 { get; set; }
		[MaxLength(60)]
		public string 헤더비고 { get; set; }
		[Key]
		[Column(TypeName = "decimal(5, 0)")]
		public decimal 순번 { get; set; }
		[MaxLength(30)]
		public string 품목코드 { get; set; }
		[MaxLength(40)]
		public string 품목명 { get; set; }
		[MaxLength(40)]
		public string 규격 { get; set; }
		[MaxLength(4)]
		public string 관리단위 { get; set; }
		[MaxLength(8)]
		public string 납기일 { get; set; }
		[MaxLength(8)]
		public string 출하예정일 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal 수량 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal 단가 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal 부가세단가 { get; set; }
		[Column(TypeName = "decimal(17, 4)")]
		public decimal 공급가 { get; set; }
		[Column(TypeName = "decimal(17, 4)")]
		public decimal SOV_AM { get; set; }
		[Column(TypeName = "decimal(17, 4)")]
		public decimal 합계액 { get; set; }
		[MaxLength(10)]
		public string 관리구분코드 { get; set; }
		[MaxLength(40)]
		public string 관리구분명 { get; set; }
		[MaxLength(10)]
		public string 프로젝트코드 { get; set; }
		[MaxLength(30)]
		public string 프로젝트명 { get; set; }
		[MaxLength(60)]
		public string 디테일비고 { get; set; }
		[MaxLength(1)]
		public string 마감여부 { get; set; }
		[MaxLength(1)]
		public string 검사구분 { get; set; }
		[MaxLength(4)]
		public string 환종 { get; set; }


		[MaxLength(1)]
		public string 주문완료유무 { get; set; }

		public 사업장 회사 { get; set; }

		public 주문서헤더정보 헤더정보 { get; set; }

	}
}
