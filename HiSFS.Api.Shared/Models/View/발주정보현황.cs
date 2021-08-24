using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View
{
    public class 발주정보현황
    {
        [Key]
        public int 발주순번 { get; set; }

        public string 발주서명 { get; set; }

        public string 거래처명 { get; set; }

        public string 발주상태 { get; set; }
        public DateTime 발주일시 { get; set; }
        public DateTime? 입고예정일시 { get; set; }

        public string 비고 { get; set; }

    }
}
