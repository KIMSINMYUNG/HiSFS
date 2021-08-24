using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 생산계획구매정보 : 생산계획공통
    {
        [MaxLength(20)]
        [ForeignKey("생산계획")]
        [Key]
        public string 생산계획코드 { get; set; }

        //------
        public 생산계획정보 생산계획 { get; set; }
    }
}
