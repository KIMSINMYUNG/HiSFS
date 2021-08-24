using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 재고조정정보 : 공통정보
	{
		[MaxLength(4)]
		public string 회사코드 { get; set; }
		[MaxLength(12)]
		public string 조정번호 { get; set; }
		[Column(TypeName = "decimal(5, 0)")]
		public decimal? 조정순번 { get; set; }
		[MaxLength(1)]
		public string 조정구분 { get; set; }
		[MaxLength(4)]
		public string 조정구분명 { get; set; }
		[MaxLength(8)]
		public string 조정일자 { get; set; }
		[MaxLength(4)]
		public string 창고코드 { get; set; }
		[MaxLength(40)]
		public string 창고명 { get; set; }
		[MaxLength(4)]
		public string 장소코드 { get; set; }
		[MaxLength(40)]
		public string 장소명 { get; set; }
		[MaxLength(5)]
		public string 담당자코드 { get; set; }
		[MaxLength(20)]
		public string 담당자명 { get; set; }
		[MaxLength(30)]
		public string 품번 { get; set; }
		[MaxLength(40)]
		public string 품명 { get; set; }
		[MaxLength(40)]
		public string 규격 { get; set; }
		[MaxLength(4)]
		public string 단위 { get; set; }
		[MaxLength(4)]
		public string 관리단위 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal? 환산계수 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal? 조정수량 { get; set; }
		[Column(TypeName = "decimal(17, 6)")]
		public decimal? 단가 { get; set; }
		[Column(TypeName = "decimal(17, 4)")]
		public decimal? 조정금액 { get; set; }
		[MaxLength(20)]
		public string LOT번호 { get; set; }
		[MaxLength(10)]
		public string 관리구분 { get; set; }
		[MaxLength(40)]
		public string 관리구분명 { get; set; }
		[MaxLength(10)]
		public string 프로젝트코드 { get; set; }
		[MaxLength(30)]
		public string 프로젝트명 { get; set; }
		[MaxLength(60)]
		public string 비고_건 { get; set; }
		[MaxLength(60)]
		public string 비고_내역 { get; set; }
		[MaxLength(10)]
		public string 거래처 { get; set; }
		[MaxLength(60)]
		public string 거래처명 { get; set; }

	}
}
