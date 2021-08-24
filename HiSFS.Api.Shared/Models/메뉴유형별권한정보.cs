using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 메뉴유형별권한정보 : 공통정보
    {
        [ForeignKey("메뉴")]
        //[Key]
        public int 메뉴순번 { get; set; }

        [MaxLength(10)]
        [ForeignKey("권한유형")]
        //[Key]
        public string 권한유형코드 { get; set; }

        public bool? 읽기권한 { get; set; }
        public bool? 등록권한 { get; set; }
        public bool? 변경권한 { get; set; }
        public bool? 삭제권한 { get; set; }

        //////

        public 메뉴정보 메뉴 { get; set; }
        public 공통코드 권한유형 { get; set; }
    }
}
