using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    /// <summary>
    /// 화면에서 제공하는 메뉴 기본 정보
    /// 
    /// 아이디는 M01 -> M0101 -> M010101 형태로 하이라키 구성된다.
    /// 메뉴명은 화면에 보여질 
    /// </summary>

    public class 메뉴정보 : 공통정보
    {
        /// <summary>
        /// 아이디는 M01 -> M0101 -> M010101 형태로 하이라키 구성된다.
        /// </summary>
        [Key]
        public int 순번 { get; set; }
        /// <summary>
        /// 화면에 보여지는 메뉴 이름
        /// </summary>
        [MaxLength(30)]
        public string 메뉴명 { get; set; }
        /// <summary>
        /// 페이지 경로 이름
        /// </summary>
        [MaxLength(100)]
        public string 경로명 { get; set; }
        /// <summary>
        /// 페이지 클래스 전태 이름
        /// </summary>
        [MaxLength(200)]
        public string 클래스명 { get; set; }
        [ForeignKey("상위메뉴")]
        public int? 상위메뉴순번 { get; set; }
        /// <summary>
        /// 메뉴 뎁스
        /// </summary>
        public int 뎁스 { get; set; }
        /// <summary>
        /// 정렬 순번
        /// </summary>
        public int 정렬순번 { get; set; }

        /////////////////////////////////////////////////////////////

        //------
        /// <summary>
        /// 상위 메뉴
        /// </summary>
        public 메뉴정보 상위메뉴 { get; set; }
        public IList<메뉴정보> 하위메뉴목록 { get; set; }

        //public 메뉴유형별권한정보 메뉴유형별권한 { get; set; }
        public IEnumerable<메뉴부서권한정보> 메뉴부서권한목록 { get; set; }
        [NotMapped]
        public 메뉴부서권한정보 메뉴부서권한 { get; set; }

        public IEnumerable<메뉴직원권한정보> 메뉴직원권한목록 { get; set; }
        [NotMapped]
        public 메뉴직원권한정보 메뉴직원권한 { get; set; }

        public IEnumerable<메뉴유형별권한정보> 메뉴유형별권한목록 { get; set; }
        [NotMapped]
        public 메뉴유형별권한정보 메뉴유형별권한 { get; set; }
    }
}
