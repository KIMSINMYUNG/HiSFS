using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 도면정보 : 공통정보
    {
        /// <summary>
        /// D0000 ~
        /// </summary>
        [MaxLength(20)]
        [Key]
        public string 도면코드 { get; set; }

        [MaxLength(20)]
        public string 원도면코드 { get; set; }
        public int 관리차수 { get; set; }
        [Required(ErrorMessage = "도면번호는 필수 입니다.")]

        [MaxLength(64)]
        public string 도면번호 { get; set; }
        [Required(ErrorMessage = "도면명은 필수 입니다.")]
        [MaxLength(64)]
        public string 도면명 { get; set; }
        [MaxLength(64)]
        public string 도면영문명 { get; set; }

        [MaxLength(10)]
        [Required(ErrorMessage = "도면종류는 필수 입니다.")]
        [ForeignKey("도면종류")]
        public string 도면종류코드 { get; set; }

        [MaxLength(300)]
        public string 개요 { get; set; }

        [MaxLength(300)]
        public string 설명 { get; set;  }


        [ForeignKey("파일폴더")]
        public int? 파일폴더순번 { get; set; }
        //------

        public 파일폴더정보 파일폴더 { get; set; }
        public 공통코드 도면종류 { get; set; }
    }
}
