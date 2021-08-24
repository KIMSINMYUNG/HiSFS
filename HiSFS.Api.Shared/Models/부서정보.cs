using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 부서정보 : 공통정보
    {

        [Key, ForeignKey("회사")]
        [MaxLength(4)]
        public string 회사코드 { get; set; }

        //[MaxLength(4)]
        //[ForeignKey("회사")]
        //public string 사업장코드 { get; set; }


        [MaxLength(10)]
        [Key]
        public string 부서코드 { get; set; }

        [MaxLength(20)]
        public string 부서명 { get; set; }

        //[ForeignKey("상위부서")]
        //public string 상위부서코드 { get; set; }
        //[ForeignKey("권한")]
        //public string 권한코드 { get; set; }


        public int 정렬순번 { get; set; }
        public bool 선택가능유무 { get; set; }


        [MaxLength(10)]
        public string 연계부서코드 { get; set; }

        [MaxLength(10)]

        public string 부문코드 { get; set; }

        [MaxLength(20)]
        public string 부문명 { get; set; }


        [MaxLength(4)]
        public string 사업장코드 { get; set; }


        public 사업장 회사 { get; set; }
        // ------
        //public 부서정보 상위부서 { get; set; }
        //public 공통코드 권한 { get; set; }

        //public IList<부서정보> 하위부서목록 { get; set; }
    }
}
