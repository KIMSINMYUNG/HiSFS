using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 생산품공정정보 : 공통정보
    {
        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(30)]
        [Key]
        public string 생산품공정코드 { get; set; }

        [MaxLength(200)]
        [Required(ErrorMessage = "생산품공정명은 필수입니다.")]
        public string 생산품공정명 { get; set; }

        [MaxLength(30)]
        [Required(ErrorMessage = "생산품은 필수입니다.")]
        [ForeignKey("생산품")]
        public string 생산품코드 { get; set; }
        public int 관리차수 { get; set; }

        [NotMapped]
        public int 공정차수 { get; set; }
        //----------------------
        public 품목정보 생산품 { get; set; }
        public IList<생산품공정차수정보> 생산품공정차수목록 { get; set; }


        public 사업장 회사 { get; set; }
    }
}
