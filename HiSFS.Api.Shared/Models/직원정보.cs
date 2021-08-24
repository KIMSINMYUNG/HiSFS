using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 직원정보 : 공통정보
    {
        [MaxLength(4)]
        [Key]
        public string 회사코드 { get; set; }

        // 더존 사번코드
        [Required(ErrorMessage = "사번은 필수입니다.")]
        [MaxLength(10)]
        [Key]
        public string 사번 { get; set; }


        [Required(ErrorMessage = "이름은 필수입니다.")]
        [MaxLength(20)]
        public string 사용자명 { get; set; }


        [MaxLength(8)]
        public string 식별번호 { get; set; }

		[MaxLength(14)]
        [ForeignKey("권한정보")]
		public string 식별인자 { get; set; }

		[MaxLength(10)]
        [Required(ErrorMessage = "부서는 필수입니다.")]
        [ForeignKey("부서")]
        public string 부서코드 { get; set; }

        [MaxLength(10)]
        [ForeignKey("권한")]
        public string 권한코드 { get; set; }

        //[Required(ErrorMessage = "입사일은 필수입니다.")]
        [MaxLength(8)]
        public string 입사일 { get; set; }

        [MaxLength(8)]
        public string 퇴사일 { get; set; }


        [MaxLength(10)]
        [ForeignKey("직급")]
        public string 직급코드 { get; set; }

        [MaxLength(10)]
        [ForeignKey("직책")]
        public string 직책코드 { get; set; }


        [MaxLength(300)]
        public string 주소 { get; set; }

        [MaxLength(200)]
        public string 상세주소 { get; set; }

        [MaxLength(100)]
        //[Required(ErrorMessage = "연락처는 필수입니다.")]
        public string 연락처1 { get; set; }
        [MaxLength(100)]
        public string 연락처2 { get; set; }
        [MaxLength(100)]
        public string 이메일 { get; set; }

        // ------
        public 부서정보 부서 { get; set; }
        public 공통코드 권한 { get; set; }
        public 공통코드 직급 { get; set; }
        public 공통코드 직책 { get; set; }
        public 직원권한정보 권한정보 { get; set; }
        //public 사업장 회사 { get; set; }

        [NotMapped]
        public IEnumerable<메뉴직원권한정보> 메뉴직원권한목록 { get; set; }
    }
}
