using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 장소정보 : 공통정보
    {

        [MaxLength(4)]
        [Key,ForeignKey("회사")]
        public string 회사코드 { get; set; }

        // 더존 창고/공정/외주코드
        [Key]
        [MaxLength(10)]
        public string 장소코드 { get; set; }

        [Required(ErrorMessage = "장소명은 필수입니다.")]

        // 더존 창고/공정/외주명
        [MaxLength(40)]
        public string 장소명 { get; set; }

        // 공정구분
        [MaxLength(10)]
        //[Required(ErrorMessage = "장소유형은 필수입니다.")]
        [ForeignKey("장소유형")]
        public string? 장소유형코드 { get; set; }

        [MaxLength(10)]
        [ForeignKey("공정구분")]
        public string 공정구분코드 { get; set; }


        [MaxLength(128)]
        public string 공정구분명 { get; set; }


        [MaxLength(1)]
        public string 장소사용여부 { get; set; }

        // 장소/작업장/외주처코드  LOC_CD
        //[MaxLength(4)]
        //[Key]
        //public string 장소위치코드 { get; set; }

        //// 장소/작업장/외주처명  LOC_NM
        //[MaxLength(40)]
        //public string 장소위치명 { get; set; }

        ////장소/작업장/외주처사용여부 LOC_USE_YN
        //[MaxLength(1)]
        //public string 장소위치사용여부 { get; set; }

        [MaxLength(4)]
        public string 사업장코드 { get; set; }


        public 공통코드 장소유형 { get; set; }

        public 공통코드 공정구분 { get; set; }

        public 사업장 회사 { get; set; }

        public IList<장소위치정보> 장소위치목록 { get; set; }

    }
}
