using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 직원권한정보 : 공통정보
    {
		[MaxLength(14)]
		[Key]
		public string 식별인자 { get; set; }

		//[MaxLength(10)]
        //[Key]
        //[ForeignKey("직원정보")]
        //public string 사번 { get; set; }

        [MaxLength(20)]
        public string 암호 { get; set; }

        //[MaxLength(4)]
        //[ForeignKey("회사")]
        public string 회사코드 { get; set; }

        public bool 암호암호화유무 { get; set; } = false;

        // ------
       // public 직원정보 직원정보 { get; set; }
        public 사업장 회사 { get; set; }
    }
}
