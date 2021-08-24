using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Models.Parameters;
using HiSFS.Api.Shared.Models.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace HiSFS.Api.Shared.Services
{
    public interface I생산관리서비스
    {
        [WampProcedure("공정단위_조회")]
        Task<IEnumerable<공정단위정보>> 공정단위_조회(string 회사코드);
        [WampProcedure("공정단위_저장")]
        Task 공정단위_저장(공정단위정보 info, bool isAdd);
        [WampProcedure("공정단위_삭제")]
        Task 공정단위_삭제(공정단위정보 info, bool isCompletely);
        [WampProcedure("생산품공정_조회")]
        Task<IEnumerable<생산품공정정보>> 생산품공정_조회(string 회사코드, 검색정보 검색 = null);
        [WampProcedure("생산품공정_저장")]
        Task 생산품공정_저장(생산품공정정보 info, bool isAdd);
        [WampProcedure("생산품공정_삭제")]
        Task 생산품공정_삭제(생산품공정정보 info, bool isCompletely);
        [WampProcedure("생산품공정차수_조회")]
        Task<IEnumerable<생산품공정차수정보>> 생산품공정차수_조회(string 생산품공정코드);
        [WampProcedure("생산품공정차수_저장")]
        Task 생산품공정차수_저장(생산품공정차수정보 info, bool isAdd);
        [WampProcedure("생산품공정차수_삭제")]
        Task 생산품공정차수_삭제(생산품공정차수정보 info, bool isCompletely);
        [WampProcedure("설비현황_조회")]
        Task<IEnumerable<보유품목정보>> 설비현황_조회(string 회사코드,  bool isOnlyUse = false);

        [WampProcedure("보유설비현황_조회")]
        Task<IEnumerable<보유품목정보>> 보유설비현황_조회(string 회사코드, bool isOnlyUse, string 품목코드 );

        [WampProcedure("공정단위_설비목록")]
        Task<IEnumerable<보유품목정보>> 공정단위_설비목록();
        [WampProcedure("공정단위자재_저장")]
        Task 공정단위자재_저장(공정단위자재정보 info, bool isAdd);
        [WampProcedure("공정단위자재_삭제")]
        Task 공정단위자재_삭제(공정단위자재정보 info, bool isCompletely);
        [WampProcedure("공정단위설비_저장")]
        Task 공정단위설비_저장(공정단위설비정보 info, bool isAdd);
        [WampProcedure("공정단위설비_삭제")]
        Task 공정단위설비_삭제(공정단위설비정보 info, bool isCompletely);
        [WampProcedure("생산계획_조회")]
        Task<IEnumerable<생산계획정보>> 생산계획_조회(string 회사코드 , 검색정보 검색 = null);
        [WampProcedure("생산계획_저장")]
        Task 생산계획_저장(생산계획정보 info, bool isAdd);
        [WampProcedure("생산계획_삭제")]
        Task 생산계획_삭제(생산계획정보 info, bool isCompletely);
        [WampProcedure("생산계획상세_저장")]
        Task 생산계획상세_저장(생산계획정보 info);
        [WampProcedure("생산계획상세_조회")]
        Task<생산계획정보> 생산계획상세_조회(string 생산계획코드, string 회사코드);
        [WampProcedure("작업지시_조회")]
        Task<IEnumerable<생산지시정보>> 작업지시_조회(string 회사코드, 검색정보 검색 = null);
        [WampProcedure("작업지시_저장")]
        Task 작업지시_저장(생산지시정보 info, bool isAdd);
        [WampProcedure("작업지시_삭제")]
        Task 작업지시_삭제(생산지시정보 info, bool isCompletely);
        [WampProcedure("작업지시상세_조회")]
        Task<생산지시정보> 작업지시상세_조회(string 작업지시코드, string 회사코드);
        [WampProcedure("작업지시상세_저장")]
        Task 작업지시상세_저장(생산지시정보 info);
        // ----------------------------
        [WampProcedure("생산계획생산자재소요현황_조회")]
        Task<IEnumerable<생산계획자재보유현황>> 생산계획생산자재소요현황_조회(string 생산계획코드, string 회사코드);
        [WampProcedure("생산계획생산지시현황_조회")]
        Task<IEnumerable<생산계획작업지시현황>> 생산계획생산지시현황_조회(string 생산계획코드, string 회사코드);
        

        //수정 파라메터 추가 2020.02.01
        [WampProcedure("공정단위검사_조회")]
        Task<IEnumerable<공정단위검사정보>> 공정단위검사_조회(string 공정단위코드 = null);
        [WampProcedure("공정단위검사_저장")]
        Task 공정단위검사_저장(공정단위검사정보 info, bool isAdd);
        [WampProcedure("공정단위검사_삭제")]
        Task 공정단위검사_삭제(공정단위검사정보 info, bool isCompletely);
        [WampProcedure("공정단위검사장비_저장")]
        Task 공정단위검사장비_저장(공정단위검사장비 info, bool isAdd);
        [WampProcedure("공정단위검사장비_삭제")]
        Task 공정단위검사장비_삭제(공정단위검사장비 info, bool isCompletely);
        //[WampProcedure("작업지시_완제품_발행")]
        //Task<string> 작업지시_완제품_발행(string 작업지시코드, int 공정차수, 품목정보 품목);
        //[WampProcedure("작업지시_완제품_조회")]
        //Task<보유품목정보> 작업지시_완제품_조회(string 작업지시코드, int 공정차수);

        // 2021 02 08  작업지시품질검사 엑셀작업
        [WampProcedure("작업지시품질검사_조회")]
        Task<IEnumerable<생산지시정보>> 작업지시품질검사_조회(string 회사코드);

        [WampProcedure("작업지시품질검사현황_조회")]
        Task<IEnumerable<작업지시공정현황>> 작업지시품질검사현황_조회(string 생산지시코드);


        [WampProcedure("작업지시품질검사항목_조회")]
        Task<IEnumerable<품질검사측정정보>> 작업지시품질검사항목_조회(string 공정단위코드);
        [WampProcedure("작업지시품질검사품목_조회")]
        Task<List<품질검사측정정보>> 작업지시품질검사품목_조회(string 공정단위코드);


        // 추가 2021 03 17
        [WampProcedure("공정이력정보_저장")]
        Task 공정이력정보_저장(생산지시정보 info, 생산지시공정차수정보 info2, int 생산수량, string 공정상태, bool isAdd);

        // 추가 2021 03 22

        [WampProcedure("작업지시공정이력현황_조회")]
        Task<IEnumerable<공정이력정보>> 작업지시공정이력현황_조회(string 회사코드);


        [WampProcedure("작업지시공정이력상세_조회")]
        Task<IEnumerable<공정이력상세정보>> 작업지시공정이력상세_조회(int 공정이력인덱스);

        [WampProcedure("작업지시공정완료이력현황_조회")]
        Task<공정이력정보> 작업지시공정완료이력현황_조회(string 회사코드, string 생산지시코드, string 공정단위코드);


        // 추가 2021 0508
        [WampProcedure("작업생산실적_저장")]
        Task<bool> 작업생산실적_저장(생산실적헤더정보 헤더, 생산실적상세정보 상세, List<공정단위자재현황> listBom);


        [WampProcedure("재고이동작업지시_조회")]
        Task<IEnumerable<생산지시정보>> 재고이동작업지시_조회(string 회사코드);


        [WampProcedure("공정단위자재BOM정보_저장")]
        Task 공정단위자재BOM정보_저장(List<공정단위자재정보> info, bool isAdd);

        [WampProcedure("BOM정보를공정단위자재_저장")]
        Task BOM정보를공정단위자재_저장(List<공정단위자재정보> info, IList<공정단위자재정보> remove);


        [WampProcedure("작업지시공정별실적현황_조회")]
        Task<List<생산실적헤더정보>> 작업지시공정별실적현황_조회(string 회사코드, 작업지시공정현황 작업지시공정);

        [WampProcedure("작업지시기준_조회")]
        Task<IEnumerable<생산지시정보>> 작업지시기준_조회(string 회사코드);


        [WampProcedure("작업지시공정현황_조회")]
        Task<IEnumerable<작업지시공정현황>> 작업지시공정현황_조회(string 회사코드, string 생산지시코드);



        [WampProcedure("작업지시공정별품질검사현황_조회")]
        Task<IEnumerable<작업지시공정현황>> 작업지시공정별품질검사현황_조회(string 회사코드, 작업지시공정현황 작업지시공정);

        //2021.05.18
        [WampProcedure("작업지시Action_조회")]
        Task<IEnumerable<생산지시정보>> 작업지시Action_조회(string 회사코드, 검색정보 검색);



        // 대쉬보드
        [WampProcedure("작업지시별실적현황_조회")]
        Task<List<작업지시공정현황>> 작업지시별실적현황_조회(string 회사코드);


        [WampProcedure("작업지시별생산실적현황_조회")]
        Task<List<작업지시생산실적현황>> 작업지시별생산실적현황_조회(string 회사코드);

        [WampProcedure("공정이력완료정보_저장")]
        Task<bool> 공정이력완료정보_저장(공정이력정보 공정이력정보);

        [WampProcedure("NEW공정단위_저장")]
        Task NEW공정단위_저장(공정단위정보 info, bool isAdd);

        [WampProcedure("생산제품_위치등록")]
        Task<bool> 생산제품_위치등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom);

        [WampProcedure("생산제품입고처리_등록")]
        Task<bool> 생산제품입고처리_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom);


        [WampProcedure("더존_생산실적헤더정보_등록")]
        Task<bool> 더존_생산실적헤더정보_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom);

        [WampProcedure("더존_불량실적헤더정보_등록")]
        Task<bool> 더존_불량실적헤더정보_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom);





        [WampProcedure("더존멀티_생산실적헤더정보_등록")]
        Task<bool> 더존멀티_생산실적헤더정보_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom);
        [WampProcedure("더존멀티_불량실적헤더정보_등록")]
        Task<bool> 더존멀티_불량실적헤더정보_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom);





        //20210530
        [WampProcedure("공정단위검사장비_조회")]
        Task<IEnumerable<공정단위검사장비>> 공정단위검사장비_조회(string 공정단위코드, string 회사코드);


        [WampProcedure("외주지시별품질검사장비_조회")]
        Task<IEnumerable<외주지시별품질검사장비>> 외주지시별품질검사장비_조회(string 지시번호, string 회사코드);

        [WampProcedure("외주지시별품질검사장비_저장")]
        Task 외주지시별품질검사장비_저장(외주지시별품질검사장비 info, bool isAdd);

        [WampProcedure("외주지시별품질검사장비_삭제")]
        Task 외주지시별품질검사장비_삭제(외주지시별품질검사장비 info, bool isCompletely);


        [WampProcedure("외주지시별검사정보_조회")]
        Task<IEnumerable<외주지시별검사정보>> 외주지시별검사정보_조회(string 지시번호, string 회사코드);

        [WampProcedure("외주지시별검사정보_저장")]
        Task 외주지시별검사정보_저장(외주지시별검사정보 info, bool isAdd);

        [WampProcedure("외주지시별검사정보_삭제")]
        Task 외주지시별검사정보_삭제(외주지시별검사정보 info, bool isCompletely);


        //수입검사

        [WampProcedure("발주서별품질검사장비_조회")]
        Task<IEnumerable<발주서별품질검사장비>> 발주서별품질검사장비_조회(string 발주번호, decimal 발주순번, string 회사코드);

        [WampProcedure("발주서별별검사정보_조회")]
        Task<IEnumerable<발주서별품질검사정보>> 발주서별별검사정보_조회(string 발주번호, decimal 발주순번, string 회사코드);

        [WampProcedure("발주서별품질검사장비_삭제")]
        Task 발주서별품질검사장비_삭제(발주서별품질검사장비 info, bool isCompletely);

        [WampProcedure("발주서별품질검사장비_저장")]
        Task 발주서별품질검사장비_저장(발주서별품질검사장비 info, bool isAdd);

        [WampProcedure("발주서별품질검사정보_저장")]
        Task 발주서별품질검사정보_저장(발주서별품질검사정보 info, bool isAdd);

        [WampProcedure("발주서별품질검사정보_삭제")]
        Task 발주서별품질검사정보_삭제(발주서별품질검사정보 info, bool isCompletely);







        [WampProcedure("수입검사품질검사현황_조회")]
        Task<IEnumerable<발주서별수입검사>> 수입검사품질검사현황_조회(string 회사코드);


        [WampProcedure("발주서별품질검사항목_조회")]
        Task<List<발주서별품질검사측정정보>> 발주서별품질검사항목_조회(발주서별수입검사 info);

        [WampProcedure("발주서별품질검사_조회")]
        Task<IEnumerable<발주서별품질검사정보>> 발주서별품질검사_조회(발주서별수입검사 info);


        [WampProcedure("발주서별품질검사측정정보_조회")]
        Task<IEnumerable<발주서별품질검사측정정보>> 발주서별품질검사측정정보_조회(발주서별품질검사측정정보 info, int seq);




        [WampProcedure("외주작업지시서품검정보현황_조회")]
        Task<IEnumerable<외주작업지시서품검정보>> 외주작업지시서품검정보현황_조회(string 회사코드);

        [WampProcedure("외주작업지시서품검항목_조회")]
        Task<List<외주품질검사측정정보>> 외주작업지시서품검항목_조회(외주작업지시서품검정보 info);


        [WampProcedure("외주품질검사_조회")]
        Task<IEnumerable<외주지시별검사정보>> 외주품질검사_조회(외주작업지시서품검정보 info);

        [WampProcedure("외주품질검사측정정보_조회")]
        Task<IEnumerable<외주품질검사측정정보>> 외주품질검사측정정보_조회(외주품질검사측정정보 info, int seq);


        [WampProcedure("공정단위실적처리_저장")]
        Task<bool> 공정단위실적처리_저장(생산실적헤더정보 헤더, 생산실적상세정보 상세, List<공정단위자재현황> listBom);



        [WampProcedure("품질검사_작업생산실적_저장")]
        Task<bool> 품질검사_작업생산실적_저장(생산실적헤더정보 헤더, 생산실적상세정보 상세, List<공정단위자재현황> listBom);
        [WampProcedure("품질검사_생산제품입고처리_등록")]
        Task<bool> 품질검사_생산제품입고처리_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom);
        [WampProcedure("품질검사_생산제품_위치등록")]
        Task<bool> 품질검사_생산제품_위치등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom);
        [WampProcedure("품질검사_더존_생산실적헤더정보_등록")]
        Task<bool> 품질검사_더존_생산실적헤더정보_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom);
        [WampProcedure("품질검사_더존_불량실적헤더정보_등록")]
        Task<bool> 품질검사_더존_불량실적헤더정보_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom);



        [WampProcedure("완료보고용생산실적현황_조회")]
        Task<List<작업지시생산실적현황>> 완료보고용생산실적현황_조회(string 회사코드);
    }

}
