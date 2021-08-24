using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 장소위치정보 : 공통정보
    {
        [MaxLength(4)]
        [Key,ForeignKey("회사")]
        public string 회사코드 { get; set; }


        [Key]
        [MaxLength(10)]
        public string 장소위치코드 { get; set; }


        [MaxLength(10)]
        [ForeignKey("장소")]
        public string 장소코드 { get; set; }


        [MaxLength(10)]
        public string 위치코드 { get; set; }


        [MaxLength(100)]
        [Required(ErrorMessage = "위치명은 필수 입니다.")]
        public string 위치명 { get; set; }

        [MaxLength(10)]
        //[Required(ErrorMessage = "위치분류는 필수 입니다.")]
        //[ForeignKey("위치분류")]
        public string? 위치분류코드 { get; set; }

        //------
        public 장소정보 장소 { get; set; }
        public 공통코드 위치분류 { get; set; }

        public 사업장 회사 { get; set; }

        // 20210210 추가
        public ICollection<보유품목위치정보> 보유품목위치모두 { get; set; }

        public IList<위치상세정보> 위치상세목록 { get; set; }
    }
}
