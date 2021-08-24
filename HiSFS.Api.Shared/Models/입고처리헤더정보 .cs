using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 입고처리헤더정보 : 공통정보 
	{
		[MaxLength(4)]
		[Key]
		public string 회사코드 { get; set; }

		[MaxLength(12)]
		[Key]
		public string 작업번호 { get; set; }

	
		public DateTime? 작업일자 { get; set; }

		[MaxLength(1)]
		public string 입고구분 { get; set; }

		[MaxLength(12)]
		public string 입고번호 { get; set; }

		[MaxLength(10)]
		public string 거래처코드 { get; set; }

		public DateTime? 입고일자 { get; set; }

		[MaxLength(4)]
		public string 입고창고 { get; set; }

		[MaxLength(4)]
		public string 입고장소 { get; set; }

		[MaxLength(12)]
		[Required(ErrorMessage = "필수사항 입니다.")]
		public string 발주번호 { get; set; }

		[MaxLength(1)]
		public string 거래구분 { get; set; }

		[MaxLength(4)]
		public string 환종 { get; set; }

		[Column(TypeName = "decimal(17, 6)")]
		public Decimal 환율 { get; set; }

		[MaxLength(1)]
		public string LC여부 { get; set; }

		[MaxLength(10)]
		public string 사원코드 { get; set; }

		[MaxLength(4)]
		public string 부서코드 { get; set; }

		[MaxLength(4)]
		public string 사업장코드 { get; set; }

		[MaxLength(10)]
		public string 프로젝트코드 { get; set; }

		[MaxLength(1)]
		public string 과세구분 { get; set; }

		[MaxLength(1)]
		public string 작업구분 { get; set; }

		[MaxLength(10)]
		public string 관리구분코드 { get; set; }

		[MaxLength(12)]
		public string EXCST_NB { get; set; }

		[MaxLength(4)]
		public string 배부여부 { get; set; }

		[MaxLength(60)]
		public string 비고 { get; set; }

		[MaxLength(10)]
		public string 최초입력사원코드 { get; set; }
		public DateTime? 최초입력일 { get; set; }

		[MaxLength(15)]
		public string 최초입력IP { get; set; }

		[MaxLength(19)]
		public string 수정사원코드 { get; set; }
		public DateTime? 수정일 { get; set; }

		[MaxLength(15)]
		public string 수정IP { get; set; }

		[MaxLength(20)]
		public string DUMMY1 { get; set; }

		[MaxLength(20)]
		public string DUMMY2 { get; set; }

		[MaxLength(20)]
		public string DUMMY3 { get; set; }

		[MaxLength(5)]
		public string PLN_CD { get; set; }

		[MaxLength(12)]
		public string SO_NB3 { get; set; }

		[MaxLength(1)]
		public string UMVAT_FG { get; set; }

		[MaxLength(1)]
		public string APP_FG { get; set; }


	}
}
