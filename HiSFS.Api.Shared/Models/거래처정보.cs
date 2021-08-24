using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 거래처정보 : 공통정보
    {

        [MaxLength(4)]
        [Key]
        public string 회사코드 { get; set; }

        [MaxLength(10)]
        [Key]
        public string 거래처코드 { get; set; }

        [MaxLength(60)]
        [Required(ErrorMessage = "거래처명은 필수입니다.")]
        public string 거래처명 { get; set; }

        [MaxLength(60)]
        public string 거래처약칭 { get; set; }


        [MaxLength(10)]
        [Required(ErrorMessage = "거래처구분은 필수입니다.")]
        [ForeignKey("거래처구분")]
        public string 거래처구분코드 { get; set; }     // B03


        [MaxLength(128)]
        public string 거래처구분명 { get; set; }


        [MaxLength(30)]
        public string 등록번호 { get; set; }

        [MaxLength(30)]
        public string 담당자 { get; set; }

        [MaxLength(40)]
        public string 업태 { get; set; }
        [MaxLength(40)]
        public string 종목 { get; set; }

        [MaxLength(300)]
        public string 주소 { get; set; }

        [MaxLength(150)]
        public string 주소2 { get; set; }

        [MaxLength(30)]
        public string 대표연락처 { get; set; }

        [MaxLength(30)]
        public string 담당자연락처 { get; set; }

        [MaxLength(30)]
        public string 공급가격 { get; set; }

        [MaxLength(100)]
        public string 팩스및비고 { get; set; }

        [MaxLength(100)]
        public string 이메일 { get; set; }

        [MaxLength(20)]
        public string 화물도착지  { get; set; }

        [MaxLength(20)]
        public string 거래1 { get; set; }

        [MaxLength(20)]
        public string 거래2 { get; set; }

        [MaxLength(20)]
        public string 거래3 { get; set; }

        [MaxLength(20)]
        public string 거래4 { get; set; }

        [MaxLength(20)]
        public string 거래5 { get; set; }


        public 공통코드 거래처구분 { get; set; }



    }
}
