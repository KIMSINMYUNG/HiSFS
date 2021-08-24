using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
	public class 보유품목일련정보 : 공통정보
	{

        [Required(ErrorMessage = "보유년월일은 필수입니다.")]
        [MaxLength(8)]
        public string 보유년월일 { get; set; }
        //품목코드  : 보유년월 : 순번

        [MaxLength(8)]
        public string? 생산년월일 { get; set; }


        [MaxLength(8)]
        public string? 출고년월일 { get; set; }

        [MaxLength(100)]
        //품목코드  : 보유년월 : 순번
        public string 일년번호 { get; set; }
        //[ForeignKey("거래처")]
        [MaxLength(10)]
        public string? 거래처코드 { get; set; }

        public int 순번 { get; set; }

        [MaxLength(30)]
        [ForeignKey("품목")]
        public string 품목코드 { get; set; }

        [MaxLength(20)]
        [ForeignKey("생산지시")]
        public string? 생산지시코드 { get; set; }

        [MaxLength(100)]
        [ForeignKey("보유품목일지")]
        public string 보유품목일지코드 { get; set; }

        [MaxLength(100)]
        public string 보유명 { get; set; }
        public DateTime? 보유일 { get; set; }

        [MaxLength(20)]
        public string LOT번호 { get; set; }

        [MaxLength(50)]
        public string 품목_LOT번호 { get; set; }

        public 보유품목일지 보유품목일지 { get; set; }
        public 품목정보 품목 { get; set; }

        public 생산지시정보 생산지시 { get; set; }

        public 거래처정보 거래처 { get; set; }

    }
}
