using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models.View
{
    /// <summary>
    /// 생산계획 기준 필요 원자재/부품/반제품에 대한 보유현황을 나타낸다.
    /// </summary>
    public class 생산계획자재보유현황 : 공통정보
    {
        /// <summary>
        /// 생산계획 기준
        /// </summary>
        [Key]
        public string 생산계획코드 { get; set; }
        /// <summary>
        /// 필요 자재 대비 (필요자재는 공정별 원자재/부품 및 생산 반제품까지 포함한다.)
        /// </summary>
        [ForeignKey("필요자재")]
        public string 필요자재코드 { get; set; }
        [ForeignKey("자재유형")]
        public string 자재유형코드 { get; set; }
        /// <summary>
        /// 보유 자재
        /// </summary>
        [ForeignKey("보유자재")]
        public string 보유자재코드 { get; set; }
        /// <summary>
        /// 필요 수량
        /// </summary>
        public decimal 필요수량 { get; set; }
        /// <summary>
        /// 계획수량 대비 총 필요수량
        /// </summary>
        public decimal 총필요수량 { get; set; }
        /// <summary>
        /// 보유 수량
        /// </summary>
        public decimal 보유수량 { get; set; }
        /// <summary>
        /// B19
        /// </summary>
        [ForeignKey("보유상태")]
        public string 보유상태코드 { get; set; }
        // ------
        public 품목정보 보유자재 { get; set; }
        public 품목정보 필요자재 { get; set; }
        public 공통코드 자재유형 { get; set; }
        public 보유품목정보 필요보유품목 { get; set; }
        public 공통코드 보유상태 { get; set; }
    }
}
