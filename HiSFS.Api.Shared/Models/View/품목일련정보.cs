using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HiSFS.Api.Shared.Models.View
{
    public class 품목일련정보
    {
        [MaxLength(10)]
        public string 사번 { get; set; }
        [MaxLength(30)]
        public string 품목코드 { get; set; }
    }
}
