using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    /// <summary>
    /// 생산지시 별 공정차수 별 정보
    /// 작업자가 지정되지 않을 수도 있고 지정할 수도 있다.
    /// 작업자가 지정되지 않을 경우 어떤 작업자도 해당 공정을 진행할 수 있다.
    /// 작업자가 지정되었을 경우 그 작업자만 해당 공정을 진행할 수 있다.
    /// 
    /// 작업지시서에 표시되는 비고는 -- 공전단위 -> 생산품공정차수정보 -> 생산지시공정차수정보의 비고로 전달되며 최종 생산지시공정차수정보의 비고가 표시됨
    /// ※ 생산관리의 포인트는 생산을 누적하면서 동일한 실수를 반복하지 않도록 관리한느 것으로 비고란을 적극적으로 활용 / 시스템적으로 최종 작업자에게 잘 전달되도록 하는데 있다.
    /// </summary>
    public class 생산지시공정차수정보 : 공통정보
    {
        [MaxLength(20)]
        //[Key]
        [ForeignKey("생산지시")]
        public string 생산지시코드 { get; set; }
        //[Key]
        public int 공정차수 { get; set; }

        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(30)]
        public string 생산품공정코드 { get; set; }
        public int 생산품공정차수순번 { get; set; }

        [MaxLength(10)]
        [ForeignKey("작업자")]
        public string 작업자사번 { get; set; }

        [MaxLength(300)]
        public string 비고 { get; set; }


        // ------
        public 생산지시정보 생산지시 { get; set; }
        public 직원정보 작업자 { get; set; }
        [ForeignKey("생산품공정코드,생산품공정차수순번")]
        public 생산품공정차수정보 생산품공정차수 { get; set; }
        public 사업장 회사 { get; set; }
    }
}
