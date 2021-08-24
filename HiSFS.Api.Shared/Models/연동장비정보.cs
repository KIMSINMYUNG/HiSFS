using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 연동장비정보 : 공통정보
    {
        [Key]
        public int 식별번호 { get; set; }

        [MaxLength(100)]
        public string 식별코드 { get; set; }
        [MaxLength(256)]
        public string 장비명 { get; set; }
        [MaxLength(256)]
        public string 에이전트명 { get; set; }

        [MaxLength(10)]
        [ForeignKey("연동장비유형")]
        public string 연동장비유형코드 { get; set; }

        [MaxLength(50)]
        public string 테스트 { get; set; }

        public DateTime 등록시각 { get; set; }
        public DateTime? 승인시각 { get; set; }

        //------
        public 공통코드 연동장비유형 { get; set; }
    }
}
