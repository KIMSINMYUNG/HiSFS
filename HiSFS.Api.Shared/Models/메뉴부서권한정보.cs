using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    /// <summary>
    /// 부서 별 메뉴권한 정보 설정
    /// 
    /// 부서는 상위 하위 관계가 있으며 상위 권한설정에 의해 하위 권한이 동일하게 적용된다.
    /// 만약, 개별 하위 권한설정을 하면 하위 권한이 더 우선순위가 높게 적용되도록 한다.
    /// 작업자 개별 권한설정을 통해 작업자가 속한 부서와 상관없이 작업자의 권한 설정을 별도로 할 수 도 있다.
    /// 
    /// 읽기권한이 없을 경우 메뉴 노출을 시키지 않는다.
    /// 등록/변경/삭제권한은 각각 항목 등록/변경/삭제 개별 권한 설정을 할 수 있도록 한다.
    /// </summary>
    public class 메뉴부서권한정보 : 공통정보
    {

        [ForeignKey("메뉴")]
        public int 메뉴순번 { get; set; }

        [MaxLength(10)]
        //[Key]
        [ForeignKey("부서")]
        public string 부서코드 { get; set; }

        [MaxLength(4)]
        //[Key]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        public bool? 읽기권한 { get; set; }
        public bool? 등록권한 { get; set; }
        public bool? 변경권한 { get; set; }
        public bool? 삭제권한 { get; set; }

        // -----------------------
        public 메뉴정보 메뉴 { get; set; }
        public 부서정보 부서 { get; set; }
        public 사업장 회사 { get; set; }



    }
}
