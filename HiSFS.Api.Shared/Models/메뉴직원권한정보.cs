using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 메뉴직원권한정보 : 공통정보
    {
        //[Key]
        [ForeignKey("메뉴")]
        public int 메뉴순번 { get; set; }
        //[Key]
        [MaxLength(10)]
        [ForeignKey("직원")]
        public string 직원사번 { get; set; }

        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        public bool? 읽기권한 { get; set; }
        public bool? 등록권한 { get; set; }
        public bool? 변경권한 { get; set; }
        public bool? 삭제권한 { get; set; }

        // -----------------------
        public 메뉴정보 메뉴 { get; set; }
        public 직원정보 직원 { get; set; }
        public 사업장 회사 { get; set; }
    }
}
