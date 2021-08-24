using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 공통코드 : 공통정보
    {
        [MaxLength(10)]
        [Key]
        public string 코드 { get; set; }
        [MaxLength(10)]

        public string 상위코드 { get; set; }
        [MaxLength(50)]
        public string 코드명 { get; set; }
        [MaxLength(50)]
        public string 코드영문명 { get; set; }
        [MaxLength(100)]
        public string 설명 { get; set; }
        [MaxLength(128)]
        public string 인자1 { get; set; }
        [MaxLength(128)]
        public string 인자2 { get; set; }
        [MaxLength(128)]
        public string 인자3 { get; set; }

        [MaxLength(10)]
        [DisplayName(nameof(코드유형))]
        public string 코드유형코드 { get; set; }

        public int 뎁스 { get; set; }
        public int 정렬순번 { get; set; }
        //------
        [JsonIgnore]
        public 공통코드 상위 { get; set; }
        [JsonIgnore]
        public IEnumerable<공통코드> 하위 { get; set; }
        [JsonIgnore]
        public 공통코드 코드유형 { get; set; }
    }
}
