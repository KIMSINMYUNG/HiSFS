using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 생산품공정차수정보 : 공통정보
    {

        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

       
        [MaxLength(30)]
        //[Key]
        [Key,ForeignKey("생산품공정")]
        public string 생산품공정코드 { get; set; }
        [Key]
        public int 순번 { get; set; }
        [Required(ErrorMessage = "공정차수는 필수입니다.")]
        public int 공정차수 { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "공정단위는 필수입니다.")]
        [ForeignKey("공정단위")]
        public string 공정단위코드 { get; set; }

        [MaxLength(300)]
        public string 비고 { get; set; }
        //-----------------------
        public 생산품공정정보 생산품공정 { get; set; }
        public 공정단위정보 공정단위 { get; set; }

        public 사업장 회사 { get; set; }

    }
}
