using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 공정단위설비정보 : 공통정보
    {


        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(20)]
        //[Key]
        [Key,ForeignKey("공정단위")]
        public string 공정단위코드 { get; set; }
        [Required(ErrorMessage = "설비는 필수 입니다.")]

        [MaxLength(30)]
        //[Key]
        [Key,ForeignKey("설비")]
        public string 설비코드 { get; set; }

        // ------
        public 공정단위정보 공정단위 { get; set; }

        public 보유품목정보 설비 { get; set; }

        public 사업장 회사 { get; set; }
    }
}
