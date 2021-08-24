using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiSFS.Api.Shared.Models
{
    public class 메시지정보 : 공통정보
    {
        /// <summary>
        /// 메시지 아이디
        /// </summary>
        [Key]
        public int Id { get; set; }

        [MaxLength(4)]
        [ForeignKey("회사")]
        public string 회사코드 { get; set; }
        /// <summary>
        /// 전체메시지유무가 '예'일 경우 모든 사람에게 메시지가 전송된다.
        /// </summary>
        public bool 전체메시지유무 { get; set; }
        /// <summary>
        /// 메시지를 확인했을 경유 '예' 아닌 경우 '아니오'
        /// </summary>
        [MaxLength(10)]
        [ForeignKey("발송인")]
        public string 발송인사번 { get; set; }

        [MaxLength(10)]
        [ForeignKey("수신인")]
        public string 수신인사번 { get; set; }

        public bool 메시지확인유무 { get; set; }
        [MaxLength(256)]
        public string 메시지명 { get; set; }

        public string 메시지 { get; set; }
        /// <summary>
        /// B22 - [메시지] 메시지유형
        /// </summary>
        [MaxLength(10)]
        [ForeignKey("메시지")]
        public string 메시지유형코드 { get; set; }
        
        //////////
        
        public 직원정보 발송인 { get; set; }
        public 직원정보 수신인 { get; set; }
        public 공통코드 메시지유형 { get; set; }
        public 사업장 회사 { get; set; }
    }
}
