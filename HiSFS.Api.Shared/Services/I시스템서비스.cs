using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Models.Parameters;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace HiSFS.Api.Shared.Services
{
    public interface I시스템서비스
    {
        [WampProcedure("메뉴_조회")]
        Task<IEnumerable<메뉴정보>> 메뉴_조회();
        [WampProcedure("메뉴_저장")]
        Task 메뉴_저장(메뉴정보 info, bool isAdd);
        [WampProcedure("메뉴_삭제")]
        Task 메뉴_삭제(메뉴정보 info, bool isCompletely);

        [WampProcedure("연동장비_조회")]
        Task<IEnumerable<연동장비정보>> 연동장비_조회(검색정보 검색 = null);
        [WampProcedure("연동장비_승인")]
        Task 연동장비_승인(연동장비정보 info, bool isApproval);
        [WampProcedure("액션로그_조회")]
        Task<IEnumerable<액션로그>> 액션로그_조회();
        [WampProcedure("메뉴부서권한_조회")]
        Task<IEnumerable<메뉴정보>> 메뉴부서권한_조회(string 부서코드);
        [WampProcedure("메뉴부서권한_저장")]
        Task 메뉴부서권한_저장(IEnumerable<메뉴부서권한정보> list);
        [WampProcedure("메뉴직원권한_조회")]
        Task<IEnumerable<메뉴정보>> 메뉴직원권한_조회(string 직원사번);
        [WampProcedure("메뉴직원권한_저장")]
        Task 메뉴직원권한_저장(IEnumerable<메뉴직원권한정보> list);
        [WampProcedure("메뉴유형별권한_조회")]
        Task<IEnumerable<메뉴정보>> 메뉴유형별권한_조회(string 권한유형코드);
        [WampProcedure("메뉴유형별권한_저장")]
        Task 메뉴유형별권한_저장(IEnumerable<메뉴유형별권한정보> list);
        [WampProcedure("공통코드_조회")]
        Task<IEnumerable<공통코드>> 공통코드_조회(string 상위코드);
        
    }
}
