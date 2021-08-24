using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 파일폴더정보 : 공통정보
    {
        [Key]
        public int 순번 { get; set; }
        [MaxLength(512)]
        public string 폴더명 { get; set; }
        [MaxLength(512)]
        public string 폴더경로 { get; set; }

        // ------
        public IList<파일정보> 파일목록 { get; set; }
    }
}
