using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Models.Parameters;
using HiSFS.Api.Shared.Models.View;
using HiSFS.Api.Shared.Models.View_DZICUBE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace HiSFS.Api.Shared.Services
{
    public interface I기준정보서비스
    {
        [WampProcedure("직원_조회")]
        Task<IEnumerable<직원정보>> 직원_조회(bool isOnlyUse = false, string 회사코드 = null);
        [WampProcedure("직원_저장")]
        Task 직원_저장(직원정보 info, bool isAdd);
        [WampProcedure("직원_삭제")]
        Task 직원_삭제(직원정보 info, bool isCompletely);
        [WampProcedure("부서_조회")]
        Task<IEnumerable<부서정보>> 부서_조회(검색정보 검색 = null);
        [WampProcedure("부서_저장")]
        Task 부서_저장(부서정보 info, bool isAdd);
        [WampProcedure("부서_삭제")]
        Task 부서_삭제(부서정보 info, bool isCompletely);
        [WampProcedure("거래처_조회")]
        Task<IEnumerable<거래처정보>> 거래처_조회(string 회사코드 = null);
        [WampProcedure("거래처_삭제")]
        Task 거래처_삭제(거래처정보 info, bool isCompletely);
        [WampProcedure("거래처_저장")]
        Task 거래처_저장(거래처정보 info, bool isAdd);

        [WampProcedure("생산품_조회")]
        Task<IEnumerable<품목정보>> 생산품_조회(bool isOnlyUse = false, bool 생산품만 = false);
        [WampProcedure("생산품_저장")]
        Task 생산품_저장(품목정보 info, bool isAdd);
        [WampProcedure("생산품_삭제")]
        Task 생산품_삭제(품목정보 info, bool isCompletely);

        [WampProcedure("품목_조회")]
        Task<IEnumerable<품목정보>> 품목_조회();
        [WampProcedure("품목_저장")]
        Task 품목_저장(품목정보 info, bool isAdd);
        [WampProcedure("품목_삭제")]
        Task 품목_삭제(품목정보 info, bool isCompletely);

        [WampProcedure("설비_조회")]
        Task<IEnumerable<품목정보>> 설비_조회();
        [WampProcedure("설비_저장")]
        Task 설비_저장(품목정보 info, bool isAdd);
        [WampProcedure("설비_삭제")]
        Task 설비_삭제(품목정보 info, bool isCompletely);
        [WampProcedure("보유설비_저장")]
        Task 보유설비_저장(보유품목정보 info, bool isAdd);
        [WampProcedure("보유설비_삭제")]
        Task 보유설비_삭제(보유품목정보 info, bool isCompletely);


        [WampProcedure("원자재_조회")]
        Task<IEnumerable<품목정보>> 원자재_조회(bool isOnlyUse = false, bool 반제품포함유무 = false);
        [WampProcedure("원자재_저장")]
        Task 원자재_저장(품목정보 info, bool isAdd);
        [WampProcedure("원자재_삭제")]
        Task 원자재_삭제(품목정보 info, bool isCompletely);


        [WampProcedure("장소_조회")]
        Task<IEnumerable<장소정보>> 장소_조회(string 회사코드 = null);
        [WampProcedure("장소_저장")]
        Task 장소_저장(장소정보 info, bool isAdd);
        [WampProcedure("장소_삭제")]
        Task 장소_삭제(장소정보 info, bool isCompletely);
        [WampProcedure("장소위치_조회")]
        Task<IEnumerable<장소위치정보>> 장소위치_조회(string 회사코드 = null);
        [WampProcedure("장소위치_삭제")]
        Task 장소위치_삭제(장소위치정보 info, bool isCompletely);
        [WampProcedure("장소위치_저장")]
        Task 장소위치_저장(장소위치정보 info, bool isAdd);
        [WampProcedure("도면_조회")]
        Task<IEnumerable<도면정보>> 도면_조회();
        [WampProcedure("도면_저장")]
        Task 도면_저장(도면정보 info, bool isAdd);
        [WampProcedure("도면_삭제")]
        Task 도면_삭제(도면정보 info, bool isCompletely);

        [WampProcedure("공정_조회")]
        Task<IEnumerable<공정정보>> 공정_조회(bool isOnlyUse = false);
        [WampProcedure("공정_삭제")]
        Task 공정_삭제(공정정보 info, bool isCompletely);
        [WampProcedure("공정_저장")]
        Task 공정_저장(공정정보 info, bool isAdd);
        [WampProcedure("자재부품설비_조회")]

        Task<IEnumerable<품목정보>> 자재부품설비_조회(bool isOnlyUse, bool 반제품포함유무, string 회사코드 = null);
        [WampProcedure("BOM_조회")]
        Task<IEnumerable<BOM정보>> BOM_조회();

        // 2021.03.10
        [WampProcedure("BOM품목정보_조회")]
        Task<IEnumerable<BOM품목정보현황>> BOM품목정보_조회();


        [WampProcedure("BOM정보상세_조회")]
        Task<IEnumerable<BOM품목정보상세>> BOM품목정보상세_조회(string 품목코드, string 공정단위코드);

        [WampProcedure("BOM품목정보상세_저장")]
        Task BOM품목정보상세_저장(BOM품목정보상세 info, bool isAdd);

        [WampProcedure("BOM품목정보상세공정단위_저장")]
        Task BOM품목정보상세공정단위_저장(string 공정단위코드, string 품목코드, bool isAdd);

        // [추가] 2021.03.25 
        [WampProcedure("BOM품목정보_저장")]
        Task BOM품목정보_저장(BOM품목정보현황 info, bool isAdd);

        [WampProcedure("BOM품목정보_삭제")]
        Task BOM품목정보_삭제(BOM품목정보현황 info);
        [WampProcedure("BOM품목정보상세_삭제")]
        Task BOM품목정보상세_삭제(BOM품목정보상세 info);

        //////// [추가] 2021.03.12

        [WampProcedure("발주정보_조회")]
        Task<IEnumerable<발주정보>> 발주정보_조회();
        [WampProcedure("발주정보_저장")]
        Task 발주정보_저장(발주정보 info, bool isAdd);
        [WampProcedure("발주정보상세_조회")]
        Task<IEnumerable<발주정보상세>> 발주정보상세_조회(int 발주순번);
        [WampProcedure("발주정보상세_저장")]
        Task 발주정보상세_저장(발주정보상세 info, bool isAdd);
        [WampProcedure("발주정보_삭제")]
        Task 발주정보_삭제(발주정보 info, bool isCompletely);

        [WampProcedure("발주정보상세_삭제")]
        Task 발주정보상세_삭제(발주정보상세 info, bool isCompletely);






        [WampProcedure("VL_MES_EMP")]
        Task<IEnumerable<VL_MES_EMP>> VL_MES_EMP();

        [WampProcedure("VL_MES_LOC")]
        Task<IEnumerable<VL_MES_LOC>> VL_MES_LOC();

        [WampProcedure("VL_MES_ITEM")]
        Task<IEnumerable<VL_MES_ITEM>> VL_MES_ITEM();

        //사업장VIEW
        [WampProcedure("VL_MES_DIV")]
        Task<IEnumerable<VL_MES_DIV>> VL_MES_DIV();

        [WampProcedure("VL_MES_DEPT")]
        Task<IEnumerable<VL_MES_DEPT>> VL_MES_DEPT();

        [WampProcedure("VL_MES_CUST")]
        Task<IEnumerable<VL_MES_CUST>> VL_MES_CUST();


        [WampProcedure("VL_MES_BOM")]
        Task<IEnumerable<VL_MES_BOM>> VL_MES_BOM();

        // 주문VIEW
        [WampProcedure("VL_MES_SO")]
        Task<IEnumerable<VL_MES_SO>> VL_MES_SO();


        // 발주VIEW
        [WampProcedure("VL_MES_PO")]
        Task<IEnumerable<VL_MES_PO>> VL_MES_PO();



        // 외주지시VIEW
        [WampProcedure("VL_MES_WO_WF")]
        Task<IEnumerable<VL_MES_WO_WF>> VL_MES_WO_WF();



        // 물류담당자 정보 VIEW
        [WampProcedure("VL_MES_PLN")]
        Task<IEnumerable<VL_MES_PLN>> VL_MES_PLN();


        // 재고조정현황 VIEW
        [WampProcedure("VL_MES_ADJUST")]
        Task<IEnumerable<VL_MES_ADJUST>> VL_MES_ADJUST();


        //////// [ERP반영] 2021.04.14 
        ///
        [WampProcedure("VL_MES_CUST_반영")]
        Task<bool> VL_MES_CUST_반영(List<VL_MES_CUST> 더존거래처);

        [WampProcedure("VL_MES_EMP_반영")]
        Task<bool> VL_MES_EMP_반영(List<VL_MES_EMP> 더존사원정보);

        [WampProcedure("VL_MES_DEPT_반영")]
        Task<bool> VL_MES_DEPT_반영(List<VL_MES_DEPT> 더존부서정보);

        [WampProcedure("VL_MES_ITEM_반영")]
        Task<bool> VL_MES_ITEM_반영(List<VL_MES_ITEM> 더존품목정보);

        [WampProcedure("VL_MES_LOC_반영")]
        Task<bool> VL_MES_LOC_반영(List<VL_MES_LOC> 더존창고정보);

        [WampProcedure("VL_MES_BOM_반영")]
        Task<bool> VL_MES_BOM_반영(List<VL_MES_BOM> 더존BOM정보);

        [WampProcedure("VL_MES_ADJUST_반영")]
        Task<bool> VL_MES_ADJUST_반영(List<VL_MES_ADJUST> 더존재고조정현황);

        [WampProcedure("VL_MES_SO_반영")]
        Task<bool> VL_MES_SO_반영(List<VL_MES_SO> 더존주문서정보);

        [WampProcedure("VL_MES_PO_반영")]
        Task<bool> VL_MES_PO_반영(List<VL_MES_PO> 더존발주서정보);

        [WampProcedure("VL_MES_PLN_반영")]
        Task<bool> VL_MES_PLN_반영(List<VL_MES_PLN> 더존물류담당자정보);

        [WampProcedure("VL_MES_WO_WF_반영")]
        Task<bool> VL_MES_WO_WF_반영(List<VL_MES_WO_WF> 더존외주지시확정정보);


        [WampProcedure("BOM_정보_조회")]
        Task<IEnumerable<BOM_정보>> BOM_정보_조회(string 회사코드 = null);

        [WampProcedure("부서정보_조회")]
        Task<IEnumerable<부서정보>> 부서정보_조회(string 회사코드 = null);


        [WampProcedure("물류담당자정보_조회")]
        Task<IEnumerable<물류담당자정보>> 물류담당자정보_조회(string 회사코드 = null);

        //[WampProcedure("발주서정보_조회")]
        //Task<IEnumerable<발주서정보>> 발주서정보_조회(string 회사코드 = null);

        //[WampProcedure("주문서정보_조회")]
        //Task<IEnumerable<주문서정보>> 주문서정보_조회(string 회사코드 = null);

        [WampProcedure("더존입고처리_BARPLUS_LSTOCK")]
        Task<bool> 더존입고처리_BARPLUS_LSTOCK(BARPLUS_LSTOCK 입고처리);

        [WampProcedure("MES입고처리_입고처리헤더정보_등록")]
        Task<bool> MES입고처리_입고처리헤더정보_등록(입고처리헤더정보 입고처리, BARPLUS_LSTOCK 입고처리2);

        //[WampProcedure("MES출고처리_출고처리헤더정보_등록")]
        //Task<bool> MES출고처리_출고처리헤더정보_등록(출고처리헤더정보 출고처리, bool isAdd);

        [WampProcedure("더존입고상세_BARPLUS_LSTOCK")]
        Task<bool> 더존입고상세_BARPLUS_LSTOCK_D(BARPLUS_LSTOCK_D 입고상세처리);

        [WampProcedure("MES입고처리_입고처리상세정보_등록")]
        Task<bool> MES입고처리_입고처리상세정보_등록(입고처리상세정보 입고상세처리);

        [WampProcedure("MES입고추가_조회")]
        Task<IEnumerable<입고처리헤더정보>> MES입고추가_조회(string 회사코드);

        [WampProcedure("MES입고상세_조회")]
        Task<IEnumerable<입고처리상세정보>> MES입고상세_조회(string 작업번호, string 회사코드);

        [WampProcedure("MES출고추가_조회")]
        Task<IEnumerable<출고처리헤더정보>> MES출고추가_조회(string 회사코드);

        [WampProcedure("MES출고상세_조회")]
        Task<IEnumerable<출고처리상세정보>> MES출고상세_조회(string 작업번호, string 회사코드);

        [WampProcedure("MES출고처리_출고처리헤더정보_등록")]
        Task<bool> MES출고처리_출고처리헤더정보_등록(출고처리헤더정보 출고처리, bool isAdd);

        [WampProcedure("MES출고처리_출고처리상세정보_등록")]
        Task<bool> MES출고처리_출고처리상세정보_등록(출고처리상세정보 출고처리, bool isAdd);


        // 20210426 추가
        [WampProcedure("발주서헤더정보_조회")]
        Task<IEnumerable<발주서헤더정보>> 발주서헤더정보_조회(string 회사코드);

        [WampProcedure("발주서정보_조회")]
        Task<IEnumerable<발주서정보>> 발주서정보_조회(string 발주번호, string 회사코드);

        [WampProcedure("주문서헤더정보_조회")]
        Task<IEnumerable<주문서헤더정보>> 주문서헤더정보_조회(string 회사코드);

        [WampProcedure("주문서정보_조회")]
        Task<IEnumerable<주문서정보>> 주문서정보_조회(string 주문번호, string 회사코드);



        [WampProcedure("보유품목_출고처리")]
        Task<bool> 보유품목_출고처리(string 보유품목코드, decimal 수량, string 장소코드, string 위치코드, string 사유);





        [WampProcedure("MES재고이동_재고이동헤더정보_등록")]
        Task<bool> MES재고이동_재고이동헤더정보_등록(재고이동헤더정보 이동, bool isAdd);

        [WampProcedure("MES재고이동_재고이동헤더정보_조회")]
        Task<List<재고이동헤더정보>> MES재고이동_재고이동헤더정보_조회(string 회사코드);

        [WampProcedure("MES재고이동상세_조회")]
        Task<List<재고이동상세정보>> MES재고이동상세_조회(string 작업번호, string 회사코드);

        [WampProcedure("MES재고이동_재고이동상세정보_등록")]
        Task<bool> MES재고이동_재고이동상세정보_등록(재고이동상세정보 이동처리, bool isAdd);



        [WampProcedure("발주서정보_순번조회")]
        Task<발주서정보> 발주서정보_순번조회(string 발주번호, string 회사코드, decimal 순번);




        [WampProcedure("MES생산관리_일괄생산실적헤더정보_조회")]
        Task<List<일괄생산실적헤더정보>> MES생산관리_일괄생산실적헤더정보_조회(string 회사코드);

        [WampProcedure("MES생산관리_일괄생산실적상세정보_조회")]
        Task<List<일괄생산실적상세정보>> MES생산관리_일괄생산실적상세정보_조회(string 작업번호, string 회사코드);

        [WampProcedure("MES재고이동_일괄생산실적헤더정보_등록")]
        Task<bool> MES재고이동_일괄생산실적헤더정보_등록(일괄생산실적헤더정보 생산실적, bool isAdd);

        [WampProcedure("MES재고이동_일괄생산실적상세정보_등록")]
        Task<bool> MES재고이동_일괄생산실적상세정보_등록(일괄생산실적상세정보 생산실적상세, bool isAdd);



        [WampProcedure("MES생산실적_작업외주생산실적등록정보_등록")]
        Task<bool> MES생산실적_작업외주생산실적등록정보_등록(작업외주생산실적등록정보 생산실적, bool isAdd);


        [WampProcedure("MES생산관리_작업외주생산실적등록정보_조회")]
        Task<List<작업외주생산실적등록정보>> MES생산관리_작업외주생산실적등록정보_조회(string 회사코드);


        [WampProcedure("MES생산관리_사용자재보고정보_조회")]
        Task<List<사용자재보고정보>> MES생산관리_사용자재보고정보_조회(string 회사코드);


        [WampProcedure("품목장소위치Popup_조회")]
        Task<IEnumerable<보유품목정보>> 품목장소위치Popup_조회(string 장소위치코드);

        //2021.04.30
        [WampProcedure("VL_MES_PO_View")]
        Task<IEnumerable<발주서헤더정보>> VL_MES_PO_View(string 회사코드);
        [WampProcedure("발주서정보_조회Dz")]
        Task<IEnumerable<발주서정보>> 발주서정보_조회Dz(string 발주번호, string 회사코드);

        [WampProcedure("VL_MES_SO_View")]
        Task<IEnumerable<주문서헤더정보>> VL_MES_SO_View(string 회사코드);
        [WampProcedure("주문서정보_조회Dz")]
        Task<IEnumerable<주문서정보>> 주문서정보_조회Dz(string 발주번호, string 회사코드);

        [WampProcedure("MES출고처리_출고처리헤더정보_등록_PDA")]
        Task<bool> MES출고처리_출고처리헤더정보_등록_PDA(출고처리헤더정보 출고처리, BARPLUS_LDELIVER 출고처리2);



        [WampProcedure("VL_MES_WO_WF_View")]
        Task<IEnumerable<외주작업지시헤더정보>> VL_MES_WO_WF_View(string 회사코드);


        [WampProcedure("외주작업지시정보_조회Dz")]
        Task<IEnumerable<외주작업지시서정보>> 외주작업지시서정보_조회Dz(string 회사코드);

        [WampProcedure("MES생산실적_사용자재보고정보_등록")]
        Task<bool> MES생산실적_사용자재보고정보_등록(사용자재보고정보 자재보고, bool isAdd);




        //20210505
        [WampProcedure("MES입고처리_NEW입고처리상세정보_등록")]
        Task<bool> MES입고처리_NEW입고처리상세정보_등록(입고처리상세정보 입고상세처리, BARPLUS_LSTOCK_D BLD, string 창고코드, string 위치코드, DateTime 보유년월일);





        //20210510
        [WampProcedure("MES생산관리_생산실적헤더정보_조회")]
        Task<List<생산실적헤더정보>> MES생산관리_생산실적헤더정보_조회(string 회사코드);


        [WampProcedure("MES생산관리_생산실적상세정보_조회")]
        Task<List<생산실적상세정보>> MES생산관리_생산실적상세정보_조회(string 생산지시코드, string 회사코드);


        [WampProcedure("MES재고이동_생산실적헤더정보_등록")]
        Task<bool> MES재고이동_생산실적헤더정보_등록(생산실적헤더정보 생산실적, 일괄생산실적헤더정보 일괄생산실적헤더, bool isAdd);


        [WampProcedure("MES재고이동_생산실적상세정보_등록")]
        Task<bool> MES재고이동_생산실적상세정보_등록(생산실적상세정보 생산실적상세, 일괄생산실적상세정보 일괄생산상세, bool isAdd);


        [WampProcedure("위치상세정보_저장")]
        Task 위치상세정보_저장(위치상세정보 info, bool isAdd);


        [WampProcedure("위치상세정보_조회")]
        Task<IEnumerable<위치상세정보>> 위치상세정보_조회(string 회사코드, string 장소위치코드);

        [WampProcedure("장소위치2_조회")]
        Task<IEnumerable<장소위치정보>> 장소위치2_조회(string 회사코드, string 장소코드);




        [WampProcedure("바코드발급정보_조회")]
        Task<IEnumerable<바코드발급정보>> 바코드발급정보_조회(string 회사코드);


        [WampProcedure("위치상세전체정보_조회")]
        Task<IEnumerable<위치상세정보>> 위치상세전체정보_조회(string 회사코드);


        [WampProcedure("보유품목위치정보Popup_조회")]
        Task<IEnumerable<보유품목위치정보>> 보유품목위치정보Popup_조회(string 회사코드, string 위치상세코드);



        [WampProcedure("공정단위BOM_정보_조회")]
        Task<IEnumerable<BOM_정보>> 공정단위BOM_정보_조회(string 회사코드, string 품목코드);


        [WampProcedure("공정단위자재정보_조회")]
        Task<List<공정단위자재정보>> 공정단위자재정보_조회(string 회사코드, string 공정단위코드);

        [WampProcedure("공정단위자재현황_조회")]
        Task<List<공정단위자재현황>> 공정단위자재현황_조회(string 회사코드, string 공정단위코드);


        // 20210516
        [WampProcedure("MES생산관리_작업자생산실적정보_조회")]
        Task<List<작업자생산실적정보>> MES생산관리_작업자생산실적정보_조회(string 회사코드);

        [WampProcedure("MES생산관리_작업자생산실적상세정보_조회")]
        Task<List<작업자생산실적정보>> MES생산관리_작업자생산실적상세정보_조회(string 생산지시코드, string 회사코드, string 작업자사번);



        [WampProcedure("품목별바코드발급정보_조회")]
        Task<IEnumerable<바코드발급정보>> 품목별바코드발급정보_조회(string 회사코드, string 품목코드);

        [WampProcedure("생산실적헤더LOT번호_등록")]
        Task<bool> 생산실적헤더LOT번호_등록(생산실적헤더정보 생산실적);


        [WampProcedure("재고조정품목_저장")]
        Task 재고조정품목_저장(재고조정품목정보 info, bool isAdd);

        [WampProcedure("재고조정품목_조회")]
        Task<IEnumerable<재고조정품목정보>> 재고조정품목_조회();


        [WampProcedure("재고조정품목_삭제")]
        Task 재고조정품목_삭제(string 품목코드);


        [WampProcedure("생산계획주문서정보_조회Dz")]
        Task<IEnumerable<주문서정보>> 생산계획주문서정보_조회Dz(string 회사코드);

        [WampProcedure("재고이동외주작업지시서정보_조회Dz")]
        Task<IEnumerable<외주작업지시서정보>> 재고이동외주작업지시서정보_조회Dz(string 회사코드, string 처리구분);


        [WampProcedure("외주작업지시서입고정보_조회Dz")]
        Task<IEnumerable<외주작업지시서정보>> 외주작업지시서입고정보_조회Dz(string 회사코드);


        [WampProcedure("보유품목장소위치Popup_조회")]
        Task<IEnumerable<보유품목위치정보>> 보유품목장소위치Popup_조회(string 회사코드, string 장소위치코드);


        [WampProcedure("외주제품입고처리_등록")]
        Task<bool> 외주제품입고처리_등록(작업외주생산실적등록정보 작업외주생산실적, 외주작업지시서품검정보 외주작업지시서);

        [WampProcedure("외주제품_위치등록")]
        Task<bool> 외주제품_위치등록(작업외주생산실적등록정보 작업외주생산실적, 외주작업지시서품검정보 외주작업지시서, decimal 불량수량);

        [WampProcedure("MES재고이동생산외주_재고이동상세정보_등록")]
        Task<bool> MES재고이동생산외주_재고이동상세정보_등록(재고이동상세정보 이동처리, bool isAdd);


        [WampProcedure("NEW공정단위BOM_정보_조회")]
        Task<IEnumerable<BOM_정보>> NEW공정단위BOM_정보_조회(string 회사코드);

        [WampProcedure("작업지시공정단위자재현황_조회")]
        Task<List<공정단위자재현황>> 작업지시공정단위자재현황_조회(string 회사코드, string 공정단위코드);



        //20210529

        [WampProcedure("외주작업지시서번호정보_조회Dz")]
        Task<외주작업지시서품검정보> 외주작업지시서번호정보_조회Dz(string 회사코드, string 지시번호);

        [WampProcedure("외주작업지시서품검정보_조회Dz")]
        Task<IEnumerable<외주작업지시서품검정보>> 외주작업지시서품검정보_조회Dz(string 회사코드);



        [WampProcedure("VL_MES_BOM_Auto")]
        Task<IEnumerable<BOM_정보>> VL_MES_BOM_Auto(string 회사코드);

        [WampProcedure("VL_MES_ITEM_Auto")]
        Task<IEnumerable<품목정보>> VL_MES_ITEM_Auto();



        [WampProcedure("MES외주생산실적_작업외주생산실적등록정보_등록")]
        Task<bool> MES외주생산실적_작업외주생산실적등록정보_등록(작업외주생산실적등록정보 생산실적, bool isAdd);

        [WampProcedure("MES외주생산실적_외주생산실적소유자재털기_등록")]
        Task<bool> MES외주생산실적_외주생산실적소유자재털기_등록(작업외주생산실적등록정보 생산실적파라미터, 외주작업지시서품검정보 외주작업지시, List<공정단위자재현황> listBom);

        [WampProcedure("MES외주생산실적_멀티외주생산실적소유자재털기_등록")]
        Task<bool> MES외주생산실적_멀티외주생산실적소유자재털기_등록(작업외주생산실적등록정보 생산실적파라미터, 외주작업지시서품검정보 외주작업지시, List<공정단위자재현황> listBom);



        [WampProcedure("외주생산실적소유자재털기_등록")]
        Task<bool> 외주생산실적소유자재털기_등록(작업외주생산실적등록정보 생산실적파라미터, 외주작업지시서품검정보 외주작업지시, List<공정단위자재현황> listBom);

        [WampProcedure("품목구분_조회")]
        public Task<품목정보> 품목구분_조회(string 품목코드);




        //수입검사
        [WampProcedure("발주서별수입검사_조회Dz")]
        Task<IEnumerable<발주서별수입검사>> 발주서별수입검사_조회Dz(string 회사코드);

        [WampProcedure("발주서별발주번호수입검사_조회Dz")]
        Task<발주서별수입검사> 발주서별발주번호수입검사_조회Dz(string 회사코드, string 발주번호, decimal 발주순번);


        [WampProcedure("수입검사입고처리_등록")]
        Task<bool> 수입검사입고처리_등록(수입실적등록정보 수입실적등록);



        [WampProcedure("작업지시공정단위자재현황2_조회")]
        Task<IEnumerable<공정단위자재현황>> 작업지시공정단위자재현황2_조회(string 회사코드, string 공정단위코드);




        [WampProcedure("작업지시상세_생산실적헤더정보_조회")]
        Task<List<생산실적헤더정보>> 작업지시상세_생산실적헤더정보_조회(string 회사코드, string 생산지시코드);


        [WampProcedure("외주생산위치정보_조회")]
        Task<List<외주생산위치정보>> 외주생산위치정보_조회( string 회사코드, string 지시번호);


        [WampProcedure("외주생산위치품목_조회")]
        Task<외주생산위치정보> 외주생산위치품목_조회(string 회사코드, string 지시번호, string 품목코드);


        [WampProcedure("반입처리_재고이동헤더정보_등록")]
        Task<string> 반입처리_재고이동헤더정보_등록(재고이동헤더정보 재고이동);

        [WampProcedure("반입처리_재고이동상세정보_등록")]
        Task<bool> 반입처리_재고이동상세정보_등록(재고이동상세정보 이동처리, decimal 필요수량);

        [WampProcedure("불량반입처리_재고이동상세정보_등록")]
        Task<bool> 불량반입처리_재고이동상세정보_등록(재고이동상세정보 이동처리, decimal 필요수량);


        [WampProcedure("공정단위생산실적헤더정보_조회")]
        Task<생산실적헤더정보> 공정단위생산실적헤더정보_조회(string 회사코드, string 생산지시코드, string 공정단위코드);


        [WampProcedure("직원부서_조회")]
        public Task<IEnumerable<직원정보>> 직원부서_조회(bool isOnlyUse, string 회사코드, string 부서코드);

        //2021.08.17
        [WampProcedure("VL_MES_BOM_Upload")]
        Task<IEnumerable<BOM_정보>> VL_MES_BOM_Upload(string 회사코드);

        [WampProcedure("VL_MES_ITEM_Upload")]
        Task<IEnumerable<품목정보>> VL_MES_ITEM_Upload();
    }

}
