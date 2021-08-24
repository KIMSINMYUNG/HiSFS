using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 파일정보 : 공통정보
    {
        [Key]
        /// <summary>
        /// 1 ~ N
        /// </summary>
        //[Key]
        public int 순번 { get; set; }
        /// <summary>
        /// 파일 이름
        /// </summary>
        [MaxLength(512)]
        public string 파일이름 { get; set; }
        /// <summary>
        /// 파일 설명
        /// </summary>
        [MaxLength(512)]
        public string 설명 { get; set; }
        /// <summary>
        /// 파일 확장자
        /// </summary>
        [MaxLength(12)]
        public string 확장자 { get; set; }
        /// <summary>
        /// 파일 크기
        /// </summary>
        public int 크기 { get; set; }
        /// <summary>
        /// 파일 경로
        /// </summary>
        [MaxLength(512)]
        public string 경로 { get; set; }
        /// <summary>
        /// 파일 임시 경로
        /// </summary>
        [MaxLength(512)]
        public string 임시경로 { get; set; }
        /// <summary>
        /// 사진의 경우 썸네일 경로
        /// </summary>
        [MaxLength(512)]
        public string 썸네일경로 { get; set; }

        [ForeignKey("폴더")]
        public int 폴더순번 { get; set; }
        
        // ----------

        public 파일폴더정보 폴더 { get; set; }
    }
}
