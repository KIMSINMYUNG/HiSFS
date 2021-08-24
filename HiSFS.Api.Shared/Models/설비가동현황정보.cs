using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 설비가동현황정보 : 공통정보
    {
        [MaxLength(30)]
        [Key]
        public string 코드 { get; set; }

        [MaxLength(4)]
        [Key,ForeignKey("회사")]
        public string 회사코드 { get; set; }

        [MaxLength(32)]
        public string 상태 { get; set; }
        public string 상태유형코드 { get; set; }
        [MaxLength(32)]
        public string 이전상태 { get; set; }

        [MaxLength(30)]
        [ForeignKey("설비")]
        public string 설비코드 { get; set; }
        
        public DateTime? 상태변경시각 { get; set; }
        
        public long 상태유지시간Ticks { get; set; }
        [NotMapped]
        public TimeSpan? 상태유지시간
        {
            get => TimeSpan.FromTicks(상태유지시간Ticks);
            set => 상태유지시간Ticks = value?.Ticks ?? 0;
        }

        // ----------------------------
        public 사업장 회사 { get; set; }
        public 보유품목정보 설비 { get; set; }
        public 공통코드 상태유형 { get; set; }
    }
}
