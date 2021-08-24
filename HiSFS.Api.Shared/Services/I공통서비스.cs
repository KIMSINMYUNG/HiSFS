using HiSFS.Api.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace HiSFS.Api.Shared.Services
{
    public interface I공통서비스
    {
        [WampProcedure("메뉴_조회")]
        Task<IEnumerable<메뉴정보>> 메뉴_조회();

        [WampProcedure("공통코드_조회")]
        Task<IEnumerable<공통코드>> 공통코드_조회();
        [WampProcedure("공통코드_저장")]
        Task 공통코드_저장(공통코드 code, bool isAdd);
        [WampProcedure("공통코드_삭제")]
        Task 공통코드_삭제(공통코드 code, bool isCompletely);
        [WampProcedure("직원_인증")]
        Task<(직원정보, string)> 직원_인증(string userId, string password, string 회사코드);
        [WampProcedure("직원상세_조회")]
        Task<직원정보> 직원상세_조회(string userId);
        [WampProcedure("메시지_수신조회")]
        Task<IEnumerable<메시지정보>> 메시지_수신조회(string 사번);
        [WampProcedure("메시지_송신조회")]
        Task<IEnumerable<메시지정보>> 메시지_송신조회(string 사번);
        [WampProcedure("메시지_송신")]
        Task 메시지_송신(메시지정보 메시지);
        [WampProcedure("메시지_삭제")]
        Task 메시지_삭제(메시지정보 메시지);
        [WampProcedure("메시지_확인")]
        Task 메시지_확인(메시지정보 메시지);
        [WampProcedure("메시지_읽지않은개수")]
        Task<int> 메시지_읽지않은개수(string 사번);
        [WampProcedure("메시지_전체확인")]
        Task 메시지_전체확인(string 사번);
        [WampProcedure("PDA_메시지")]
        Task PDA_메시지(string 사번);
        [WampProcedure("QR_메시지")]
        Task QR_메시지(int cnt, string message_qr, string message_txt);
        [WampProcedure("PrtUri")]
        Task<string> PrtUri();
        [WampProcedure("Server_Uri")]
        Task<string> Server_Uri();
        [WampProcedure("포인트_저장")]
        Task 포인트_저장(string userid, string 품목코드, int 수량);
        [WampProcedure("포인트_조회")]
        Task<int> 포인트_조회(string userid, string 품목코드, string 생산지시코드, string 회사코드);
        [WampProcedure("포인트_삭제")]
        Task<bool> 포인트_삭제(string userid, string 품목코드);
        [WampProcedure("포인트_설정")]
        Task<bool> 포인트_설정(string userid, string 품목코드, int row_select, int cnt);

        //2021.03.02 추가
        [WampProcedure("포인트_저장2")]
        Task 포인트_저장2(string userid, string 품목코드, int 수량, int 검사항목수);

        //추가 2021.03.17

        [WampProcedure("PDA_공정메시지")]
        Task PDA_공정메시지(string 사번);

        [WampProcedure("공통코드명_조회")]
        Task<string> 공통코드명_조회(string 코드명);

        [WampProcedure("PDA_작업지시서메시지")]
        Task PDA_작업지시서메시지(string 사번, List<string> 담당자들);


        [WampProcedure("외주포인트_삭제")]
        Task<bool> 외주포인트_삭제(string userid, string 품목코드);

        [WampProcedure("외주포인트_설정")]
        Task<bool> 외주포인트_설정(string userid, string 품목코드, int row_select, int cnt);

        [WampProcedure("외주포인트_조회")]
        Task<int> 외주포인트_조회(string userid, string 품목코드, string 지시번호, string 회사코드);

        [WampProcedure("외주포인트_저장2")]
        Task 외주포인트_저장2(string userid, string 품목코드, int 수량, int 검사항목수);

        [WampProcedure("외주포인트_저장")]
        Task 외주포인트_저장(string userid, string 품목코드, int 수량);


        [WampProcedure("메시지ToPDA")]
        Task 메시지ToPDA(string 사유, string 사번);


        [WampProcedure("수입검사포인트_저장")]
        Task 수입검사포인트_저장(string userid, string 품목코드, int 수량);

        [WampProcedure("수입검사포인트_저장2")]
        Task 수입검사포인트_저장2(string userid, string 품목코드, int 수량, int 검사항목수);

        [WampProcedure("수입검사포인트_조회")]
        Task<int> 수입검사포인트_조회(string userid, string 품목코드, string 발주번호,decimal 발주순번, string 회사코드);

        [WampProcedure("수입검사포인트_설정")]
        Task<bool> 수입검사포인트_설정(string userid, string 품목코드, int row_select, int cnt);

        [WampProcedure("수입검사포인트_삭제")]
        Task<bool> 수입검사포인트_삭제(string userid, string 품목코드);
    }
}
