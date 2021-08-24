using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    /// <summary>
    /// TODO: 상세한 속성은 품질검사 설계 완료 후 추가 될 예정임
    /// </summary>
    public class 품질검사정보 : 공통정보
    {
        [MaxLength(10)]
        [Key]
        public string 품질검사코드 { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "품질검사명은 필수입니다.")]
        public string 품질검사명 { get; set; }
    }
}
