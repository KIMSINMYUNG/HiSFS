using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Models.Parameters;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using WampSharp.V2.Rpc;


namespace HiSFS.Api.Shared.Services
{
    public interface I품질관리서비스
    {
        [WampProcedure("품질검사_조회")]
        Task<IEnumerable<품질검사정보>> 품질검사_조회(검색정보 검색 = null);
        [WampProcedure("품질검사_저장")]
        Task 품질검사_저장(품질검사정보 info, bool isAdded);
        [WampProcedure("품질검사_삭제")]
        Task 품질검사_삭제(품질검사정보 info, bool isCompletely);
        [WampProcedure("수입검사_조회")]
        Task<IEnumerable<보유품목불량정보>> 수입검사_조회();
        [WampProcedure("제품검사_조회")]
        Task<IEnumerable<보유품목검사정보>> 제품검사_조회();

        // 추가 2021.02.01  (품질검사측정정보_저장, 품질검사측정정보_조회, 품질검사측정정보유무_조회)
        [WampProcedure("품질검사측정정보_저장")]
        Task<bool> 품질검사측정정보_저장(품질검사측정정보 info, bool isAdded);

        // 수정 2021.03.08 생산지시코드 파라메터 추가
        [WampProcedure("품질검사측정정보_조회")]
        Task<IEnumerable<품질검사측정정보>> 품질검사측정정보_조회(string 생산지시코드, int seq);

        [WampProcedure("품질검사측정정보유무_조회")]
        Task<품질검사측정정보> 품질검사측정정보유무_조회(int seq, 품질검사측정정보 품질검사측정);

        // 수정 2021.03.02  파라메터 수정 추가 2021.02.17  (품질검사측정_보유품목코드_저장, 품질검사측정정보목록_조회)
        [WampProcedure("품질검사측정_보유품목코드_저장")]
        Task 품질검사측정_보유품목코드_저장(string 생산품코드, string 품목구분코드, int seq);
        // 수정 2021.03.02  파라메터 수정 추가 2021.02.17  (품질검사측정_보유품목코드_저장, 품질검사측정정보목록_조회)


        [WampProcedure("품질검사측정정보목록_조회")]
        Task<IEnumerable<품질검사측정정보>> 품질검사측정완료유무_조회(string 생산지시코드, string 회사코드);

        [WampProcedure("품질검사측정_생산지시측정수량_저장")]
        Task 품질검사측정_생산지시측정수량_저장(string 생산지시코드, string 합격여부, int 총품질검사수량);


        // 수정 2021.03.02  품질검사측정_보유품목일련정보_저장
        [WampProcedure("품질검사측정_보유품목일련정보_저장")]
        Task 품질검사측정_보유품목일련정보_저장(string 생산품코드, int seq);

        // 수정 2021.03.03  품질검사측정_보유품목일련정보_저장
        [WampProcedure("품질검사측정_보유품목일련정보_조회")]
        Task<List<보유품목일련정보>> 품질검사측정_보유품목일련정보_조회(string 품목코드);

        [WampProcedure("품질검사측정_보유품목일련정보_상세")]
        Task<List<보유품목일련정보>> 품질검사측정_보유품목일련정보_상세(string 품목코드, string 보유년월일);


        // 수정 2021.03.05  품질검사측정_보유품목일련번호생성_저장
        [WampProcedure("품질검사측정_보유품목일련번호생성_저장")]
        Task 품질검사측정_보유품목일련번호생성_저장(string 생산지시코드, string 생산품코드, int count);

        [WampProcedure("수입검사_저장")]
        Task 수입검사_저장(보유품목불량정보 info, bool isAdded);
        [WampProcedure("수입검사_삭제")]
        Task 수입검사_삭제(보유품목불량정보 info, bool isCompletely);

        [WampProcedure("보유품목일지_저장")]
        Task 보유품목일지_저장(string 생산품코드, int 수량, string 생산지시코드);

        [WampProcedure("품질검사시작_보유품목코드_저장")]
        Task 품질검사시작_보유품목코드_저장(string 생산품코드, string 품목구분코드, decimal 수량, string 회사코드);









        //20210531 외주품질검사측정

        [WampProcedure("외주품질검사측정정보_저장")]
        Task<bool> 외주품질검사측정정보_저장(외주품질검사측정정보 info, bool isAdded);

        [WampProcedure("외주품질검사측정정보유무_조회")]
        public Task<외주품질검사측정정보> 외주품질검사측정정보유무_조회(int seq, 외주품질검사측정정보 품질검사측정);

        [WampProcedure("외주품질검사측정정보_조회")]
        Task<IEnumerable<외주품질검사측정정보>> 외주품질검사측정정보_조회(string 지시번호, int seq);

        [WampProcedure("외주품질검사측정완료유무_조회")]
        Task<IEnumerable<외주품질검사측정정보>> 외주품질검사측정완료유무_조회(string 지시번호, string 회사코드);
        
        [WampProcedure("외주품질검사측정_외주지시측정수량_저장")]
        Task 외주품질검사측정_외주지시측정수량_저장(string 지시번호, string 합격여부, string 회사코드);


        [WampProcedure("수입검사품질검사측정정보유무_조회")]
        Task<발주서별품질검사측정정보> 수입검사품질검사측정정보유무_조회(int seq, 발주서별품질검사측정정보 품질검사측정);

        [WampProcedure("수입검사품질검사측정정보_조회")]
        Task<IEnumerable<발주서별품질검사측정정보>> 수입검사품질검사측정정보_조회(string 발주번호, decimal 발주순번, int seq);

        [WampProcedure("수입검사품질검사측정정보_저장")]
        Task<bool> 수입검사품질검사측정정보_저장(발주서별품질검사측정정보 info, bool isAdded);

        [WampProcedure("수입검사품질검사측정완료유무_조회")]
        Task<IEnumerable<발주서별품질검사측정정보>> 수입검사품질검사측정완료유무_조회(string 발주번호,decimal 발주순번,  string 회사코드);

        [WampProcedure("수입검사품질검사측정_측정수량_저장")]
        Task 수입검사품질검사측정_측정수량_저장(string 발주번호,decimal 발주순번,  string 합격여부, string 회사코드);

        [WampProcedure("발주서별품질검사측정완료유무_조회")]
        Task<IEnumerable<발주서별품질검사측정정보>> 발주서별품질검사측정완료유무_조회(string 발주번호, decimal 발주순번,  string 회사코드);



    }
}
