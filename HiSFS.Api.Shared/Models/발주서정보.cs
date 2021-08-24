using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 발주서정보 : 공통정보
	{
        //[Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int 발주서번호 { get; set; }

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
		[Key,ForeignKey("헤더정보")]
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
		public string 검사구분 { get; set; }
		[MaxLength(1)]
		public string 과세구분 { get; set; }
		[MaxLength(128)]
		public string 과세구분명 { get; set; }
		[MaxLength(5)]
		public string 담당자코드 { get; set; }
		[MaxLength(20)]
		public string 담당자명 { get; set; }
		[MaxLength(300)]
		public string 비고 { get; set; }
		[Key]
		[Column(TypeName = "decimal(5, 0)")]
		public decimal 발주순번 { get; set; }
		[MaxLength(30)]
		public string 품번 { get; set; }
		[MaxLength(40)]
		public string 품명 { get; set; }
		[MaxLength(40)]
		public string 규격 { get; set; }
		[MaxLength(4)]
		public string 관리단위 { get; set; }
		[MaxLength(8)]
		public string 납기일 { get; set; }
		[MaxLength(8)]
		public string 출하예정일 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal 발주수량 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal 발주단가 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal 공급가 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal 부가세 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal 합계액 { get; set; }
		[MaxLength(10)]
		public string 관리구분코드 { get; set; }
		[MaxLength(40)]
		public string 관리구분명 { get; set; }
		[MaxLength(10)]
		public string 프로젝트 { get; set; }
		[MaxLength(30)]
		public string 프록젝트명 { get; set; }
		[MaxLength(60)]
		public string 비고_내역 { get; set; }
		[MaxLength(4)]
		public string 환종 { get; set; }
		[MaxLength(1)]
		public string 부가세구분 { get; set; }

		[MaxLength(1)]
		public string 발주완료유무 { get; set; }

		public 사업장 회사 { get; set; }

		public 발주서헤더정보 헤더정보 { get; set; }
	}
}
