using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 액션로그 : 공통정보
    {
        [Key]
        public long 순번 { get; set; }

        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(10)]
        [ForeignKey("직원")]
        public string 직원사번 { get; set; }

        [MaxLength(10)]
        [ForeignKey("액션")]
        public string 액션코드 { get; set; }

        [MaxLength(50)]
        public string 액션인자 { get; set; }
        public DateTime 액션시각 { get; set; }


        [ForeignKey("연동장비")]
        public int 연동장비식별번호 { get; set; }

        //------
        public 직원정보 직원 { get; set; }
        public 액션정보 액션 { get; set; }
        public 연동장비정보 연동장비 { get; set; }
        public 사업장 회사 { get; set; }
    }
}
