using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 공정단위정보 : 공통정보
    {

		[MaxLength(4)]
		[ForeignKey("회사")]
        //[Key]
		public string 회사코드 { get; set; }

		[MaxLength(20)]
        [Key]
        public string 공정단위코드 { get; set; }

        [MaxLength(20)]
        public string 원공정단위코드 { get; set; }
        public int 관리차수 { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "공정단위명은 필수 입니다.")]
        public string 공정단위명 { get; set; }

        [MaxLength(30)]
        //[Required(ErrorMessage = "공정품은 필수 입니다.")]
        [ForeignKey("공정품")]
        public string 공정품코드 { get; set; }

        [MaxLength(10)]
        [Required(ErrorMessage = "공정품유형은 필수 입니다.")]
        [ForeignKey("공정품유형")]
        public string 공정품유형코드 { get; set; }

        [MaxLength(20)]
        //[Required(ErrorMessage = "도면은 필수 입니다.")]
        [ForeignKey("도면")]
        public string? 도면코드 { get; set; }

        [MaxLength(10)]
        [Required(ErrorMessage = "공정은 필수 입니다.")]
        [ForeignKey("공정")]
        public string 공정코드 { get; set; }

        [MaxLength(30)]
        [ForeignKey("완제품")]
        public string 완제품코드 { get; set; }
        [Required(ErrorMessage = "공정예상시간은 필수 입니다.")]
        public double? 공정예상시간 { get; set; }

        [MaxLength(300)]
        public string 비고 { get; set; }

        [MaxLength(30)]

        [ForeignKey("BOM품목정보상세")]
        public string 품목코드 { get; set; }

        //------
        // 공정품
        public 품목정보 공정품 { get; set; }
        public 품목정보 완제품 { get; set; }
        public 공통코드 공정품유형 { get; set; }
        public 도면정보 도면 { get; set; }
        public 공정정보 공정 { get; set; }

        public 사업장 회사 { get; set; }
        public IList<공정단위자재정보> 공정자재목록 { get; set; }

        public IList<공정단위설비정보> 공정설비목록 { get; set; }

        public IList<공정단위검사정보> 공정검사목록 { get; set; }

        public IList<BOM품목정보상세> S_BOM품목정보상세 { get; set; }

       

        public IList<생산품공정차수정보> 생산품공정차수목록 { get; set; }

      
      
    }
}
