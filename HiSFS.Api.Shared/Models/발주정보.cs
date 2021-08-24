using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HiSFS.Api.Shared.Models
{
    public class 발주정보 : 공통정보
    {
        [MaxLength(4)]
        [Key]
        public string 회사코드 { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int 발주순번 { get; set; }

        [MaxLength(200)]
        public string 발주서명 { get; set; }

        [MaxLength(10)]
        //[ForeignKey("거래처")]
        public string 거래처코드 { get; set; }


        [MaxLength(10)]
        public string 발주상태코드 { get; set; } // 입고, 입고지연, 진행중

        public DateTime? 발주일시 { get; set; }
        public DateTime? 입고예정일시 { get; set; }
       

        [MaxLength(256)]
        public string 비고 { get; set; }

        public 거래처정보 거래처 { get; set; }

        //public 사업장 회사  { get; set; }

        public List<발주정보상세> S_발주정보상세 { get; set; }
    }
}
