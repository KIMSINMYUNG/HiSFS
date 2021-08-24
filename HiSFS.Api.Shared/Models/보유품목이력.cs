using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    /// <summary>
    /// 보유품목이력
    /// 
    /// 보유품목 이력은 동일한 보유품목의 경우 목록의 수량 합산이 보유품목정보의 수량과 동일해야 한다.
    /// 
    /// 장소1 -> 장소2 장소변경은, 수량이 0
    /// </summary>
    public class 보유품목이력 : 공통정보
    {
        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }

        //[Key]
        [MaxLength(30)]
        public string 보유품목코드 { get; set; }
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int 이력순번 { get; set; }


        /// B17
        /// 
        [MaxLength(10)]
        [ForeignKey("변경유형")]
        public string 변경유형코드 { get; set; }

        [MaxLength(10)]
        [ForeignKey("장소")]
        public string 장소코드 { get; set; }


        [MaxLength(10)]
        [ForeignKey("위치")]
        public string 장소위치코드 { get; set; }

        [Column(TypeName = "decimal(17, 6)")]
        public decimal 변경수량 { get; set; }

        [MaxLength(30)]
        public string 변경사유 { get; set; }

        [MaxLength(30)]
        public string 유형사유 { get; set; }

        [MaxLength(30)]
        [ForeignKey("연계보유품목")]
        public string 연계보유품목코드 { get; set; }
        public DateTime? 변경일시 { get; set; }


        [MaxLength(15)]

        public string 위치상세코드 { get; set; }

        [MaxLength(20)]
        public string LOT번호 { get; set; }

        [MaxLength(50)]
        public string 품목_LOT번호 { get; set; }

        // ------
        public 사업장 회사 { get; set; }
        public 공통코드 변경유형 { get; set; }
        public 장소정보 장소 { get; set; }
        public 장소위치정보 위치 { get; set; }
        public 보유품목정보 연계보유품목 { get; set; }
    }
}
