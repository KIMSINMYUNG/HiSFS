using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 공정정보 : 공통정보
    {
        [MaxLength(10)]
        [Key]
        public string 공정코드 { get; set; }
        [Required(ErrorMessage = "공정명은 필수 입니다.")]

        [MaxLength(30)]
        public string 공정명 { get; set; }
        [Required(ErrorMessage = "공정유형은 필수 입니다.")]

        [MaxLength(10)]
        [ForeignKey("공정유형")]
        public string 공정유형코드 { get; set; }


        public bool 설비사용유무 { get; set; }
        //[ForeignKey("공정설비")]
        //public string 공정설비품목코드 { get; set; }
        //[ForeignKey("설비유형")]

        [MaxLength(10)]
        public string 설비유형코드 { get; set; }
        //[ForeignKey("장소")]
        //public string 장소코드 { get; set; }
        //[ForeignKey("위치")]
        //public string 위치코드 { get; set; }
        //------
        public 공통코드 공정유형 { get; set; }
        //public 품목정보 공정설비 { get; set; }
        public 공통코드 설비유형 { get; set; }
        //public 장소정보 장소 { get; set; }
        //public 장소위치정보 위치 { get; set; }
    }
}
