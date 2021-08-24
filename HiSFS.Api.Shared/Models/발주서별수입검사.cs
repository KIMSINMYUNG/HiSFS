using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 발주서별수입검사 : 공통정보
	{
        //[Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int 발주서번호 { get; set; }

        [MaxLength(4)]
		[Key, ForeignKey("회사")]
		public string 회사코드 { get; set; }

		[MaxLength(12)]
		[Key]
		public string 발주번호 { get; set; }

		[Key]
		[Column(TypeName = "decimal(5, 0)")]
		public decimal 발주순번 { get; set; }


		[MaxLength(4)]
		public string 사업장코드 { get; set; }
		[MaxLength(4)]
		public string 부서코드 { get; set; }
		[MaxLength(10)]
		public string 사원코드 { get; set; }
		

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

		[MaxLength(1)]
		public string 수입검사완료유무 { get; set; }


		public 사업장 회사 { get; set; }

		//B2004 완료
		[MaxLength(10)]
		public string 실행상태코드 { get; set; }

		
		[Column(TypeName = "decimal(7, 3)")]
		public decimal 검사수량 { get; set; }

		[Column(TypeName = "decimal(7, 3)")]
		public decimal 합격수량 { get; set; }

		[Column(TypeName = "decimal(7, 3)")]
		public decimal 불량수량 { get; set; }

		[Column(TypeName = "decimal(7, 3)")]
		public decimal 실적수량 { get; set; }

		public DateTime? 시작일 { get; set; }
		public DateTime? 종료일 { get; set; }

		[NotMapped]
		public string 상세 { get; set; }

		[MaxLength(1)] //0.미입고,  1.입고
		public string 입고여부 { get; set; }

		[MaxLength(1)] //0.미완료,  1.완료
		public string 품질검사완료여부 { get; set; }
		


	}
}
