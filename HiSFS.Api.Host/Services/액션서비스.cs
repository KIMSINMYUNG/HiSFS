using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Models.View;
using HiSFS.Api.Shared.Models.View_DZICUBE;
using HiSFS.Api.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace HiSFS.Api.Host.Services
{
    public class 액션서비스 : I액션서비스
    {
        private readonly IContextProvider cp;
        private 자재관리서비스 _자재관리;
        private 품질관리서비스 _품질관리;
        private 공통서비스 _공통관리;
        private 기준정보서비스 _기준정보;

        private int BeforeDay = -7;


        public 액션서비스(IContextProvider contextProvider, 자재관리서비스 자재관리,
            품질관리서비스 품질관리, 공통서비스 공통관리, 기준정보서비스 기준정보)
        {
            this.cp = contextProvider;
            _자재관리 = 자재관리;
            _품질관리 = 품질관리;
            _공통관리 = 공통관리;
            _기준정보 = 기준정보;
        }

        public Task 연동장비_등록(string deviceId, string deviceKind)
        {
            using var scope = cp.GetDbContextScope();
            var dc = scope.DbContext;

            var deviceKindCode = deviceKind switch
            {
                "PDA" => "S8101",                       // PDA
                _ => "S8199"                            // 기타
            };

            // 같은 유형의 장비 갯수 확인
            var deviceCount = dc.연동장비정보.Count(x => x.연동장비유형코드 == deviceKindCode);
            deviceCount++;

            var info = new 연동장비정보
            {
                사용유무 = false,
                삭제유무 = false,
                식별코드 = deviceId,
                장비명 = $"{deviceKind}-{deviceCount}",
                연동장비유형코드 = deviceKindCode,
                등록시각 = DateTime.Now
            };

            dc.연동장비정보.Add(info);
            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<bool> 연동장비_승인(string deviceId, bool isConfirm)
        {
            using var scope = cp.GetDbContextScope();
            var dc = scope.DbContext;

            var info = dc.연동장비정보.FirstOrDefault(x => x.식별코드 == deviceId);
            if (info == default)
                return Task.FromResult(false);

            info.사용유무 = isConfirm;
            dc.연동장비정보.Update(info);
            dc.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<(bool, bool)> 연동장비_등록확인(string 장비ID)
        {
            using var scope = cp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.연동장비정보.FirstOrDefault(x => x.식별코드 == 장비ID);
            if (result == null)
                return Task.FromResult((false, false));

            return Task.FromResult((true, result.사용유무));
        }

        //public async Task<IReadOnlyDictionary<string, dynamic>> 액션태그_스키마확인(string deviceId, string actionCode)
        //{
        //    using var scope = dcp.GetDbContextScope();
        //    var dc = scope.DbContext;

        //    var result = new Dictionary<string, dynamic>();

        //    await Task.Yield();

        //    var actionInfo = dc.액션정보.FirstOrDefault(x => x.액션코드 == actionCode);
        //    if (actionInfo == default)
        //    {
        //        result[ActionResult.오류] = "등록되지 않은 액션코드 입니다.";
        //        return result;
        //    }
        //    var 연동장비 = dc.연동장비정보.FirstOrDefault(x => x.식별코드 == deviceId && x.사용유무 == true);
        //    if (연동장비 == default)
        //    {
        //        result[ActionResult.오류] = "등록되지 않거나 승인되지 않는 장비 입니다.";
        //        return result;
        //    }

        //    result[ActionResult.액션인자] = actionInfo.액션인자;
        //    result[ActionResult.액션인자설명] = actionInfo.액션인자설명;

        //    return result;
        //}

        // 2021.04.27
        public Task<bool> 입고_판단(string deviceId, string actionCode, string 발주서코드)
        {
            using var scope = cp.GetDbContextScope();
            var dc = scope.DbContext;


            //int 일련번호자리수;
            //bool 일련번호_flag = false;
            string[] actionArgs_Ary = null;
            string actionArgsCode;
            string CompCode;
            if (발주서코드 != null)
            {

                actionArgs_Ary = 발주서코드.Split(':');
                actionArgsCode = actionArgs_Ary[0];
                //일련번호자리수 = actionArgs_Ary[2].Length;
                //if (일련번호자리수 == 5)
                //    일련번호_flag = true;
            }


            bool isOut = false;
            decimal 순번 = Convert.ToDecimal(actionArgs_Ary[1]);
            //발주서 조회 - 품목코드 가져온다
            CompCode = actionArgs_Ary[2];
            //string result;
            //var list = dc.발주서정보
            //    .Where(x => x.회사코드 == CompCode && x.발주번호 == actionArgs_Ary[0] && x.발주순번 == 순번)
            //    .Where_미삭제_사용()
            //    .Order_등록최신()
            //    .FirstOrDefault();
            //if (list == null) {
            //    isOut = false;
            //    return Task.FromResult(isOut);
            //}

            // 입고처리 되었는지 검색
            var bResult = dc.입고처리상세정보
                .Where(x => x.회사코드 == CompCode && x.발주번호 == actionArgs_Ary[0] && x.발주순번 == 순번)
                 .Where_미삭제_사용()
                .Order_등록최신()
                .FirstOrDefault();

            if (bResult == null)
                isOut = true;

            return Task.FromResult(isOut);
        }

        // 2021.04.30
        public Task<bool> 주문출고_판단(string deviceId, string actionCode, string 주문서코드)
        {
            using var scope = cp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scopeDz = cp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;


            //int 일련번호자리수;
            //bool 일련번호_flag = false;
            string[] actionArgs_Ary = null;
            string actionArgsCode;
            string CompCode;
            if (주문서코드 != null)
            {

                actionArgs_Ary = 주문서코드.Split(':');
                actionArgsCode = actionArgs_Ary[0];
                //일련번호자리수 = actionArgs_Ary[2].Length;
                //if (일련번호자리수 == 5)
                //    일련번호_flag = true;
            }


            bool isOut = false;
            decimal 순번 = Convert.ToDecimal(actionArgs_Ary[1]);
            //발주서 조회 - 품목코드 가져온다
            CompCode = actionArgs_Ary[2];
            //string result;
            //var list = dc.발주서정보
            //    .Where(x => x.회사코드 == CompCode && x.발주번호 == actionArgs_Ary[0] && x.발주순번 == 순번)
            //    .Where_미삭제_사용()
            //    .Order_등록최신()
            //    .FirstOrDefault();
            //if (list == null) {
            //    isOut = false;
            //    return Task.FromResult(isOut);
            //}

            // 출고처리 되었는지 검색
            var bResult = dc.출고처리상세정보
                .Where(x => x.회사코드 == CompCode && x.주문번호 == actionArgs_Ary[0] && x.주문순번 == 순번)
                 .Where_미삭제_사용()
                .Order_등록최신()
                .FirstOrDefault();

            if (bResult != null)
            {
                isOut = true;
                return Task.FromResult(isOut);
            }

            var resp = dcDz.VL_MES_SO
                            .Where(x => x.CO_CD == CompCode && x.SO_NB == actionArgs_Ary[0] && x.SO_SQ == 순번)
                            .FirstOrDefault();
            var pumCode = resp.ITEM_CD;
            var pumSu = resp.SO_QT;

            var 보유품목 = dc.보유품목정보
                                  .Where(x => x.회사코드 == CompCode && x.보유품목코드 == actionArgs_Ary[0])
                                  .FirstOrDefault();
            if (보유품목 == null)
            {
                isOut = true;
                return Task.FromResult(isOut);
            }
            else
            {
                var su = 보유품목.수량;
                if (pumSu > su)
                {
                    isOut = true;
                    return Task.FromResult(isOut);
                }
            }

            return Task.FromResult(isOut);
        }

        //*//
        public Task<bool> 출고_판단(string deviceId, string actionCode, string 보유품목코드)
        {
            using var scope = cp.GetDbContextScope();
            var dc = scope.DbContext;


            int 일련번호자리수;
            bool 일련번호_flag = false;
            string[] actionArgs_Ary = null;
            string actionArgsCode;
            if (보유품목코드 != null)
            {

                actionArgs_Ary = 보유품목코드.Split(':');
                actionArgsCode = actionArgs_Ary[0];
                if (actionArgs_Ary.Length > 2)
                {
                    일련번호자리수 = actionArgs_Ary[2].Length;
                    if (일련번호자리수 == 5)
                        일련번호_flag = true;
                }
            }


            bool isOut = false;
            if (actionCode == TagAction.A06_장소반출
                || actionCode == TagAction.A06_위치반출)
            {
                string result;
                try
                {
                    if (일련번호_flag == true)
                        result = dc.보유품목일련정보.FirstOrDefault(x => x.일년번호 == 보유품목코드).출고년월일;
                    else
                        result = dc.보유품목일지.FirstOrDefault(x => x.보유품목일지코드 == 보유품목코드).출고년월일;
                }
                catch (Exception ex)
                {
                    result = null;
                    isOut = false;
                }
                //if (일련번호_flag == true)
                //    result = dc.보유품목일련정보.FirstOrDefault(x => x.일년번호 == 보유품목코드).출고년월일;
                //else
                //    result = dc.보유품목일지.FirstOrDefault(x => x.보유품목일지코드 == 보유품목코드).출고년월일;

                if (result != null)
                    isOut = true;
            }
            return Task.FromResult(isOut);
        }
        //*//

        public Task<IReadOnlyDictionary<string, dynamic>> 액션태그_처리(string Comp, string deviceId, string actionCode, string[] actionArgList, bool isComplete)
        {
            using var scope = cp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scopeDz = cp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            var result = new Dictionary<string, dynamic>();

            var actionInfo = dc.액션정보.FirstOrDefault(x => x.액션코드 == actionCode);
            if (actionInfo == default)
            {
                result[ActionResult.오류] = "등록되지 않은 액션코드 입니다.";
                return Task.FromResult<IReadOnlyDictionary<string, dynamic>>(result);
            }
            var 연동장비 = dc.연동장비정보.FirstOrDefault(x => x.식별코드 == deviceId && x.사용유무 == true);
            if (연동장비 == default)
            {
                result[ActionResult.오류] = "등록되지 않거나 승인되지 않는 장비 입니다.";
                return Task.FromResult<IReadOnlyDictionary<string, dynamic>>(result);
            }

            var actionArgs = GetActionArgs(actionInfo.액션인자, actionArgList);


            var actionKind = actionInfo.액션유형코드;

            result = Process();
            result[ActionResult.액션명] = actionInfo.액션명;
            result[ActionResult.액션코드] = actionInfo.액션코드;
            result[ActionResult.액션인자] = actionInfo.액션인자;
            result[ActionResult.액션인자설명] = actionInfo.액션인자설명;

            return Task.FromResult<IReadOnlyDictionary<string, dynamic>>(result);

            // --------------------------------------------------------------------------------------------------------------

            Dictionary<string, dynamic> Process()
            {
                // #################
                // ## 작업자 선택 ##
                // #################
                if (actionCode == TagAction.A01_작업자선택)  // 작업자 선택
                {
                    var 직원 = dc.직원정보
                        .Include(x => x.부서)
                        .FirstOrDefault(x => x.사번 == actionArgs[TagArg.A01_작업자]);  // 작업자
                    if (직원 != default)
                    {
                        result[ActionResult.부서] = 직원.부서.부서명;
                        result[ActionResult.작업자] = 직원.사용자명;
                        result[ActionResult.대체액션] = actionInfo.대체액션코드;
                    }

                    SaveLog();
                }
                // ######################
                // ## 작업자 선택 해제 ##
                // ######################
                else if (actionCode == TagAction.A02_작업자해제)
                {
                    SaveLog();
                }
                // ###############
                // ## 장소 입고 ##
                // ###############
                else if (actionCode == TagAction.A03_장소입고)
                //else if (actionCode == TagAction.A03_장소입고 || actionCode == TagAction.A03_외주입고)
                {
                    // 장소정보
                    장소_얻기(dc, result, actionArgs, Comp);

                    var sanum = actionArgs["S9201"];

                    // 2021.04.02
                    string[] actionArgs_Ary;
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    var actionArgsCode = "";

                    int 일련번호자리수;
                    bool 일련번호_flag = false;
                    if (actionArgsStr != null)
                    {
                        actionArgs_Ary = actionArgsStr.Split(':');
                        actionArgsCode = actionArgs_Ary[0];
                        var ary_len = actionArgs_Ary.Length;
                        if (ary_len > 2)
                        {
                            일련번호자리수 = actionArgs_Ary[2].Length;
                            if (일련번호자리수 == 5)
                                일련번호_flag = true;
                        }
                    }
                    //////////////////////////////////////

                    if (isComplete == true)
                    {
                        //SaveLog();
                        SaveLogComp(Comp);
                        // 보유품목정보에 반영 {{{
                        // actionArgList[ 작업자, 장소, 보유품목, 수량 ]
                        var 품목_LOT = actionArgsStr;
                        var 품목_LOT_Ary = 품목_LOT.Split(':');
                        var LOT = 품목_LOT_Ary[1];

                        var 장소코드 = actionArgList[1];
                        var 보유품목코드 = actionArgList[2];
                        //2021.05.18 추가
                        var 품번_Ary = 보유품목코드.Split(':');
                        var 품번 = 품번_Ary[0];
                        var LOT번호 = 품번_Ary[1];
                        ////////////////////////////////////////
                        var 수량 = decimal.Parse(actionArgList[3]);

                        //2021.05.11
                        var 사유 = actionArgList[4];  //발주서 
                        var sawonCode = actionArgList[0];
                        var balCode_Ary = 사유.Split(':');
                        var balCode = balCode_Ary[0];
                        var 납기일 = balCode_Ary[1];
                        var whereCode = "";
                        var whereCode1 = "";
                        var whereCode2 = "";
                        whereCode = 장소코드;
                        if (whereCode.Length == 8)
                        {
                            whereCode1 = whereCode.Substring(0, 4);
                            whereCode2 = whereCode.Substring(4, 4);
                        }

                        //2021.05.22
                        //var 입고코드 =  actionArgList[5];  //입고사유
                        var 입고사유 = "";
                        //if (입고코드 == "S921901")
                        //    입고사유 = "구매";  // "0";   // 구매
                        //else if (입고코드 == "S921902")
                        //    입고사유 = "도급";  // "1";   // 외주
                        //else if (입고코드 == "S921903")
                        //    입고사유 = "생산";  // "2";   // 생산

                        //바코드품목_얻기_초기(dc, result, actionArgsStr, Comp);
                        //var 구분 = result[TagArg.A06_공정];
                        //if (구분 == "0")
                        //    입고사유 = "구매";
                        //else if (구분 == "1")
                        //    입고사유 = "외주";
                        //else if (구분 == "2")
                        //    입고사유 = "생산";
                        입고사유 = "구매";
                        var now = DateTime.Now;

                        //balCode와 납기일로 순번을 찾는다
                        var resp = dcDz.VL_MES_PO
                             .Where(x => x.CO_CD == Comp && x.PO_NB == balCode && x.SHIPREQ_DT == 납기일 && x.ITEM_CD == 품번)
                             .FirstOrDefault();

                        //    // 입고처리헤더정보 Search
                        var headresp = dc.입고처리헤더정보
                                       .Where(x => x.회사코드 == Comp && x.발주번호 == balCode)
                                       .Where_미삭제_사용()
                                       .Order_등록최신()
                                       .FirstOrDefault();

                        if (headresp == null)
                        {
                            //등록한다
                            if (resp != null)
                            {
                                //사원코드로 부서 찾는다
                                var sawonRec = dc.직원정보
                                    .Where(x => x.회사코드 == Comp && x.사번 == sawonCode)
                                    .FirstOrDefault();

                                var booseo = sawonRec.부서코드;

                                // MES
                                var 입고처리헤더정보 = new 입고처리헤더정보
                                {
                                    #region BARPLUS_LSTOCK
                                    회사코드 = Comp,  //    회사코드
                                                  //작업번호 = "202104210628",
                                    작업일자 = now, //  작업일자
                                    입고구분 = "0",                                    //   입고구분
                                    거래처코드 = resp.TR_CD,        //   거래처코드
                                    입고일자 = now,   //    입고일자
                                    입고창고 = whereCode1,//장소위치정보selected != null ? 장소위치정보selected.장소코드 : null,          //    입고창고
                                    입고장소 = whereCode2,//장소위치정보selected != null ? 장소위치정보selected.위치코드 : null,         // 입고장소
                                    발주번호 = balCode, //발주서헤더정보selected != null ? 발주서헤더정보selected.발주번호 : null,  //    발주번호
                                    거래구분 = resp.PO_FG,//    거래구분
                                    환종 = "",//  환종
                                    환율 = Convert.ToDecimal(0),//    환율
                                    LC여부 = "",//    LC여부
                                    사원코드 = resp.PLN_CD,//"1001",//직원정보selected != null ? 직원정보selected.사번 : null,  //    사원코드
                                    부서코드 = booseo, //부서정보selected != null ? 부서정보selected.부서코드 : null,  //   부서코드
                                    사업장코드 = "1000", //사업장코드, // 사업장코드
                                    프로젝트코드 = "",//  프로젝트코드
                                    과세구분 = resp.VAT_FG, //과세구분
                                    작업구분 = "", //   작업구분
                                    관리구분코드 = "", // 관리구분코드
                                    EXCST_NB = "", //
                                    배부여부 = "", //   배부여부
                                    비고 = "", // 비고
                                    최초입력사원코드 = "", //   최초입력사원코드
                                    최초입력일 = null,   //  최초입력일
                                    최초입력IP = "", // 최초입력IP
                                    수정사원코드 = "", // 수정사원코드
                                    수정일 = null,   //    수정일
                                    수정IP = "", //   수정IP
                                    DUMMY1 = "",//
                                    DUMMY2 = "",    //
                                    DUMMY3 = "",//
                                    PLN_CD = "",    //
                                    SO_NB3 = "",    //
                                    UMVAT_FG = "0",     //
                                    APP_FG = "0",  //

                                    #endregion
                                };

                                //ERP  BARPLUS_LSTOCK
                                var BARPLUS_LSTOCK = new BARPLUS_LSTOCK
                                {
                                    #region BARPLUS_LSTOCK

                                    CO_CD = Comp,  //   회사코드
                                                   //WORK_NB = "202104210628",       // 작업번호
                                    WORK_DT = now.ToString("yyyyMMdd"),  // 작업일자
                                    RCV_FG = "0",                                    // 입고구분
                                    TR_CD = resp.TR_CD, //거래처정보selected != null ? 거래처정보selected.거래처코드 : null,          //   거래처코드
                                    RCV_DT = now.ToString("yyyyMMdd"),   // 입고일자
                                    WH_CD = whereCode1,//장소위치정보selected != null ? 장소위치정보selected.장소코드 : null,         //    입고창고
                                    LC_CD = whereCode2,//장소위치정보selected != null ? 장소위치정보selected.위치코드 : null,          //   입고장소
                                    PO_NB = balCode,//발주서헤더정보selected != null ? 발주서헤더정보selected.발주번호 : null, // 발주번호
                                    PO_FG = resp.PO_FG,//   거래구분
                                    EXCH_CD = "",// 환종
                                    EXCH_RT = Convert.ToDecimal(0),//   환율
                                    LC_YN = "",//   LC여부

                                    EMP_CD = sawonCode,//직원정보selected != null ? 직원정보selected.사번 : null,  // 사원코드
                                    DEPT_CD = booseo, //부서정보selected != null ? 부서정보selected.부서코드 : null,  //    부서코드
                                    DIV_CD = "1000", // 사업장코드
                                    PJT_CD = resp.PJT_CD,// 프로젝트코드
                                    VAT_FG = resp.VAT_FG, //    과세구분
                                    MAP_FG = "", // 작업구분
                                    MGMT_CD = "", //    관리구분코드
                                    EXCST_NB = "", //
                                    DIST_YN = "", //    배부여부
                                    REMARK_DC = "", //  비고
                                    INSERT_ID = "", //  최초입력사원코드
                                    INSERT_DT = null,   //  최초입력일
                                    INSERT_IP = "", //  최초입력IP
                                    MODIFY_ID = "", //  수정사원코드
                                    MODIFY_DT = null,   //  수정일
                                    MODIFY_IP = "", //  수정IP
                                    DUMMY1 = "",//
                                    DUMMY2 = "",    //
                                    DUMMY3 = "",//
                                    PLN_CD = "",    //
                                    SO_NB3 = "",    //
                                    UMVAT_FG = "0",     //
                                    APP_FG = "0",  //

                                    #endregion

                                };

                                _기준정보.MES입고처리_입고처리헤더정보_등록(입고처리헤더정보, BARPLUS_LSTOCK);

                            }
                        }

                        Task.Delay(300);
                        headresp = dc.입고처리헤더정보
                                       .Where(x => x.회사코드 == Comp && x.발주번호 == balCode)
                                       .Where_미삭제_사용()
                                       .Order_등록최신()
                                       .FirstOrDefault();

                        //입고처리상세정보 등록
                        var 입고처리상세정보 = new 입고처리상세정보
                        {
                            회사코드 = Comp,
                            작업번호 = headresp.작업번호,
                            //작업순번  = 1,
                            품번 = resp.ITEM_CD, //품목코드,
                            입고수량_관리단위 = 수량, //resp.PO_QT, //입고수량_관리단위,
                            입고수량_재고단위 = 수량, // resp.PO_QT, //입고수량_재고단위,
                            //입고단가
                            //공급가
                            //부가세
                            //합계액
                            //환종
                            //환율
                            //외화단가
                            //외화금액
                            LOT번호 = LOT,
                            발주번호 = resp.PO_NB, //발주번호,
                            발주순번 = resp.PO_SQ, //발주순번,

                            선적번호 = "",
                            선적순번 = 1,
                            //사용여부
                            //유효여부
                            //단가구분
                            입고장소코드 = whereCode2, //장소위치정보selected != null ? 장소위치정보selected.위치코드 : null,
                            비고 = "",

                        };

                        //보유품목 입고 처리도 한다
                        _기준정보.MES입고처리_입고처리상세정보_등록(입고처리상세정보);

                        var BARPLUS_LSTOCK_D = new BARPLUS_LSTOCK_D
                        {
                            CO_CD = Comp,  //   회사코드    ,
                            WORK_NB = headresp.작업번호,          //    작업번호    ,
                                                              //WORK_SQ = 1,    //    작업순번    ,
                            ITEM_CD = resp.ITEM_CD,
                            PO_QT = 수량, //resp.PO_QT, //입고수량_관리단위,  //    입고수량(관리단위)  ,
                            RCV_QT = 수량, //resp.PO_QT, //입고수량_재고단위,// 입고수량(재고단위)  ,
                            //RCV_UM        = //    입고단가    ,
                            //RCVG_AM       //  공급가 ,
                            //RCVV_AM       //  부가세 ,
                            //RCVH_AM       //  합계액 ,
                            //EXCH_CD       //  환종  ,
                            //EXCH_RT       //  환율  ,
                            //EXCH_UM       //  외화단가    ,
                            //EXCH_AM       //  외화금액    ,

                            LOT_NB = LOT,    //  LOT번호   ,
                            PO_NB = resp.PO_NB,
                            PO_SQ = resp.PO_SQ,
                            //REQ_NB        //  입고의뢰번호  ,
                            //REQ_SQ        //  입고의뢰순번  ,
                            //IBL_NB        //  선적번호    ,
                            //IBL_SQ        //  선적순번    ,
                            USE_YN = "0",// 사용여부    ,
                            EXPIRE_YN = "Y",  //    유효여부    ,
                                              //UM_FG     //  단가구분    ,
                            CONF_NB3 = 0,
                            LC_CD = whereCode2, //장소위치정보selected != null ? 장소위치정보selected.위치코드 : "", // 입고장소코드  ,
                            REMARK_DC = "",                         //  비고  ,
                            APP_FG = "0",
                        };

                        _기준정보.더존입고상세_BARPLUS_LSTOCK_D(BARPLUS_LSTOCK_D);

                        #region  
                        string str장소코드 = whereCode1; // 장소위치정보selected != null ? 장소위치정보selected.장소코드 : 창고코드;
                        string str위치코드 = whereCode2; // 장소위치정보selected != null ? 장소위치정보selected.위치코드 : null;
                        string str장소위치코드 = $"{str장소코드}{str위치코드}";


                        var info1 = new 입고처리상세정보
                        {
                            품번 = resp.ITEM_CD, //품목코드,
                            회사코드 = Comp, //회사코드,
                            //품목코드 = 품목코드,
                            입고수량_관리단위 = Convert.ToInt32(수량), //Convert.ToInt32(resp.PO_QT),
                            //장소코드 = str장소코드,
                            입고장소코드 = str장소위치코드,
                            //보유년월일 = now.ToString("yyMMdd"),
                            작업순번 = 0,
                            LOT번호 = LOT,
                            비고 = 품목_LOT,
                            입고수량_재고단위 = Convert.ToInt32(수량),

                        };
                        #endregion

                        //보유품목입고 true면 상기의 수량 일방적으로 적용
                        _자재관리.입고관리보유품목입고Action_등록(info1, false);

                        #region
                        string str거래구분 = string.Empty;
                        var _거래 = resp.PO_FG;
                        if (_거래 == "0")
                            str거래구분 = "DOMESTIC";
                        else if (_거래 == "1")
                            str거래구분 = "LOCAL L / C ";
                        else if (_거래 == "2")
                            str거래구분 = "구매승인서";
                        else if (_거래 == "3")
                            str거래구분 = "MASTER L / C";
                        else if (_거래 == "4")
                            str거래구분 = "T / T";
                        else if (_거래 == "5" || _거래 == "6")
                            str거래구분 = "기타";
                        #endregion

                        _자재관리.보유품목LOT입고_위치등록(info1, 입고사유, str거래구분, null, true);

                        바코드품목_얻기(dc, result, actionArgsStr, Comp);
                        자재발주유형_얻기(dcDz, result, actionArgs, actionArgsStr, Comp);

                        ;
                        //if (구분 == "0")
                        //    자재발주유형_얻기(dcDz, result, actionArgs, actionArgsStr, Comp);
                        //else if (구분 == "1")
                        //    자재발주유형_외주얻기(dcDz, result, actionArgs, actionArgsStr, Comp);
                        //else if (구분 == "2")
                        //    입고사유 = "생산";

                        //if (actionCode == TagAction.A03_장소입고)
                        //    자재발주유형_얻기(dcDz, result, actionArgs, actionArgsStr, Comp);
                        //else
                        //    자재발주유형_외주얻기(dcDz, result, actionArgs, actionArgsStr, Comp);
                        return result;


                    }

                    //보유품목_얻기(dc, result, actionArgs);
                    if (actionArgsStr != null)
                    {
                        //보유품목_얻기(dc, result, actionArgs);
                        //보유품목_얻기_반출(dc, result, actionArgsCode);
                        //2021.05.11
                        var 입고유무 = 바코드품목_얻기_초기(dc, result, actionArgsStr, Comp);
                        if (!입고유무)
                        {
                            _공통관리.메시지ToPDA("InINPUTScan", sanum);
                        }
                        else
                        {
                            var 구분 = result[TagArg.A06_공정];
                            //if(구분 == "0")
                            //   자재발주유형_얻기(dcDz, result, actionArgs, actionArgsStr, Comp);
                            //else if (구분 == "1")
                            //    자재발주유형_외주_Date(dcDz, result, actionArgs, actionArgsStr, Comp);
                            ////자재구매유형_얻기(dc, result, actionArgs);
                            자재발주유형_얻기(dcDz, result, actionArgs, actionArgsStr, Comp);
                        }

                    }

                    // 자재출고 유형 얻기 2021.03.12
                    //자재입고유형_얻기(dc, result, actionArgs);
                    //자재발주유형_얻기(dc, result, actionArgs, actionArgsStr, Comp);
                }
                // ###########################
                // ## 장소 입고시 불량 입력 ##
                // ###########################
                else if (actionCode == TagAction.A04_장소입고시불량입력)
                {
                    // 장소정보
                    장소_얻기(dc, result, actionArgs, Comp);

                    // 2021.04.02
                    string[] actionArgs_Ary;
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    var actionArgsCode = "";

                    int 일련번호자리수;
                    bool 일련번호_flag = false;
                    if (actionArgsStr != null)
                    {
                        actionArgs_Ary = actionArgsStr.Split(':');
                        actionArgsCode = actionArgs_Ary[0];
                        일련번호자리수 = actionArgs_Ary[2].Length;
                        if (일련번호자리수 == 5)
                            일련번호_flag = true;
                    }
                    //////////////////////////////////////

                    if (isComplete == true)
                    {
                        SaveLog();

                        // 불량등록 반영 {{{
                        var 보유품목코드 = actionArgList[2];
                        var 수량 = decimal.Parse(actionArgList[3]);
                        var 불량유형코드 = actionArgList[4];

                        //_자재관리.보유품목_불량등록(보유품목코드, 수량, 불량유형코드);
                        _자재관리.보유품목_불량등록(actionArgsCode, 수량, 불량유형코드);
                        // }}}

                        // 자재입고 불량유형 얻기
                        자재입고불량유형_얻기(dc, result, actionArgs);

                        // 보유품목 정보
                        //자재입고불량_얻기(dc, result, actionArgs, 보유품목코드);
                        자재입고불량_얻기(dc, result, actionArgs, actionArgsCode);

                        return result;
                    }

                    ///보유품목_얻기(dc, result, actionArgs);
                    if (actionArgsStr != null)
                    {
                        //보유품목_얻기(dc, result, actionArgs);
                        보유품목_얻기_반출(dc, result, actionArgsCode);

                        // 자재입고 불량유형 얻기
                        자재입고불량유형_얻기(dc, result, actionArgs);
                    }


                }
                // ####################
                // ## 장소 위치 배치 ##
                // ####################
                else if (actionCode == TagAction.A05_장소위치배치)
                {
                    //var 장소 = dc.장소위치정보
                    //    .FirstOrDefault(x => x.장소위치코드 == actionArgs[TagArg.A02_장소] && x.회사코드 == Comp);  // 장소
                    //if (장소 != default)
                    //{
                    //    result[$"{TagArg.A02_장소}.Text"] = 장소.위치명;
                    //    result[TagArg.A02_장소] = 장소.장소위치코드;
                    //}
                    // 장소위치정보
                    var 위치상세 = dc.위치상세정보
                        .Include(x => x.장소위치)
                        .ThenInclude(y => y.장소)
                        .FirstOrDefault(x => x.위치상세코드 == actionArgs[TagArg.A03_장소위치] && x.회사코드 == Comp);  // 장소
                    if (위치상세 != default)
                    {
                        result[$"{TagArg.A03_장소위치}.Text"] = $"{위치상세.장소위치.위치명}\n{위치상세.위치명}";
                        result[TagArg.A03_장소위치] = 위치상세.위치상세코드;
                    }

                    // 2021.04.02
                    string[] actionArgs_Ary;
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    var actionArgsCode = "";

                    int 일련번호자리수;
                    bool 일련번호_flag = false;
                    if (actionArgsStr != null)
                    {
                        actionArgs_Ary = actionArgsStr.Split(':');
                        actionArgsCode = actionArgs_Ary[0];
                        var ary_len = actionArgs_Ary.Length;
                        if (ary_len > 2)
                        {
                            일련번호자리수 = actionArgs_Ary[2].Length;
                            if (일련번호자리수 == 5)
                                일련번호_flag = true;
                        }
                    }
                    //////////////////////////////////////

                    if (isComplete == true)
                    {
                        //SaveLog();
                        SaveLogComp(Comp);
                        // 보유품목정보에 반영 {{{
                        // actionArgList[ 작업자, 장소위치, 보유품목, 수량 ]

                        //var info = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == actionArgList[2]); // 보유품목 선택
                        //var info = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == actionArgsCode); // 보유품목 선택
                        //품목코드
                        var 품번_LOT = actionArgs[TagArg.A04_보유품목];
                        //위치상세코드
                        var 위치상세코드 = actionArgs[TagArg.A03_장소위치];
                        var 장소위치코드 = 위치상세코드.Substring(0, 8);
                        decimal D수량 = decimal.Parse(actionArgList[3]);

                        var actArgs_Ary = 품번_LOT.Split(':');
                        var actArgsCode = actArgs_Ary[0];
                        var actLotCode = actArgs_Ary[1];

                        var 입고보유품목 = dc.보유품목위치정보
                                                    .Include(x => x.보유품목)
                                                    .ThenInclude(y => y.품목)
                                                    .Where(x => x.보유품목코드 == actArgsCode && x.장소위치코드 == 장소위치코드 && x.품목_LOT번호 == 품번_LOT)
                                                    .FirstOrDefault();

                        //var info1 = new 보유품목위치정보
                        //{
                        //    보유품목코드 = 입고보유품목.보유품목코드, //품목코드,
                        //    회사코드 = Comp, //회사코드,
                        //    장소위치코드 = 입고보유품목.장소위치코드,
                        //    수량 = Convert.ToInt32(수량), //Convert.ToInt32(resp.PO_QT),
                        //    LOT번호 = actLotCode,
                        //    품목_LOT번호 = 품번_LOT,
                        //    //보유년월일 = now.ToString("yyMMdd"),                         
                        //};

                        var info1 = new 입고처리상세정보
                        {
                            품번 = 입고보유품목.보유품목코드, //품목코드,
                            회사코드 = Comp, //회사코드,
                            //품목코드 = 품목코드,
                            입고수량_관리단위 = Convert.ToInt32(D수량), //Convert.ToInt32(resp.PO_QT),
                            //장소코드 = str장소코드,
                            입고장소코드 = 입고보유품목.장소위치코드,
                            //보유년월일 = now.ToString("yyMMdd"),
                            작업순번 = 0,
                            LOT번호 = actLotCode,
                            비고 = 품번_LOT,
                        };

                        _자재관리.보유품목LOT입고_위치등록(info1, "위치배치", "입고위치배치", 위치상세코드, true);

                        //var 장소위치정보 = dc.장소위치정보.Include(x => x.장소).FirstOrDefault(x => x.장소위치코드 == actionArgList[1]);
                        if (입고보유품목 != default)
                        {
                            // 보유품목 수량 변경
                            // 장소 변경
                            입고보유품목.수량 = 입고보유품목.수량 - D수량;
                            if (입고보유품목.수량 > 0)
                            {
                                dc.보유품목위치정보.Update(입고보유품목);
                            }
                            else
                            {
                                dc.보유품목위치정보.Remove(입고보유품목);
                            }


                            dc.SaveChanges();

                            // 2021.05.12
                            바코드입고품목_얻기(dc, result, actionArgs, Comp, true);
                        }
                        // }}}


                        return result;
                    }

                    // 보유품목 정보
                    //보유품목_얻기(dc, result, actionArgs);
                    if (actionArgsStr != null)
                    {
                        //보유품목_얻기(dc, result, actionArgs);
                        //보유품목_얻기_반출(dc, result, actionArgsCode);
                        //2021.05.12
                        바코드입고품목_얻기(dc, result, actionArgs, Comp, true);
                    }
                }
                // ###############
                // ## 장소 반출 ##
                // ###############
                else if (actionCode == TagAction.A06_장소반출)
                {
                    // 장소정보
                    장소_얻기(dc, result, actionArgs, Comp);

                    // 2021.04.02
                    string[] actionArgs_Ary;
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    var actionArgsCode = "";

                    int 일련번호자리수;
                    bool 일련번호_flag = false;
                    if (actionArgsStr != null)
                    {
                        actionArgs_Ary = actionArgsStr.Split(':');
                        actionArgsCode = actionArgs_Ary[0];
                        var ary_len = actionArgs_Ary.Length;
                        if (ary_len > 2)
                        {
                            일련번호자리수 = actionArgs_Ary[2].Length;
                            if (일련번호자리수 == 5)
                                일련번호_flag = true;
                        }
                    }
                    //////////////////////////////////////

                    if (isComplete == true)
                    {
                        //SaveLog();
                        SaveLogComp(Comp);

                        // 보유품목정보에 반영 {{{
                        // actionArgList[ 작업자, 장소, 보유품목, 수량 ]
                        //var info = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == actionArgList[2]); // 보유품목 선택
                        var info = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == actionArgsCode && x.회사코드 == Comp); // 보유품목 선택
                        //var 장소위치정보 = dc.장소위치정보.Include(x => x.장소).FirstOrDefault(x => x.장소위치코드 == actionArgList[1]);
                        if (info != default)
                        {
                            // actionArgsStr = "Z0002:00001"
                            var 품목_LOT = actionArgsStr;
                            var 품목_LOT_Ary = 품목_LOT.Split(':');
                            var 품번 = 품목_LOT_Ary[0];
                            var LOT = 품목_LOT_Ary[1];

                            var 장소코드 = actionArgList[1];
                            var 보유품목코드 = actionArgList[2];
                            var 수량 = decimal.Parse(actionArgList[4]);

                            //2021.05.11
                            var 사유 = actionArgList[3];  //발주서:납기예정일 
                            var sawonCode = actionArgList[0];
                            var balCode_Ary = 사유.Split(':');
                            var balCode = balCode_Ary[0]; //발주서번호
                            var 납기일 = balCode_Ary[1];  //납기예정일
                            var whereCode = "";
                            var whereCode1 = "";
                            var whereCode2 = "";
                            whereCode = 장소코드;
                            if (whereCode.Length == 8)
                            {
                                whereCode1 = whereCode.Substring(0, 4);
                                whereCode2 = whereCode.Substring(4, 4);
                            }

                            var now = DateTime.Now;
                            string 작업번호str = "";


                            var resp = dcDz.VL_MES_SO
                            .Where(x => x.CO_CD == Comp && x.SO_NB == balCode && x.SHIPREQ_DT == 납기일 && x.ITEM_CD == 품번)
                            .FirstOrDefault();

                            //출고처리헤더정보 Search
                            var headresp = dc.출고처리헤더정보
                                           .Where(x => x.회사코드 == Comp && x.주문번호 == balCode)
                                           .Where_미삭제_사용()
                                           .Order_등록최신()
                                           .FirstOrDefault();

                            if (headresp == null)
                            {
                                //등록한다
                                if (resp != null)
                                {
                                    //사원코드로 부서 찾는다
                                    var sawonRec = dc.직원정보
                                        .Where(x => x.회사코드 == Comp && x.사번 == sawonCode)
                                        .FirstOrDefault();

                                    var booseo = sawonRec.부서코드;

                                    // MES
                                    var 순번_더존 = dcDz.BARPLUS_LDELIVER.Count(x => x.CO_CD == Comp) + 1;
                                    now = DateTime.Now;
                                    var yyyy = now.ToString("yyyy");
                                    string 작업번호2 = $"{"IS"}{yyyy}{순번_더존:000000}";
                                    작업번호str = 작업번호2;

                                    var 출고처리헤더정보 = new 출고처리헤더정보
                                    {
                                        #region BARPLUS_LSTOCK
                                        회사코드 = Comp,  //    회사코드
                                                      //작업번호 = "202104210628",
                                        작업번호 = 작업번호2,
                                        작업일자 = now,
                                        출고구분 = "0", //.출고구분,
                                        거래처코드 = resp.TR_CD,
                                        출고일자 = now,
                                        주문번호 = balCode,
                                        창고코드 = whereCode1,
                                        거래구분 = "0",//   거래구분
                                        환종 = "",//  환종
                                        환율 = Convert.ToDecimal(0),//    환율
                                        사원코드 = sawonCode,
                                        부서코드 = booseo,
                                        사업장코드 = "1000",
                                        과세구분 = resp.VAT_FG,
                                        단가구분 = resp.UMVAT_FG,
                                        연동구분 = "0",  //resp.연동구분,
                                        #endregion
                                    };

                                    var BARPLUS_LDELIVER = new BARPLUS_LDELIVER
                                    {
                                        CO_CD = Comp,
                                        WORK_NB = 작업번호2,
                                        WORK_DT = now.ToString("yyyyMMdd"), //String.Format("{0:yyyyMMdd}", Convert.ToDateTime(출고처리.작업일자.ToString())),
                                        ISU_FG = "0", //출고구분
                                        TR_CD = resp.TR_CD,
                                        ISU_DT = now.ToString("yyyyMMdd"), //String.Format("{0:yyyyMMdd}", Convert.ToDateTime(출고처리.출고일자.ToString())),
                                        WH_CD = whereCode1,
                                        SO_FG = "0", //resp.거래구분,
                                        EXCH_CD = resp.EXCH_CD, // .출고처리.환종,
                                        EXCH_RT = 1, //resp.환율, //출고처리.환율,
                                        EMP_CD = sawonCode,
                                        DEPT_CD = booseo,
                                        DIV_CD = "1000",  //사업장코드,
                                        VAT_FG = resp.VAT_FG,
                                        UMVAT_FG = resp.UMVAT_FG,
                                        APP_FG = "0", //headresp.연동구분,
                                        SHIP_CD = resp.SHIP_CD,
                                        REMARK_DC = "",
                                        PLN_CD = "",
                                    };

                                    _기준정보.MES출고처리_출고처리헤더정보_등록_PDA(출고처리헤더정보, BARPLUS_LDELIVER);

                                }
                            }
                            else
                            {
                                작업번호str = headresp.작업번호;
                            }

                            //출고처리상세정보 등록
                            var 출고처리상세정보 = new 출고처리상세정보
                            {
                                회사코드 = Comp,
                                작업번호 = 작업번호str,
                                작업순번 = 1,
                                품번 = resp.ITEM_CD, //품목코드,
                                출고수량_관리단위 = 수량, //resp.SO_QT, //출고수량_관리단위,
                                출고수량_재고단위 = 수량, //resp.SO_QT, //출고수량_재고단위,
                                주문번호 = resp.SO_NB, //주문번호,
                                주문순번 = resp.SO_SQ, //주문순번,

                                장소코드 = whereCode2,
                                연동구분 = "1",
                                LOT번호 = LOT,
                            };

                            var info1 = new 출고처리상세정보
                            {
                                품번 = resp.ITEM_CD, //품목코드,
                                회사코드 = Comp, //회사코드,
                                             //품목코드 = 품목코드,
                                출고수량_관리단위 = Convert.ToInt32(수량), //resp.SO_QT),
                                //장소코드 = str장소코드,
                                장소코드 = whereCode,
                                //보유년월일 = now.ToString("yyMMdd"),
                                작업순번 = 0,

                            };
                            //#endregion

                            //_자재관리.출고관리보유품목입고_등록(info1, true);

                            _기준정보.MES출고처리_출고처리상세정보_등록(출고처리상세정보, true);


                            var info2 = new 보유품목정보
                            {
                                보유품목코드 = resp.ITEM_CD,
                                회사코드 = Comp,
                                품목코드 = resp.ITEM_CD,
                                수량 = Convert.ToInt32(수량),
                                장소코드 = whereCode1,
                                장소위치코드 = whereCode,
                                보유년월일 = now.ToString("yyMMdd"),
                                LOT번호 = LOT,
                                품목_LOT번호 = 품목_LOT,

                            };
                            _자재관리.보유품목LOT출고_위치등록(info2, "출고처리", "", null);


                            /*
                            // 보유품목정보에 반영 {{{
                            // actionArgList[ 작업자, 장소, 보유품목, 수량 ]
                            var 장소코드 = actionArgList[1];
                            //var 장소코드 = 장소위치정보.장소코드;
                            var 보유품목코드 = actionArgList[2];
                            var 수량 = 1;
                            if (!일련번호_flag)
                            {
                                var act_Ary = actionArgList[2].Split(':');
                                수량 = Convert.ToInt32(act_Ary[2]);
                            }
                            //var 수량 = decimal.Parse(actionArgList[3]);

                            var 사유 = actionArgList[4];
                            //_자재관리.보유품목_출고(보유품목코드, 수량, 장소코드, null, 사유);
                            _자재관리.보유품목_출고(actionArgsCode, 수량, 장소코드, null, 사유);

                            if (사유 != "S921802") // 이동출고
                                _자재관리.자재관리_보유품목일련번호생성_Update(보유품목코드, 수량, 장소코드, null, 사유);
                            */
                            //*// 보유품목 수량 변경
                            //info.수량 = info.수량 - 수량;      

                            // 장소 변경
                            //info.장소코드 = null;
                            //info.장소위치코드 = null;
                            //dc.보유품목정보.Update(info);
                            //_자재관리.보유품목_출고(보유품목코드, 수량, 장소코드);
                            //dc.SaveChanges(); */

                            /*
                            // 보유품목 정보
                            //보유품목_얻기(dc, result, actionArgs);
                            보유품목_얻기_반출(dc, result, actionArgsCode);

                            // 자재출고 유형 얻기 2021.03.08
                            자재출고유형_얻기(dc, result, actionArgs);
                            */

                            //2021.05.12
                            바코드품목_얻기_출고(dc, result, actionArgsStr, Comp);

                            자재주문유형_얻기(dcDz, result, actionArgs, actionArgsStr, Comp);
                        }
                        // }}}

                        return result;
                    }

                    // 보유품목 정보
                    if (actionArgsStr != null)
                    {
                        /*
                        //보유품목_얻기(dc, result, actionArgs);
                        보유품목_얻기_반출(dc, result, actionArgsCode);
                        // 자재출고 유형 얻기
                        자재출고유형_얻기(dc, result, actionArgs);
                        */
                        //2021.05.12
                        바코드품목_얻기_출고(dc, result, actionArgsStr, Comp);

                        자재주문유형_얻기(dcDz, result, actionArgs, actionArgsStr, Comp);
                    }


                    // 자재출고 유형 얻기
                    //자재출고유형_얻기(dc, result, actionArgs);
                }
                // ###############
                // ## 위치 반출 ##
                // ###############
                else if (actionCode == TagAction.A06_위치반출)
                {
                    /*
                    // 장소위치정보
                    var 장소위치 = dc.장소위치정보
                        .Include(x => x.장소)
                        .FirstOrDefault(x => x.장소위치코드 == actionArgs[TagArg.A03_장소위치]);  // 장소
                    if (장소위치 != default)
                    {
                        result[$"{TagArg.A03_장소위치}.Text"] = $"{장소위치.장소.장소명}\n{장소위치.위치명}";
                        result[TagArg.A03_장소위치] = 장소위치.장소위치코드;
                        //result[$"{TagArg.A02_장소}.Text"] = 장소위치.장소코드.장소명;
                        result[TagArg.A02_장소] = 장소위치.장소코드;
                        var 장소 = dc.장소정보
                        .FirstOrDefault(x => x.장소코드 == 장소위치.장소코드);  // 장소
                        if (장소 != default)
                        {
                            result[$"{TagArg.A02_장소}.Text"] = 장소.장소명;
                        }
                    }
                    */
                    var 위치상세 = dc.위치상세정보
                        .Include(x => x.장소위치)
                        .ThenInclude(y => y.장소)
                        .FirstOrDefault(x => x.위치상세코드 == actionArgs[TagArg.A03_장소위치] && x.회사코드 == Comp);  // 장소
                    if (위치상세 != default)
                    {
                        result[$"{TagArg.A03_장소위치}.Text"] = $"{위치상세.장소위치.위치명}\n{위치상세.위치명}";
                        result[TagArg.A03_장소위치] = 위치상세.위치상세코드;
                    }

                    // 2021.05.13
                    string[] actionArgs_Ary;
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    var actionArgsCode = "";

                    int 일련번호자리수;
                    bool 일련번호_flag = false;
                    if (actionArgsStr != null)
                    {
                        if (actionArgsStr == "READ FAIL")
                            return result;
                        actionArgs_Ary = actionArgsStr.Split(':');
                        actionArgsCode = actionArgs_Ary[0];
                        var ary_len = actionArgs_Ary.Length;
                        if (ary_len > 2)
                        {
                            일련번호자리수 = actionArgs_Ary[2].Length;
                            if (일련번호자리수 == 5)
                                일련번호_flag = true;
                        }
                    }
                    //////////////////////////////////////

                    if (isComplete == true)
                    {
                        SaveLogComp(Comp);

                        // 보유품목정보에 반영 {{{
                        // actionArgList[ 작업자, 장소, 보유품목, 수량 ]
                        //var info = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == actionArgList[2]); // 보유품목 선택
                        var info = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == actionArgsCode); // 보유품목 선택
                        //var 장소위치정보 = dc.장소위치정보.Include(x => x.장소).FirstOrDefault(x => x.장소위치코드 == actionArgList[1]);
                        if (info != default)
                        {

                            // actionArgsStr = "Z0002:00001"
                            var 품목_LOT = actionArgsStr;
                            var 품목_LOT_Ary = 품목_LOT.Split(':');
                            var LOT = 품목_LOT_Ary[1];

                            var 장소코드 = actionArgList[1];
                            var 보유품목코드 = actionArgList[2];
                            var 수량 = decimal.Parse(actionArgList[3]);

                            //2021.05.11
                            var 사유 = actionArgList[4];  //발주서:납기예정일 
                            var sawonCode = actionArgList[0];
                            var balCode_Ary = 사유.Split(':');
                            var balCode = balCode_Ary[0]; //발주서번호
                            var 납기일 = balCode_Ary[1];  //납기예정일
                            var whereCode = "";
                            var whereCode1 = "";
                            var whereCode2 = "";
                            whereCode = 장소코드;
                            if (whereCode.Length == 8)
                            {
                                whereCode1 = whereCode.Substring(0, 4);
                                whereCode2 = whereCode.Substring(4, 4);
                            }

                            var now = DateTime.Now;
                            string 작업번호str = "";


                            var resp = dcDz.VL_MES_SO
                            .Where(x => x.CO_CD == Comp && x.SO_NB == balCode && x.SHIPREQ_DT == 납기일)
                            .FirstOrDefault();

                            //출고처리헤더정보 Search
                            var headresp = dc.출고처리헤더정보
                                           .Where(x => x.회사코드 == Comp && x.주문번호 == balCode)
                                           .Where_미삭제_사용()
                                           .Order_등록최신()
                                           .FirstOrDefault();

                            if (headresp == null)
                            {
                                //등록한다
                                if (resp != null)
                                {
                                    //사원코드로 부서 찾는다
                                    var sawonRec = dc.직원정보
                                        .Where(x => x.회사코드 == Comp && x.사번 == sawonCode)
                                        .FirstOrDefault();

                                    var booseo = sawonRec.부서코드;

                                    // MES
                                    var 순번_더존 = dcDz.BARPLUS_LDELIVER.Count(x => x.CO_CD == Comp) + 1;
                                    now = DateTime.Now;
                                    var yyyy = now.ToString("yyyy");
                                    string 작업번호2 = $"{"IS"}{yyyy}{순번_더존:000000}";
                                    작업번호str = 작업번호2;

                                    var 출고처리헤더정보 = new 출고처리헤더정보
                                    {
                                        #region BARPLUS_LSTOCK
                                        회사코드 = Comp,  //    회사코드
                                                      //작업번호 = "202104210628",
                                        작업번호 = 작업번호2,
                                        작업일자 = now,
                                        출고구분 = "0", //.출고구분,
                                        거래처코드 = resp.TR_CD,
                                        출고일자 = now,
                                        주문번호 = balCode,
                                        창고코드 = whereCode1,
                                        거래구분 = "0",//   거래구분
                                        환종 = "",//  환종
                                        환율 = Convert.ToDecimal(0),//    환율
                                        사원코드 = sawonCode,
                                        부서코드 = booseo,
                                        사업장코드 = "1000",
                                        과세구분 = resp.VAT_FG,
                                        단가구분 = resp.UMVAT_FG,
                                        연동구분 = "0",  //resp.연동구분,
                                        #endregion
                                    };

                                    var BARPLUS_LDELIVER = new BARPLUS_LDELIVER
                                    {
                                        CO_CD = Comp,
                                        WORK_NB = 작업번호2,
                                        WORK_DT = now.ToString("yyyyMMdd"), //String.Format("{0:yyyyMMdd}", Convert.ToDateTime(출고처리.작업일자.ToString())),
                                        ISU_FG = "0", //출고구분
                                        TR_CD = resp.TR_CD,
                                        ISU_DT = now.ToString("yyyyMMdd"), //String.Format("{0:yyyyMMdd}", Convert.ToDateTime(출고처리.출고일자.ToString())),
                                        WH_CD = whereCode1,
                                        SO_FG = "0", //resp.거래구분,
                                        EXCH_CD = resp.EXCH_CD, // .출고처리.환종,
                                        EXCH_RT = 1, //resp.환율, //출고처리.환율,
                                        EMP_CD = sawonCode,
                                        DEPT_CD = booseo,
                                        DIV_CD = "1000",  //사업장코드,
                                        VAT_FG = resp.VAT_FG,
                                        UMVAT_FG = resp.UMVAT_FG,
                                        APP_FG = "0", //headresp.연동구분,
                                    };

                                    _기준정보.MES출고처리_출고처리헤더정보_등록_PDA(출고처리헤더정보, BARPLUS_LDELIVER);

                                }
                            }
                            else
                            {
                                작업번호str = headresp.작업번호;
                            }

                            //출고처리상세정보 등록
                            var 출고처리상세정보 = new 출고처리상세정보
                            {
                                회사코드 = Comp,
                                작업번호 = 작업번호str,
                                작업순번 = 1,
                                품번 = resp.ITEM_CD, //품목코드,
                                출고수량_관리단위 = 수량, //resp.SO_QT, //출고수량_관리단위,
                                출고수량_재고단위 = 수량, //resp.SO_QT, //출고수량_재고단위,
                                주문번호 = resp.SO_NB, //주문번호,
                                주문순번 = resp.SO_SQ, //주문순번,

                                장소코드 = whereCode2,
                                연동구분 = "1",
                            };

                            var info1 = new 출고처리상세정보
                            {
                                품번 = resp.ITEM_CD, //품목코드,
                                회사코드 = Comp, //회사코드,
                                             //품목코드 = 품목코드,
                                출고수량_관리단위 = Convert.ToInt32(수량), //resp.SO_QT),
                                //장소코드 = str장소코드,
                                장소코드 = whereCode,
                                //보유년월일 = now.ToString("yyMMdd"),
                                작업순번 = 0,

                            };
                            //#endregion

                            _자재관리.출고관리보유품목입고_등록(info1, true);

                            _기준정보.MES출고처리_출고처리상세정보_등록(출고처리상세정보, true);


                            var info2 = new 보유품목정보
                            {
                                보유품목코드 = resp.ITEM_CD,
                                회사코드 = Comp,
                                품목코드 = resp.ITEM_CD,
                                수량 = Convert.ToInt32(수량),
                                장소코드 = whereCode1,
                                장소위치코드 = whereCode,
                                보유년월일 = now.ToString("yyMMdd"),
                                LOT번호 = LOT,
                                품목_LOT번호 = 품목_LOT,

                            };
                            _자재관리.보유품목LOT출고_위치등록(info2, "출고처리", "", null);



                            //2021.05.12
                            바코드품목_얻기(dc, result, actionArgsStr, Comp);

                            자재주문유형_얻기(dcDz, result, actionArgs, actionArgsStr, Comp);
                        }
                        // }}}

                        return result;
                    }

                    // 보유품목 정보
                    if (actionArgsStr != null)
                    {
                        /*
                        //보유품목_얻기(dc, result, actionArgs);
                        보유품목_얻기_반출(dc, result, actionArgsCode);
                        // 자재출고 유형 얻기
                        자재출고유형_얻기(dc, result, actionArgs);
                        */
                        //2021.05.12
                        바코드품목_얻기(dc, result, actionArgsStr, Comp);

                        자재주문유형_얻기(dcDz, result, actionArgs, actionArgsStr, Comp);
                    }
                }

                // #################################################
                // ## 이동 출고 ## 생산이동 출고 ## 외주이동 출고 ##
                // #################################################
                else if (actionCode == TagAction.A06_자재이동출고
                      || actionCode == TagAction.A06_생산이동출고
                      || actionCode == TagAction.A06_외주이동출고)
                {
                    //var 위치상세 = dc.위치상세정보
                    //    .Include(x => x.장소위치)
                    //    .ThenInclude(y => y.장소)
                    //    .FirstOrDefault(x => x.위치상세코드 == actionArgs[TagArg.A03_장소위치] && x.회사코드 == Comp);  // 장소
                    //if (위치상세 != default)
                    //{
                    //    result[$"{TagArg.A03_장소위치}.Text"] = $"{위치상세.장소위치.위치명}\n{위치상세.위치명}";
                    //    result[TagArg.A03_장소위치] = 위치상세.위치상세코드;
                    //}


                    var 위치상세 = dc.장소위치정보
                        //.Include(x => x.장소위치)
                        .Include(x => x.장소)
                        .FirstOrDefault(x => x.장소위치코드 == actionArgs[TagArg.A03_장소위치] && x.회사코드 == Comp);  // 장소
                    if (위치상세 != default)
                    {
                        result[$"{TagArg.A03_장소위치}.Text"] = $"{위치상세.장소.장소명}\n{위치상세.위치명}";
                        result[TagArg.A03_장소위치] = 위치상세.장소위치코드;
                    }

                    // 2021.05.13
                    string[] actionArgs_Ary;
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    var actionArgsCode = "";

                    int 일련번호자리수;
                    bool 일련번호_flag = false;
                    if (actionArgsStr != null)
                    {
                        actionArgs_Ary = actionArgsStr.Split(':');
                        actionArgsCode = actionArgs_Ary[0];
                        var ary_len = actionArgs_Ary.Length;
                        if (ary_len > 2)
                        {
                            일련번호자리수 = actionArgs_Ary[2].Length;
                            if (일련번호자리수 == 5)
                                일련번호_flag = true;
                        }
                    }


                    if (isComplete == true)
                    {
                        SaveLogComp(Comp);

                        string 이동유형 = "5";
                        string 사유 = null;
                        string 이동형태 = "";

                        if (actionCode == TagAction.A06_자재이동출고)
                        {
                            이동유형 = "5";
                            이동형태 = "자재이동출고";
                            사유 = null;
                        }
                        else if (actionCode == TagAction.A06_생산이동출고)
                        {
                            이동유형 = "0";
                            이동형태 = "생산이동출고";
                            사유 = actionArgList[4];  //생산 작업지서 번호 
                        }
                        else if (actionCode == TagAction.A06_외주이동출고)
                        {
                            이동유형 = "1";
                            이동형태 = "외주이동출고";
                            사유 = actionArgList[4];  //외주 작업지서 번호 
                        }

                        var 품번_LOT = actionArgs[TagArg.A04_보유품목];
                        //위치상세코드
                        var 위치상세코드 = actionArgs[TagArg.A03_장소위치];
                        var 장소위치코드 = 위치상세코드.Substring(0, 8);
                        decimal D수량 = decimal.Parse(actionArgList[3]);

                        var sawonCode = actionArgList[0];


                        var actArgs_Ary = 품번_LOT.Split(':');
                        var actArgsCode = actArgs_Ary[0];
                        var actLotCode = actArgs_Ary[1];



                        var 입고보유품목 = dc.보유품목위치정보
                                                    .Include(x => x.보유품목)
                                                    .ThenInclude(y => y.품목)
                                                    .Where(x => x.보유품목코드 == actArgsCode && x.장소위치코드 == 장소위치코드 && x.품목_LOT번호 == 품번_LOT)
                                                    .FirstOrDefault();



                        var info1 = new 입고처리상세정보
                        {
                            품번 = 입고보유품목.보유품목코드, //품목코드,
                            회사코드 = Comp, //회사코드,
                            //품목코드 = 품목코드,
                            입고수량_관리단위 = Convert.ToInt32(D수량), //Convert.ToInt32(resp.PO_QT),
                            //장소코드 = str장소코드,
                            입고장소코드 = 입고보유품목.장소위치코드,
                            //보유년월일 = now.ToString("yyMMdd"),
                            작업순번 = 0,
                            LOT번호 = actLotCode,
                            비고 = 품번_LOT,
                        };

                        _자재관리.자재이동_임시등록(info1, 이동형태, 이동유형, 위치상세코드, 사유);

                        ///////////////////////////////////////////////////////////////////////////
                        // 2021.06.02
                        // 외주이동출고시 입고처리를 동시에 진행

                        if (actionCode == TagAction.A06_외주이동출고 || actionCode == TagAction.A06_생산이동출고)
                        {
                            string WO_NB = null;
                            if (사유 != null)
                            {
                                var 사유_Ary = 사유.Split(':');
                                WO_NB = 사유_Ary[0];
                            }
                            var 입고품목 = dc.보유품목임시위치정보
                                             .Where(x => x.보유품목코드 == 입고보유품목.보유품목코드 && x.LOT번호 == actLotCode && x.품목_LOT번호 == 품번_LOT)
                                             .FirstOrDefault();
                            string 외주장소위치 = "";
                            if (actionCode == TagAction.A06_외주이동출고)
                            {
                                이동유형 = "1";
                                이동형태 = "외주이동입고";
                                //외주 발주서로 부터 위치상세코드 가져오기
                                var 외주발주서 = dcDz.VL_MES_WO_WF
                                                 .Where(x => x.CO_CD == Comp && x.WO_CD == WO_NB).FirstOrDefault();
                                외주장소위치 = 외주발주서.BASELOC_CD + 외주발주서.LOC_CD;
                            }
                            else
                            {
                                이동유형 = "0";
                                이동형태 = "생산이동입고";
                                //var 외주발주서 = dcDz.VL_MES_WO_WF
                                //                 .Where(x => x.CO_CD == Comp && x.WO_CD == WO_NB).FirstOrDefault();
                                외주장소위치 = "20002001";  //생산 / 가공
                            }


                            _자재관리.자재이동_임시등록_해제(입고품목, 이동형태, 이동유형, 외주장소위치, sawonCode);
                        }

                        ///////////////////////////////////////////////////////////////////////////


                        // 2021.05.12
                        바코드입고품목_얻기(dc, result, actionArgs, Comp, true);

                        return result;

                    }

                    // 보유품목 정보
                    if (actionArgsStr != null)
                    {

                        //2021.05.12
                        //바코드품목_얻기(dc, result, actionArgsStr, Comp);
                        바코드입고품목_얻기(dc, result, actionArgs, Comp, true);

                        if (actionCode == TagAction.A06_생산이동출고)
                        {
                            생산이동유형_얻기(dcDz, result, actionArgs, actionArgsStr, Comp);
                        }
                        else if (actionCode == TagAction.A06_외주이동출고)
                        {
                            외주이동유형_얻기(dcDz, result, actionArgs, actionArgsStr, Comp);
                        }

                    }

                }
                // ###################################################
                // ## 자재이동입고 ## 생산이동입고 ## 외주이동 입고 ##
                // ###################################################
                else if (actionCode == TagAction.A06_자재이동입고
                        || actionCode == TagAction.A06_생산이동입고
                        || actionCode == TagAction.A06_외주이동입고)
                {
                    //var 위치상세 = dc.위치상세정보
                    //    .Include(x => x.장소위치)
                    //    .ThenInclude(y => y.장소)
                    //    .FirstOrDefault(x => x.위치상세코드 == actionArgs[TagArg.A03_장소위치] && x.회사코드 == Comp);  // 장소
                    //if (위치상세 != default)
                    //{
                    //    result[$"{TagArg.A03_장소위치}.Text"] = $"{위치상세.장소위치.위치명}\n{위치상세.위치명}";
                    //    result[TagArg.A03_장소위치] = 위치상세.위치상세코드;
                    //}

                    var 위치상세 = dc.장소위치정보
                        //.Include(x => x.장소위치)
                        .Include(x => x.장소)
                        .FirstOrDefault(x => x.장소위치코드 == actionArgs[TagArg.A03_장소위치] && x.회사코드 == Comp);  // 장소
                    if (위치상세 != default)
                    {
                        result[$"{TagArg.A03_장소위치}.Text"] = $"{위치상세.장소.장소명}\n{위치상세.위치명}";
                        result[TagArg.A03_장소위치] = 위치상세.장소위치코드;
                    }

                    // 2021.05.13
                    string[] actionArgs_Ary;
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    var actionArgsCode = "";

                    int 일련번호자리수;
                    bool 일련번호_flag = false;
                    if (actionArgsStr != null)
                    {
                        actionArgs_Ary = actionArgsStr.Split(':');
                        actionArgsCode = actionArgs_Ary[0];
                        var ary_len = actionArgs_Ary.Length;
                        if (ary_len > 2)
                        {
                            일련번호자리수 = actionArgs_Ary[2].Length;
                            if (일련번호자리수 == 5)
                                일련번호_flag = true;
                        }
                    }


                    if (isComplete == true)
                    {
                        SaveLogComp(Comp);

                        string 이동유형 = "5";
                        string 사유 = null;
                        string 이동형태 = "";

                        if (actionCode == TagAction.A06_자재이동입고)
                        {
                            이동유형 = "5";
                            이동형태 = "자재이동입고";
                            사유 = null;
                        }
                        else if (actionCode == TagAction.A06_생산이동입고)
                        {
                            이동유형 = "0";
                            이동형태 = "생산이동입고";
                            //사유 = actionArgList[4];  //주문서 번호 
                        }
                        else if (actionCode == TagAction.A06_외주이동입고)
                        {
                            이동유형 = "1";
                            이동형태 = "외주이동입고";
                            //사유 = actionArgList[4];  //외주 작업지서 번호 
                        }

                        var sawonCode = actionArgList[0];
                        var 품번_LOT = actionArgs[TagArg.A04_보유품목];
                        //위치상세코드
                        var 위치상세코드 = actionArgs[TagArg.A03_장소위치];
                        var 장소위치코드 = 위치상세코드.Substring(0, 8);
                        //decimal D수량 = decimal.Parse(actionArgList[3]);

                        var actArgs_Ary = 품번_LOT.Split(':');
                        var actArgsCode = actArgs_Ary[0];
                        var actLotCode = actArgs_Ary[1];


                        var 입고보유품목 = dc.보유품목임시위치정보
                                             .Where(x => x.보유품목코드 == actArgsCode && x.LOT번호 == actLotCode && x.품목_LOT번호 == 품번_LOT)
                                             .FirstOrDefault();


                        if (입고보유품목 != default)
                        {
                            _자재관리.자재이동_임시등록_해제(입고보유품목, 이동형태, 이동유형, 위치상세코드, sawonCode);
                        }

                        // 2021.05.12
                        바코드입고품목_얻기(dc, result, actionArgs, Comp, true);

                        return result;

                    }

                    // 보유품목 정보
                    if (actionArgsStr != null)
                    {
                        //2021.05.12
                        바코드품목_얻기_위치(dc, result, actionArgs, actionArgsStr, Comp);
                    }

                }
                // ###############
                // ## 발주입고 ##
                // ###############
                else if (actionCode == TagAction.A06_발주입고)
                {
                    // 2021.04.14
                    // 0: S9201, SYSTEM  1: S9204, 품목코드  2: S9203, <장소위치>  3:S9218, <유형> 

                    string[] actionArgs_Ary;
                    var actionAct = actionArgList[0];
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    //발주코드 : 순번 : 회사코드 - 분리한다
                    var balCode = "";
                    var sunBun = "";
                    var compCode = "";
                    var actionArgsCode = actionArgsStr;

                    //2021 04 29
                    // 선택한 장소 코드
                    var where_Code = actionArgList[2];

                    var whereCode = "";
                    var whereCode1 = "";
                    var whereCode2 = "";

                    if (where_Code == "S922001")
                    {
                        whereCode = "10001000";
                    }
                    else if (where_Code == "S922002")
                    {
                        whereCode = "10001001";
                    }
                    else if (where_Code == "S922003")
                    {
                        whereCode = "10001002";
                    }
                    else if (where_Code == "S922004")
                    {
                        whereCode = "10001009";
                    }
                    else if (where_Code == "S922005")
                    {
                        whereCode = "20002000";
                    }
                    else if (where_Code == "S922006")
                    {
                        whereCode = "30003001";
                    }
                    else if (where_Code == "S922007")
                    {
                        whereCode = "30003002";
                    }
                    else if (where_Code == "S922008")
                    {
                        whereCode = "30003003";
                    }
                    if (whereCode.Length == 8)
                    {
                        whereCode1 = whereCode.Substring(0, 4);
                        whereCode2 = whereCode.Substring(4, 4);
                    }

                    var sawonCode = actionArgList[0];

                    if (actionArgsStr != null)
                    {
                        actionArgs_Ary = actionArgsStr.Split(':');
                        balCode = actionArgs_Ary[0];
                        sunBun = actionArgs_Ary[1];
                        compCode = actionArgs_Ary[2];
                        var 순번 = Convert.ToDecimal(sunBun);


                    }

                    // 2021.04.15
                    //var actionArgsNum = actionArgs[TagArg.A05_수량];
                    //int 일련번호자리수;
                    //bool 일련번호_flag = false;
                    //if (actionArgsStr != null)
                    //{
                    //    actionArgs_Ary = actionArgs[TagArg.A18_자재출고유형]; //액션인자 출고유형으로 수량이 넘어옴
                    //    //actionArgsCode = actionArgs_Ary[0];
                    //    일련번호자리수 = actionArgs_Ary.Length;
                    //    if (일련번호자리수 == 5)
                    //        일련번호_flag = true;
                    //}
                    //////////////////////////////////////

                    if (isComplete == true)
                    {
                        SaveLogComp(compCode);

                        // Complete시에 수행한다
                        var now = DateTime.Now;
                        var 순번 = Convert.ToDecimal(sunBun);

                        //var resp = dc.발주서정보
                        //      .Where(x => x.회사코드 == compCode && x.발주번호 == balCode && x.발주순번 == 순번)
                        //      .Where_미삭제_사용()
                        //      .Order_등록최신()
                        //      .FirstOrDefault();

                        var resp = dcDz.VL_MES_PO
                             .Where(x => x.CO_CD == compCode && x.PO_NB == balCode && x.PO_SQ == 순번)
                             .FirstOrDefault();



                        //    // 입고처리헤더정보 Search
                        var headresp = dc.입고처리헤더정보
                                       .Where(x => x.회사코드 == compCode && x.발주번호 == balCode)
                                       .Where_미삭제_사용()
                                       .Order_등록최신()
                                       .FirstOrDefault();
                        if (headresp == null)
                        {
                            //등록한다
                            if (resp != null)
                            {
                                //사원코드로 부서 찾는다
                                var sawonRec = dc.직원정보
                                    .Where(x => x.회사코드 == compCode && x.사번 == sawonCode)
                                    .FirstOrDefault();

                                var booseo = sawonRec.부서코드;

                                // MES
                                var 입고처리헤더정보 = new 입고처리헤더정보
                                {
                                    #region BARPLUS_LSTOCK
                                    회사코드 = compCode,  //	회사코드
                                                      //작업번호 = "202104210628",
                                    작업일자 = now, //	작업일자
                                    입고구분 = "0",                                    //	입고구분
                                    거래처코드 = resp.TR_CD,        //	거래처코드
                                    입고일자 = now,   //	입고일자
                                    입고창고 = whereCode1,//장소위치정보selected != null ? 장소위치정보selected.장소코드 : null,          //	입고창고
                                    입고장소 = whereCode2,//장소위치정보selected != null ? 장소위치정보selected.위치코드 : null,         //	입고장소
                                    발주번호 = balCode, //발주서헤더정보selected != null ? 발주서헤더정보selected.발주번호 : null,  //	발주번호
                                    거래구분 = resp.PO_FG,//	거래구분
                                    환종 = "",//	환종
                                    환율 = Convert.ToDecimal(0),//	환율
                                    LC여부 = "",//	LC여부
                                    사원코드 = resp.PLN_CD,//"1001",//직원정보selected != null ? 직원정보selected.사번 : null,  //	사원코드
                                    부서코드 = booseo, //부서정보selected != null ? 부서정보selected.부서코드 : null,  //	부서코드
                                    사업장코드 = "1000", //사업장코드, //	사업장코드
                                    프로젝트코드 = "",//	프로젝트코드
                                    과세구분 = resp.VAT_FG, //과세구분
                                    작업구분 = "", //	작업구분
                                    관리구분코드 = "", //	관리구분코드
                                    EXCST_NB = "", //
                                    배부여부 = "", //	배부여부
                                    비고 = "", //	비고
                                    최초입력사원코드 = "", //	최초입력사원코드
                                    최초입력일 = null,   //	최초입력일
                                    최초입력IP = "", //	최초입력IP
                                    수정사원코드 = "", //	수정사원코드
                                    수정일 = null,   //	수정일
                                    수정IP = "", //	수정IP
                                    DUMMY1 = "",//
                                    DUMMY2 = "",    //
                                    DUMMY3 = "",//
                                    PLN_CD = "",    //
                                    SO_NB3 = "",    //
                                    UMVAT_FG = "0",     //
                                    APP_FG = "0",  //

                                    #endregion
                                };

                                //ERP  BARPLUS_LSTOCK
                                var BARPLUS_LSTOCK = new BARPLUS_LSTOCK
                                {
                                    #region BARPLUS_LSTOCK

                                    CO_CD = compCode,  //	회사코드
                                                       //WORK_NB = "202104210628",       //	작업번호
                                    WORK_DT = now.ToString("yyyyMMdd"),  //	작업일자
                                    RCV_FG = "0",                                    //	입고구분
                                    TR_CD = resp.TR_CD, //거래처정보selected != null ? 거래처정보selected.거래처코드 : null,          //	거래처코드
                                    RCV_DT = now.ToString("yyyyMMdd"),   //	입고일자
                                    WH_CD = whereCode1,//장소위치정보selected != null ? 장소위치정보selected.장소코드 : null,         //	입고창고
                                    LC_CD = whereCode2,//장소위치정보selected != null ? 장소위치정보selected.위치코드 : null,          //	입고장소
                                    PO_NB = balCode,//발주서헤더정보selected != null ? 발주서헤더정보selected.발주번호 : null, //	발주번호
                                    PO_FG = resp.PO_FG,//	거래구분
                                    EXCH_CD = "",//	환종
                                    EXCH_RT = Convert.ToDecimal(0),//	환율
                                    LC_YN = "",//	LC여부

                                    EMP_CD = sawonCode,//직원정보selected != null ? 직원정보selected.사번 : null,  //	사원코드
                                    DEPT_CD = booseo, //부서정보selected != null ? 부서정보selected.부서코드 : null,  //	부서코드
                                    DIV_CD = "1000", //	사업장코드
                                    PJT_CD = resp.PJT_CD,//	프로젝트코드
                                    VAT_FG = resp.VAT_FG, //	과세구분
                                    MAP_FG = "", //	작업구분
                                    MGMT_CD = "", //	관리구분코드
                                    EXCST_NB = "", //
                                    DIST_YN = "", //	배부여부
                                    REMARK_DC = "", //	비고
                                    INSERT_ID = "", //	최초입력사원코드
                                    INSERT_DT = null,   //	최초입력일
                                    INSERT_IP = "", //	최초입력IP
                                    MODIFY_ID = "", //	수정사원코드
                                    MODIFY_DT = null,   //	수정일
                                    MODIFY_IP = "", //	수정IP
                                    DUMMY1 = "",//
                                    DUMMY2 = "",    //
                                    DUMMY3 = "",//
                                    PLN_CD = "",    //
                                    SO_NB3 = "",    //
                                    UMVAT_FG = "0",     //
                                    APP_FG = "0",  //

                                    #endregion

                                };

                                _기준정보.MES입고처리_입고처리헤더정보_등록(입고처리헤더정보, BARPLUS_LSTOCK);

                            }
                        }

                        Task.Delay(300);
                        headresp = dc.입고처리헤더정보
                                       .Where(x => x.회사코드 == compCode && x.발주번호 == balCode)
                                       .Where_미삭제_사용()
                                       .Order_등록최신()
                                       .FirstOrDefault();
                        //입고처리상세정보 등록
                        var 입고처리상세정보 = new 입고처리상세정보
                        {
                            회사코드 = compCode,
                            작업번호 = headresp.작업번호,
                            //작업순번  = 1,
                            품번 = resp.ITEM_CD, //품목코드,
                            입고수량_관리단위 = resp.PO_QT, //입고수량_관리단위,
                            입고수량_재고단위 = resp.PO_QT, //입고수량_재고단위,
                            //입고단가
                            //공급가
                            //부가세
                            //합계액
                            //환종
                            //환율
                            //외화단가
                            //외화금액
                            //LOT번호
                            발주번호 = resp.PO_NB, //발주번호,
                            발주순번 = resp.PO_SQ, //발주순번,

                            선적번호 = "",
                            선적순번 = 1,
                            //사용여부
                            //유효여부
                            //단가구분
                            입고장소코드 = whereCode2, //장소위치정보selected != null ? 장소위치정보selected.위치코드 : null,
                            비고 = "",

                        };

                        _기준정보.MES입고처리_입고처리상세정보_등록(입고처리상세정보);

                        var BARPLUS_LSTOCK_D = new BARPLUS_LSTOCK_D
                        {
                            CO_CD = compCode,  //	회사코드	,
                            WORK_NB = headresp.작업번호,          //	작업번호	,
                                                              //WORK_SQ = 1,    //	작업순번	,
                            ITEM_CD = resp.ITEM_CD,
                            PO_QT = resp.PO_QT, //입고수량_관리단위,  //	입고수량(관리단위)	,
                            RCV_QT = resp.PO_QT, //입고수량_재고단위,//	입고수량(재고단위)	,
                                                 //RCV_UM		= //	입고단가	,
                                                 //RCVG_AM		//	공급가	,
                                                 //RCVV_AM		//	부가세	,
                                                 //RCVH_AM		//	합계액	,
                                                 //EXCH_CD		//	환종	,
                                                 //EXCH_RT		//	환율	,
                                                 //EXCH_UM		//	외화단가	,
                                                 //EXCH_AM		//	외화금액	,
                                                 //LOT_NB		//	LOT번호	,

                            PO_NB = resp.PO_NB,
                            PO_SQ = resp.PO_SQ,
                            //REQ_NB		//	입고의뢰번호	,
                            //REQ_SQ		//	입고의뢰순번	,
                            //IBL_NB		//	선적번호	,
                            //IBL_SQ		//	선적순번	,
                            USE_YN = "0",//	사용여부	,
                            EXPIRE_YN = "Y",  //	유효여부	,
                                              //UM_FG		//	단가구분	,
                            CONF_NB3 = 0,
                            LC_CD = whereCode2, //장소위치정보selected != null ? 장소위치정보selected.위치코드 : "", //	입고장소코드	,
                            REMARK_DC = "",                         //	비고	,
                            APP_FG = "0",
                        };

                        _기준정보.더존입고상세_BARPLUS_LSTOCK_D(BARPLUS_LSTOCK_D);

                        #region  
                        string str장소코드 = whereCode1; // 장소위치정보selected != null ? 장소위치정보selected.장소코드 : 창고코드;
                        string str위치코드 = whereCode2; // 장소위치정보selected != null ? 장소위치정보selected.위치코드 : null;
                        string str장소위치코드 = $"{str장소코드}{str위치코드}";


                        var info1 = new 입고처리상세정보
                        {
                            품번 = resp.ITEM_CD, //품목코드,
                            회사코드 = compCode, //회사코드,
                            //품목코드 = 품목코드,
                            입고수량_관리단위 = Convert.ToInt32(resp.PO_QT),
                            //장소코드 = str장소코드,
                            입고장소코드 = str장소위치코드,
                            //보유년월일 = now.ToString("yyMMdd"),
                            작업순번 = 0,

                        };
                        #endregion

                        _자재관리.입고관리보유품목입고_등록(info1, true);

                        #region
                        string str거래구분 = string.Empty;
                        var _거래 = resp.PO_FG;
                        if (_거래 == "0")
                            str거래구분 = "DOMESTIC";
                        else if (_거래 == "1")
                            str거래구분 = "LOCAL L / C ";
                        else if (_거래 == "2")
                            str거래구분 = "구매승인서";
                        else if (_거래 == "3")
                            str거래구분 = "MASTER L / C";
                        else if (_거래 == "4")
                            str거래구분 = "T / T";
                        else if (_거래 == "5" || _거래 == "6")
                            str거래구분 = "기타";
                        #endregion
                        _자재관리.보유품목입고_위치등록(info1, "입고처리", str거래구분, "", true);

                        return result;
                    }


                    // 보유품목 정보
                    if (actionArgsStr != null)
                    {
                        //보유품목_얻기(dc, result, actionArgs);
                        보유품목_얻기_발주(dc, dcDz, result, actionArgsCode);
                    }

                    // 자재출고 유형 얻기
                    장소위치_얻기(dc, result, actionArgs);
                }
                // ###############
                // ## 주문출고 ##
                // ###############
                else if (actionCode == TagAction.A06_주문출고)
                {
                    // 2021.04.14
                    // 0: S9201, SYSTEM  1: S9204, 품목코드  2: S9203, <장소위치>  3:S9218, <유형> 

                    string[] actionArgs_Ary;
                    var actionAct = actionArgList[0];
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    //주문번호 : 순번 : 회사코드 - 분리한다
                    var balCode = "";
                    var sunBun = "";
                    var compCode = "";
                    var actionArgsCode = actionArgsStr;

                    //2021 04 29
                    // 선택한 장소 코드
                    var where_Code = actionArgList[2];

                    var whereCode = "";
                    var whereCode1 = "";
                    var whereCode2 = "";
                    if (where_Code == "S922001")
                    {
                        whereCode = "10001000";
                    }
                    else if (where_Code == "S922002")
                    {
                        whereCode = "10001001";
                    }
                    else if (where_Code == "S922003")
                    {
                        whereCode = "10001002";
                    }
                    else if (where_Code == "S922004")
                    {
                        whereCode = "10001009";
                    }
                    else if (where_Code == "S922005")
                    {
                        whereCode = "20002000";
                    }
                    else if (where_Code == "S922006")
                    {
                        whereCode = "30003001";
                    }
                    else if (where_Code == "S922007")
                    {
                        whereCode = "30003002";
                    }
                    else if (where_Code == "S922008")
                    {
                        whereCode = "30003003";
                    }

                    if (whereCode.Length == 8)
                    {
                        whereCode1 = whereCode.Substring(0, 4);
                        whereCode2 = whereCode.Substring(4, 4);
                    }
                    var sawonCode = actionArgList[0];

                    if (actionArgsStr != null)
                    {
                        actionArgs_Ary = actionArgsStr.Split(':');
                        balCode = actionArgs_Ary[0];
                        sunBun = actionArgs_Ary[1];
                        compCode = actionArgs_Ary[2];
                        var 순번 = Convert.ToDecimal(sunBun);
                    }

                    if (isComplete == true)
                    {
                        SaveLogComp(compCode);

                        // Complete시에 수행한다
                        var now = DateTime.Now;
                        var 순번 = Convert.ToDecimal(sunBun);
                        string 작업번호str = "";

                        //var resp = dc.주문서정보
                        //      .Where(x => x.회사코드 == compCode && x.주문번호 == balCode && x.순번 == 순번)
                        //      .Where_미삭제_사용()
                        //      .Order_등록최신()
                        //      .FirstOrDefault();

                        var resp = dcDz.VL_MES_SO
                            .Where(x => x.CO_CD == compCode && x.SO_NB == balCode && x.SO_SQ == 순번)
                            .FirstOrDefault();

                        //출고처리헤더정보 Search
                        var headresp = dc.출고처리헤더정보
                                       .Where(x => x.회사코드 == compCode && x.주문번호 == balCode)
                                       .Where_미삭제_사용()
                                       .Order_등록최신()
                                       .FirstOrDefault();
                        if (headresp == null)
                        {
                            //등록한다
                            if (resp != null)
                            {
                                //사원코드로 부서 찾는다
                                var sawonRec = dc.직원정보
                                    .Where(x => x.회사코드 == compCode && x.사번 == sawonCode)
                                    .FirstOrDefault();

                                var booseo = sawonRec.부서코드;

                                // MES
                                var 순번_더존 = dcDz.BARPLUS_LDELIVER.Count(x => x.CO_CD == compCode) + 1;
                                now = DateTime.Now;
                                var yyyy = now.ToString("yyyy");
                                string 작업번호2 = $"{"IS"}{yyyy}{순번_더존:000000}";
                                작업번호str = 작업번호2;

                                var 출고처리헤더정보 = new 출고처리헤더정보
                                {
                                    #region BARPLUS_LSTOCK
                                    회사코드 = compCode,  //	회사코드
                                                      //작업번호 = "202104210628",
                                    작업번호 = 작업번호2,
                                    작업일자 = now,
                                    출고구분 = "0", //.출고구분,
                                    거래처코드 = resp.TR_CD,
                                    출고일자 = now,
                                    주문번호 = balCode,
                                    창고코드 = whereCode1,
                                    거래구분 = "0",//	거래구분
                                    환종 = "",//	환종
                                    환율 = Convert.ToDecimal(0),//	환율
                                    사원코드 = sawonCode,
                                    부서코드 = booseo,
                                    사업장코드 = "1000",
                                    과세구분 = resp.VAT_FG,
                                    단가구분 = resp.UMVAT_FG,
                                    연동구분 = "0",  //resp.연동구분,
                                    #endregion
                                };

                                var BARPLUS_LDELIVER = new BARPLUS_LDELIVER
                                {
                                    CO_CD = compCode,
                                    WORK_NB = 작업번호2,
                                    WORK_DT = now.ToString("yyyyMMdd"), //String.Format("{0:yyyyMMdd}", Convert.ToDateTime(출고처리.작업일자.ToString())),
                                    ISU_FG = "0", //출고구분
                                    TR_CD = resp.TR_CD,
                                    ISU_DT = now.ToString("yyyyMMdd"), //String.Format("{0:yyyyMMdd}", Convert.ToDateTime(출고처리.출고일자.ToString())),
                                    WH_CD = whereCode1,
                                    SO_FG = "0", //resp.거래구분,
                                    EXCH_CD = resp.EXCH_CD, // .출고처리.환종,
                                    EXCH_RT = 1, //resp.환율, //출고처리.환율,
                                    EMP_CD = sawonCode,
                                    DEPT_CD = booseo,
                                    DIV_CD = "1000",  //사업장코드,
                                    VAT_FG = resp.VAT_FG,
                                    UMVAT_FG = resp.UMVAT_FG,
                                    APP_FG = "0", //headresp.연동구분,
                                };

                                _기준정보.MES출고처리_출고처리헤더정보_등록_PDA(출고처리헤더정보, BARPLUS_LDELIVER);

                            }
                        }
                        else
                        {
                            작업번호str = headresp.작업번호;
                        }


                        //출고처리상세정보 등록
                        var 출고처리상세정보 = new 출고처리상세정보
                        {
                            회사코드 = compCode,
                            작업번호 = 작업번호str,
                            작업순번 = 1,
                            품번 = resp.ITEM_CD, //품목코드,
                            출고수량_관리단위 = resp.SO_QT, //출고수량_관리단위,
                            출고수량_재고단위 = resp.SO_QT, //출고수량_재고단위,
                            주문번호 = resp.SO_NB, //주문번호,
                            주문순번 = resp.SO_SQ, //주문순번,

                            장소코드 = whereCode2,
                            연동구분 = "1",
                        };

                        var info1 = new 출고처리상세정보
                        {
                            품번 = resp.ITEM_CD, //품목코드,
                            회사코드 = compCode, //회사코드,
                            //품목코드 = 품목코드,
                            출고수량_관리단위 = Convert.ToInt32(resp.SO_QT),
                            //장소코드 = str장소코드,
                            장소코드 = whereCode,
                            //보유년월일 = now.ToString("yyMMdd"),
                            작업순번 = 0,

                        };
                        //#endregion

                        _자재관리.출고관리보유품목입고_등록(info1, true);

                        _기준정보.MES출고처리_출고처리상세정보_등록(출고처리상세정보, true);

                        var info = new 보유품목정보
                        {
                            보유품목코드 = resp.ITEM_CD,
                            회사코드 = compCode,
                            품목코드 = resp.ITEM_CD,
                            수량 = Convert.ToInt32(resp.SO_QT),
                            장소코드 = whereCode1,
                            장소위치코드 = whereCode,
                            보유년월일 = now.ToString("yyMMdd"),

                        };
                        _자재관리.보유품목출고_위치등록(info, "출고처리", "");

                        return result;
                    }


                    // 보유품목 정보
                    if (actionArgsStr != null)
                    {
                        //보유품목_얻기(dc, result, actionArgs);
                        보유품목_얻기_주문(dc, dcDz, result, actionArgsCode);
                    }

                    // 자재출고 유형 얻기
                    장소위치_얻기(dc, result, actionArgs);
                }
                // ###############
                // ## 품목 출고 ##
                // ###############
                else if (actionCode == TagAction.A06_품목출고)
                {
                    // var 장소위치 = dc.장소위치정보
                    //    .Include(x => x.장소)
                    //    .FirstOrDefault(x => x.장소위치코드 == actionArgs[TagArg.A03_장소위치]);  // 장소
                    //if (장소위치 != default)
                    //{
                    //    result[$"{TagArg.A03_장소위치}.Text"] = $"{장소위치.장소.장소명}\n{장소위치.위치명}";
                    //    result[TagArg.A03_장소위치] = 장소위치.장소위치코드;
                    //    //result[$"{TagArg.A02_장소}.Text"] = 장소위치.장소코드.장소명;
                    //    result[TagArg.A02_장소] = 장소위치.장소코드;
                    //    var 장소 = dc.장소정보
                    //    .FirstOrDefault(x => x.장소코드 == 장소위치.장소코드);  // 장소
                    //    if (장소 != default)
                    //    {
                    //        result[$"{TagArg.A02_장소}.Text"] = 장소.장소명;
                    //    }
                    //}

                    // 2021.04.14
                    // 0: S9201, SYSTEM  1: S9204, 품목코드  2: S9205, <수량>  3:S9218, <유형> 

                    string[] actionArgs_Ary;
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    var actionArgsCode = actionArgsStr;

                    int 일련번호자리수;
                    bool 일련번호_flag = false;

                    // 2021.04.22 연속된 일련번호 체크
                    bool Cont_flag = false;
                    bool Cont_go = true;
                    품목일련정보 품목일련Data = null;
                    //품목일련정보List 품목일련List = null;

                    if (actionArgsStr != null)
                    {
                        actionArgs_Ary = actionArgsStr.Split(':');
                        actionArgsCode = actionArgs_Ary[0]; //품목코드
                        일련번호자리수 = actionArgs_Ary[2].Length;

                        /*//
                        //###################################### 
                        // 일련번호를 가진 품목코드가 연속으로 온다는
                        // 가정하에 Temp에 user와 품목코드 보관
                        품목일련Data = new 품목일련정보
                        {
                            사번 = actionArgs[TagArg.A01_작업자],
                            품목코드 = actionArgs[TagArg.A04_보유품목]
                        };
                        //품목코드 중복인 것 제외 루틴 필요
                        var res = from p in _자재관리.품목일련정보List
                                    where (p.품목코드 == 품목일련Data.품목코드 && p.사번 == 품목일련Data.품목코드)
                                    select p;
                        if(res != null)
                        {
                            Cont_flag = true;
                        }
                        
                        //########################################
                        //*/

                        if (일련번호자리수 == 5)
                        {
                            일련번호_flag = true;

                            /*//
                            //###################################### 
                            // 일련번호를 가진 품목코드가 연속으로 온다는
                            // 가정하에 Temp에 user와 품목코드 보관
                            품목일련Data = new 품목일련정보
                            {
                                사번 = actionArgs[TagArg.A01_작업자],
                                품목코드 = actionArgs[TagArg.A04_보유품목]
                            };
                            //품목코드 중복인 것 제외 루틴 필요
                            var res = from p in _자재관리.품목일련정보List
                                      where (p.품목코드 == 품목일련Data.품목코드 && p.사번 == 품목일련Data.품목코드)
                                      select p;
                            if(res == null)
                            {
                                _자재관리.품목일련정보List.Add(품목일련Data);
                            }
                            else
                            {
                                cp.GetSubject<(string, string)>("global.message.qr").OnNext(("DupleScan", 사번));
                                // 이미 바구니에 담긴 Data 메세지를 PDA로 보낸다 
                                Cont_go = false;
                            }
                            //########################################
                            //*/

                        }
                        /*//
                        else
                        {
                            if (Cont_flag)
                            {
                                cp.GetSubject<(string, string)>("global.message.qr").OnNext(("DupleScan", 사번));
                                Cont_go = false;
                            }  
                        }
                        //*/
                    }

                    // 2021.04.15
                    var actionArgsNum = actionArgs[TagArg.A05_수량];
                    //int 일련번호자리수;
                    //bool 일련번호_flag = false;
                    //if (actionArgsStr != null)
                    //{
                    //    actionArgs_Ary = actionArgs[TagArg.A18_자재출고유형]; //액션인자 출고유형으로 수량이 넘어옴
                    //    //actionArgsCode = actionArgs_Ary[0];
                    //    일련번호자리수 = actionArgs_Ary.Length;
                    //    if (일련번호자리수 == 5)
                    //        일련번호_flag = true;
                    //}
                    //////////////////////////////////////

                    if (isComplete == true)
                    {
                        SaveLog();

                        ///////////////////////////////////////////////////////////
                        ///  연속된 일련번호 Data 어떻게 오는지 확인 필요
                        ///////////////////////////////////////////////////////////

                        // 보유품목정보에 반영 {{{
                        // actionArgList[ 작업자, 장소, 보유품목, 수량 ]
                        //var info = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == actionArgList[2]); // 보유품목 선택
                        var info = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == actionArgsCode); // 보유품목 선택
                        //var 장소위치정보 = dc.장소위치정보.Include(x => x.장소).FirstOrDefault(x => x.장소위치코드 == actionArgList[1]);
                        if (info != default)
                        {


                            //var 장소코드 = 장소위치.장소코드;
                            var 보유품목코드 = actionArgList[2];

                            var 수량 = 1;
                            if (!일련번호_flag)
                            {
                                var act_Ary = actionArgList[2].Split(':');
                                //수량 = Convert.ToInt32(act_Ary[2]);

                                수량 = Convert.ToInt32(actionArgsNum);
                            }
                            //var 수량 = decimal.Parse(actionArgList[2]);
                            var 사유 = actionArgList[3];

                            // 2021.04.15 이동출고시 보유품목 재고 변경사항 없다
                            //if (사유 != "S921802") // 이동출고 
                            _자재관리.보유품목_품목출고(actionArgsCode, 수량, 사유);

                            if (사유 != "S921802") //이동출고")
                                _자재관리.자재관리_보유품목일련번호생성_Update(actionArgsStr, 수량, null, null, 사유);  //(보유품목코드, 수량, 장소코드, null, 사유)

                            //장소_얻기(dc, result, actionArgs);
                            // 보유품목 정보
                            //보유품목_얻기(dc, result, actionArgs);
                            보유품목_얻기_반출(dc, result, actionArgsCode);

                            // 자재출고 유형 얻기
                            자재출고유형_얻기(dc, result, actionArgs);
                        }
                        // }}}

                        return result;
                    }

                    /*//
                    if (Cont_go)
                    {
                        // 보유품목 정보
                        if (actionArgsStr != null)
                        {
                            // 연속된 일련 데이타일 경우 누적 수량 표기 해야한다
                            // su를 구함
                            var res = from p in _자재관리.품목일련정보List
                                    where (p.품목코드 == 품목일련Data.품목코드 && p.사번 == 품목일련Data.품목코드)
                                     select p;
                            if(res != null)
                            {
                                su = res.Count;
                                보유품목_연속_반출(dc, result, actionArgsCode, su);
                            }
                            else {
                                 //보유품목_얻기(dc, result, actionArgs);
                                보유품목_얻기_반출(dc, result, actionArgsCode);
                            }
                        }

                        // 자재출고 유형 얻기
                        자재출고유형_얻기(dc, result, actionArgs);
                    }
                    //*/
                    // 보유품목 정보
                    if (actionArgsStr != null)
                    {
                        //보유품목_얻기(dc, result, actionArgs);
                        보유품목_얻기_반출(dc, result, actionArgsCode);
                    }

                    // 자재출고 유형 얻기
                    자재출고유형_얻기(dc, result, actionArgs);
                }
                // ###############
                // ## 장소 출하 ##
                // ###############
                else if (actionCode == TagAction.A07_장소출하)
                {
                    // 장소정보
                    장소_얻기(dc, result, actionArgs, Comp);

                    if (isComplete == true)
                    {
                        SaveLog();

                        return result;
                    }

                    // 보유품목 정보
                    보유품목_얻기(dc, result, actionArgs);
                }
                // ###################
                // ## 공정생산 시작 ##
                // ################### 2021-03-17 수정
                else if (actionCode == TagAction.A08_공정생산시작)
                {


                    // 생산지시
                    생산지시_얻기(dc, result, actionArgs);

                    var sanum = actionArgs["S9201"];

                    //생산품공정코드
                    //var 수량 = (int)result[TagArg.A05_수량];

                    // 공정 (공정 및 생산품 정보)
                    공정_얻기(dc, result, actionArgs);

                    var goFlag = 공정이력정보_얻기(dc, result, actionArgs, true);
                    if (!goFlag)
                    {
                        result["시작"] = null;
                        //cp.GetSubject<(string, string)>("global.message.param1").OnNext(("EndProcScan", sanum));
                        //Task.Delay(100);
                        _공통관리.메시지ToPDA("StartIngScan", sanum);

                    }
                    else
                    {
                        result["시작"] = "1";
                        // 공정설비
                        //공정설비_얻기(dc, result, actionArgs);


                        var 설비코드 = "";
                        try
                        {

                            설비코드 = result[TagArg.A07_공정설비];
                        }
                        catch (Exception e)
                        {
                            설비코드 = null;
                        }

                        isComplete = true;
                        //if (설비코드 == null)
                        //    cp.GetSubject<(string, string)>("global.message.param1").OnNext(("StartProcScan", sanum));
                        //else
                        //{
                        //    //isComplete = true;
                        //}


                        if (isComplete == true)
                        {
                            SaveLogComp(Comp);

                            공정이력정보_저장(dc, result, actionArgs, "공정시작", Comp);

                            return result;
                        }
                    }

                }
                // ###################
                // ## 공정생산 종료 ##
                // ################### 2021-03-17 수정
                else if (actionCode == TagAction.A09_공정생산종료)
                {
                    var sanum = actionArgs["S9201"];
                    // 생산지시
                    생산지시_얻기(dc, result, actionArgs);

                    // 공정
                    공정_얻기(dc, result, actionArgs);

                    var goFlag = 공정이력정보_얻기(dc, result, actionArgs, false);
                    if (!goFlag)
                    {
                        result["시작"] = null;
                        cp.GetSubject<(string, string)>("global.message.param1").OnNext(("EndProcScan", sanum));
                        //Task.Delay(100);
                        _공통관리.메시지ToPDA("EndProcScan", sanum);

                    }
                    else
                    {
                        result["시작"] = "1";
                        // 공정설비
                        //공정설비_얻기(dc, result, actionArgs);

                        var 설비코드 = "";
                        try
                        {
                            설비코드 = result[TagArg.A07_공정설비];
                        }
                        catch (Exception e)
                        {
                            설비코드 = null;
                        }

                        var 수량 = -1;
                        try
                        {
                            if (설비코드 != null)
                                수량 = int.Parse(actionArgList[5]);  // actionArgs[TagArg.A05_수량]); actionArgList[5]
                        }
                        catch (Exception e)
                        {
                            수량 = -1;
                        }

                        if (수량 != -1)
                        {
                            isComplete = true;
                        }

                        //if (설비코드 == null)
                        //    cp.GetSubject<(string, string)>("global.message.param1").OnNext(("EndProcScan", sanum));

                        if (isComplete == true)
                        {
                            SaveLogComp(Comp);

                            var sawonCode = actionArgList[0];
                            var 생산지시코드 = actionArgs[TagArg.A14_생산지시];
                            var 공정코드 = actionArgs[TagArg.A06_공정];
                            var 공정설비 = actionArgs[TagArg.A07_공정설비];
                            var 생산수량 = actionArgList[5];
                            var 불량수량 = actionArgList[6];
                            var 자재불량수량 = actionArgList[7];

                            //공정이력정보_저장(dc, result, actionArgs, "공정종료");
                            공정실적_저장(dc, result, actionArgs, 생산수량, 불량수량, 자재불량수량, Comp);

                            //공정이력정보_저장(dc, result, actionArgs, "공정종료", Comp);

                            return result;
                        }

                        // 수량 (수량 기본값 1)
                        //result[$"{TagArg.A05_수량}.Text"] = actionArgs.ContainsKey(TagArg.A05_수량) == false ? "1" : actionArgs[TagArg.A05_수량];
                        //result[TagArg.A05_수량] = actionArgs.ContainsKey(TagArg.A05_수량) == false ? "1" : actionArgs[TagArg.A05_수량];

                        // 수량 (수량 기본값 result[TagArg.A05_수량])
                        result[$"{TagArg.A05_수량}.Text"] = null;//actionArgs.ContainsKey(TagArg.A05_수량) == false ? result[TagArg.A05_수량] : actionArgs[TagArg.A05_수량];
                        result[TagArg.A05_수량] = null; // actionArgs.ContainsKey(TagArg.A05_수량) == false ? result[TagArg.A05_수량] : actionArgs[TagArg.A05_수량];
                    }


                }
                // ###################
                // ## 공정생산 실적 ##
                // ################### 2021-03-17 수정
                else if (actionCode == TagAction.A09_공정생산실적)
                {
                    var sanum = actionArgs["S9201"];
                    // 생산지시
                    생산지시_얻기(dc, result, actionArgs);

                    // 공정
                    공정_얻기(dc, result, actionArgs);

                    // 공정설비
                    //공정이력정보_얻기(dc, result, actionArgs);

                    var 설비코드 = "";

                    try
                    {

                        설비코드 = result[TagArg.A07_공정설비];
                    }
                    catch (Exception e)
                    {
                        설비코드 = null;
                    }

                    var 수량 = -1;
                    try
                    {
                        if (설비코드 != null)
                            수량 = int.Parse(actionArgs[TagArg.A05_수량]);
                    }
                    catch (Exception e)
                    {
                        수량 = -1;
                    }

                    if (수량 != -1)
                    {
                        isComplete = true;
                    }

                    //if (설비코드 == null)
                    //    cp.GetSubject<(string, string)>("global.message.param1").OnNext(("EndProcScan", sanum));

                    if (isComplete == true)
                    {
                        SaveLogComp(Comp);
                        //생산실적과 불량수량은 ActionArgList[5],[6]에서
                        var sawonCode = actionArgList[0];
                        var 생산지시코드 = actionArgs[TagArg.A14_생산지시];
                        var 공정코드 = actionArgs[TagArg.A06_공정];
                        var 공정설비 = actionArgs[TagArg.A07_공정설비];
                        var 생산수량 = actionArgList[5];
                        var 불량수량 = actionArgList[6];
                        var 자재불량수량 = actionArgList[7];

                        //공정이력정보_저장(dc, result, actionArgs, "공정종료");
                        공정실적_저장(dc, result, actionArgs, 생산수량, 불량수량, 자재불량수량, Comp);

                        return result;
                    }

                    // 수량 (수량 기본값 1)
                    result[$"{TagArg.A05_수량}.Text"] = actionArgs.ContainsKey(TagArg.A05_수량) == false ? "1" : actionArgs[TagArg.A05_수량];
                    result[TagArg.A05_수량] = actionArgs.ContainsKey(TagArg.A05_수량) == false ? "1" : actionArgs[TagArg.A05_수량];
                }
                // ##############################
                // ## 공정생산시 제품불량 등록 ##
                // ##############################
                else if (actionCode == TagAction.A10_공정생산시제품불량등록)
                {
                    var sanum = actionArgs["S9201"];
                    // 생산지시
                    생산지시_얻기(dc, result, actionArgs);

                    // 공정
                    공정_얻기(dc, result, actionArgs);



                    var goFlag = 공정이력정보_얻기(dc, result, actionArgs, true);
                    if (goFlag)
                    {
                        result["시작"] = null;
                        cp.GetSubject<(string, string)>("global.message.param1").OnNext(("EndProcScan", sanum));
                        //Task.Delay(100);
                        _공통관리.메시지ToPDA("EndProcScan", sanum);

                    }
                    else
                    {
                        // 수량 (수량 기본값 1)
                        result[$"{TagArg.A05_수량}.Text"] = actionArgs.ContainsKey(TagArg.A05_수량) == false ? "1" : actionArgs[TagArg.A05_수량];
                        result[TagArg.A05_수량] = actionArgs.ContainsKey(TagArg.A05_수량) == false ? "1" : actionArgs[TagArg.A05_수량];

                        var 설비코드 = "";
                        try
                        {
                            설비코드 = result[TagArg.A07_공정설비];
                        }
                        catch (Exception e)
                        {
                            설비코드 = null;
                        }

                        if (isComplete == true)
                        {
                            SaveLogComp(Comp);

                            var sawonCode = actionArgList[0];
                            var 생산지시코드 = actionArgs[TagArg.A14_생산지시];
                            var 공정코드 = actionArgs[TagArg.A06_공정];
                            var 공정설비 = actionArgs[TagArg.A07_공정설비];
                            var 불량유형 = actionArgs[TagArg.A11_공정제품불량유형];
                            //var 생산수량 = actionArgList[5];
                            //var 불량수량 = actionArgList[6];
                            //var 자재불량수량 = actionArgList[7];

                            //공정이력정보_저장(dc, result, actionArgs, "공정종료");
                            //공정실적_저장(dc, result, actionArgs, 생산수량, 불량수량, 자재불량수량, Comp);

                            공정불량정보_저장(dc, result, actionArgs, 불량유형, Comp);

                            return result;
                        }



                        // 제품 불량유형
                        불량유형_얻기(dc, result, actionArgs, TagArg.A11_공정제품불량유형);
                    }
                }
                // ##############################
                // ## 공정생산시 자재불량 등록 ##
                // ##############################
                else if (actionCode == TagAction.A11_공정생산시자재불량등록)
                {
                    // 생산지시
                    생산지시_얻기(dc, result, actionArgs);

                    // 공정
                    공정_얻기(dc, result, actionArgs);

                    // 공정설비
                    공정설비_얻기(dc, result, actionArgs);

                    // 수량 (수량 기본값 1)
                    result[$"{TagArg.A05_수량}.Text"] = actionArgs.ContainsKey(TagArg.A05_수량) == false ? "1" : actionArgs[TagArg.A05_수량];
                    result[TagArg.A05_수량] = actionArgs.ContainsKey(TagArg.A05_수량) == false ? "1" : actionArgs[TagArg.A05_수량];

                    if (isComplete == true)
                    {
                        SaveLog();

                        return result;
                    }

                    // 자재 불량유형
                    불량유형_얻기(dc, result, actionArgs, TagArg.A10_공정자재불량유형);
                }
                // ###################
                // ## 품질검사 시작 ##
                // ###################
                else if (actionCode == TagAction.A12_품질검사시작)
                {
                    생산지시_얻기(dc, result, actionArgs);

                    //2021.02.15추가 /////////////////////////////////
                    var sanum = actionArgs["S9201"];
                    var 품목코드 = result[TagArg.A15_생산품목]; //생산품공정코드
                    var 수량 = (int)result[TagArg.A05_수량];
                    // _공통관리.포인트_저장(sanum, 품목코드,수량);
                    /////////////////////////////////////////////////////

                    // 공정
                    품질검사공정_얻기(dc, result, actionArgs);

                    // 품질검사 시작하면 메시지를 발송한다.
                    cp.GetSubject<(string, string)>("global.message.param1").OnNext(("StartScan", sanum));
                    SaveLog();
                    // 현재 하부단 실행 않함
                    if (isComplete == true)
                    {
                        SaveLog();

                        var 공정단위 = dc.공정단위정보
                            .Include(x => x.공정검사목록).ThenInclude(x => x.품질검사)
                            .FirstOrDefault(x => x.공정단위코드 == actionArgs[TagArg.A06_공정]);

                        foreach (var info in 공정단위.공정검사목록)
                        {
                            _품질관리.제품검사_등록(new 보유품목검사정보
                            {
                                보유품목코드 = actionArgList[3],
                                품질검사코드 = info.품질검사.품질검사코드,
                                공정단위코드 = 공정단위.공정단위코드
                            });
                        }

                        //2021.01.12추가
                        //품질검사보유품목_얻기(dc, result, actionArgs);


                        return result;
                    }


                    // 보유품목
                    //보유품목_얻기(dc, result, actionArgs);

                }
                // ###################
                // ## 품질검사 종료 ##
                // ###################
                else if (actionCode == TagAction.A13_품질검사종료)
                {
                    생산지시_얻기(dc, result, actionArgs);

                    //2021.02.15추가 //////////////////////////////////////////
                    var sanum = actionArgs["S9201"];
                    var 품목코드 = result[TagArg.A15_생산품목]; //생산품공정코드
                    // _공통관리.포인트_삭제(sanum, 품목코드);
                    ///////////////////////////////////////////////////////////////

                    // 공정
                    품질검사공정_얻기(dc, result, actionArgs);

                    // 2021.02.16 추가 ////////////////////
                    // 보유품목
                    //actionArgs["S9204"] = 품목코드;
                    생산품_얻기(dc, result, actionArgs);
                    //품목코드 = result[TagArg.A15_생산품목]; //생산품코드
                    //검사보유품목_얻기(dc, result, 품목코드);
                    // 품질검사 시작하면 메시지를 발송한다.
                    cp.GetSubject<(string, string)>("global.message.param1").OnNext(("EndScan", sanum));
                    SaveLog();
                    return result;
                    //////////////////////////////////////////

                    /*if (isComplete == true)
                    {
                        SaveLog();

                        return result;
                    }

                    // 보유품목
                    보유품목_얻기(dc, result, actionArgs);
                    
                    // 품질 불량유형 (정상 포함)
                    불량유형_얻기(dc, result, actionArgs, TagArg.A12_품질제품불량유형);*/
                }
                // #################
                // ## 완제품 등록 ##
                // #################
                else if (actionCode == TagAction.A14_완제품등록)
                {
                    // 생산지시
                    //생산지시_얻기(dc, result, actionArgs);
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    바코드품목_얻기(dc, result, actionArgsStr, Comp);
                    SaveLogComp(Comp);



                    return result;

                    //if (isComplete == true)
                    //{
                    //    SaveLogComp(Comp);

                    //    return result;
                    //}

                    // 보유품목
                    //보유품목_얻기(dc, result, actionArgs);

                    // 수량 (수량 기본값 1)
                    //result[$"{TagArg.A05_수량}.Text"] = actionArgs.ContainsKey(TagArg.A05_수량) == false ? "1" : actionArgs[TagArg.A05_수량];
                    //result[TagArg.A05_수량] = actionArgs.ContainsKey(TagArg.A05_수량) == false ? "1" : actionArgs[TagArg.A05_수량];
                }

                return result;

                // -----------------------------------------------------------------------------------------------------------------------------------------
                static void 장소위치_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs)
                {
                    //var 장소위치목록 = dc.공통코드
                    //    .FirstOrDefault(x => x.장소코드 == actionArgs[TagArg.A02_장소]);  // 장소
                    //if (장소 != default)
                    //{
                    //    result[$"{TagArg.A02_장소}.Text"] = 장소.장소명;
                    //    result[TagArg.A02_장소] = 장소.장소코드;
                    //}

                    var 장소위치목록 = dc.공통코드
                        .Where(x => x.상위코드 == TagArg.A02_장소위치유형)
                        .OrderBy(x => x.정렬순번)
                        .Where(x => x.삭제유무 != true && x.사용유무 == true)
                        .Select(x => new 코드정보 { 코드 = x.코드, 코드명 = x.코드명 })
                        .ToList();

                    result[$"{TagArg.A02_장소위치유형}.Items"] = 장소위치목록;

                    var select = 장소위치목록.FirstOrDefault(x => x.코드 == actionArgs[TagArg.A02_장소위치유형]);
                    if (select != default)
                    {
                        result[$"{TagArg.A02_장소위치유형}.Text"] = select.코드명;
                        result[TagArg.A02_장소위치유형] = select.코드;
                    }
                }


                static void 장소_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, string Comp)
                {
                    var 장소 = dc.장소위치정보
                        .FirstOrDefault(x => x.장소위치코드 == actionArgs[TagArg.A02_장소] && x.회사코드 == Comp);  // 장소
                    if (장소 != default)
                    {
                        result[$"{TagArg.A02_장소}.Text"] = 장소.위치명;
                        result[TagArg.A02_장소] = 장소.장소위치코드;
                    }
                }

                // 2021.04.29
                static void 보유품목_얻기_주문(Data.ApiDbContext dc, Data.ApiDbDZICUBEContext dcDz, Dictionary<string, dynamic> result, string actionArgsCode)
                {
                    //발주서 정보 얻기
                    var actionArgsAry = actionArgsCode.Split(':');
                    var _balCode = actionArgsAry[0];
                    var _sunBun = actionArgsAry[1];
                    var _compCode = actionArgsAry[2];
                    var 순번 = Convert.ToDecimal(_sunBun);

                    //var list = dc.발주서정보
                    //    .Where(x => x.회사코드 == _compCode && x.발주번호 == _balCode && x.발주순번 == 순번)
                    //    .Where_미삭제_사용()
                    //    .Order_등록최신()
                    //    .FirstOrDefault();

                    var list = dcDz.VL_MES_SO
                            .Where(x => x.CO_CD == _compCode && x.SO_NB == _balCode && x.SO_SQ == 순번)
                            .FirstOrDefault();

                    if (list == null)
                        return;

                    var 품번 = list.ITEM_CD;

                    //품목 얻기 - 보유품목에 처음 등록되는 경우도 있다
                    var 보유품목 = dc.품목정보
                        .Where(x => x.품목코드 == 품번)
                        .FirstOrDefault(); // 보유품목
                    if (보유품목 != default)
                    {
                        result[$"{TagArg.A04_보유품목}.Text"] = 보유품목.품목명;
                        result[TagArg.A04_보유품목] = 보유품목.품목코드;

                        // 2021.04.29 추가
                        // result[보유품목.품목코드] = $"{보유품목.수량}";

                        // TODO: 품목정보에서 LOT 개수정보가 추가되면 그 정보를 넣을 것
                        //result[$"{TagArg.A05_수량}.Text"] = $"{(보유품목.품목.LOT기본수량 == 0 ? 1 : 보유품목.품목.LOT기본수량)}";
                        //result[TagArg.A05_수량] = $"{(보유품목.품목.LOT기본수량 == 0 ? 1 : 보유품목.품목.LOT기본수량)}";
                    }
                }
                // 2021.04.29
                ///////////////////////////////////////////////////////////////////////////////
                ///
                // 2021.04.29
                static void 보유품목_얻기_발주(Data.ApiDbContext dc, Data.ApiDbDZICUBEContext dcDz, Dictionary<string, dynamic> result, string actionArgsCode)
                {
                    //발주서 정보 얻기
                    var actionArgsAry = actionArgsCode.Split(':');
                    var _balCode = actionArgsAry[0];
                    var _sunBun = actionArgsAry[1];
                    var _compCode = actionArgsAry[2];
                    var 순번 = Convert.ToDecimal(_sunBun);

                    //var list = dc.발주서정보
                    //    .Where(x => x.회사코드 == _compCode && x.발주번호 == _balCode && x.발주순번 == 순번)
                    //    .Where_미삭제_사용()
                    //    .Order_등록최신()
                    //    .FirstOrDefault();

                    var list = dcDz.VL_MES_PO
                            .Where(x => x.CO_CD == _compCode && x.PO_NB == _balCode && x.PO_SQ == 순번)
                            .FirstOrDefault();

                    if (list == null)
                        return;

                    var 품번 = list.ITEM_CD;

                    //품목 얻기 - 보유품목에 처음 등록되는 경우도 있다
                    var 보유품목 = dc.품목정보
                        .Where(x => x.품목코드 == 품번)
                        .FirstOrDefault(); // 보유품목
                    if (보유품목 != default)
                    {
                        result[$"{TagArg.A04_보유품목}.Text"] = 보유품목.품목명;
                        result[TagArg.A04_보유품목] = 보유품목.품목코드;

                        // 2021.04.29 추가
                        // result[보유품목.품목코드] = $"{보유품목.수량}";

                        // TODO: 품목정보에서 LOT 개수정보가 추가되면 그 정보를 넣을 것
                        //result[$"{TagArg.A05_수량}.Text"] = $"{(보유품목.품목.LOT기본수량 == 0 ? 1 : 보유품목.품목.LOT기본수량)}";
                        //result[TagArg.A05_수량] = $"{(보유품목.품목.LOT기본수량 == 0 ? 1 : 보유품목.품목.LOT기본수량)}";
                    }
                }
                // 2021.04.29
                ///////////////////////////////////////////////////////////////////////////////

                static void 보유품목_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs)
                {
                    var 보유품목 = dc.보유품목정보
                        .Include(x => x.품목)
                        .FirstOrDefault(x => x.보유품목코드 == actionArgs[TagArg.A04_보유품목]); // 보유품목
                    if (보유품목 != default)
                    {
                        result[$"{TagArg.A04_보유품목}.Text"] = 보유품목.품목.품목명;
                        result[TagArg.A04_보유품목] = 보유품목.보유품목코드;

                        // 2020.12.28 추가
                        result[보유품목.보유품목코드] = $"{보유품목.수량}";

                        // TODO: 품목정보에서 LOT 개수정보가 추가되면 그 정보를 넣을 것
                        result[$"{TagArg.A05_수량}.Text"] = $"{(보유품목.품목.LOT기본수량 == 0 ? 1 : 보유품목.품목.LOT기본수량)}";
                        result[TagArg.A05_수량] = $"{(보유품목.품목.LOT기본수량 == 0 ? 1 : 보유품목.품목.LOT기본수량)}";
                    }
                }

                static void 보유품목_얻기_반출(Data.ApiDbContext dc, Dictionary<string, dynamic> result, string actionArgsCode)
                {
                    var 보유품목 = dc.보유품목정보
                        .Include(x => x.품목)
                        .FirstOrDefault(x => x.보유품목코드 == actionArgsCode); // 보유품목
                    if (보유품목 != default)
                    {
                        result[$"{TagArg.A04_보유품목}.Text"] = 보유품목.품목.품목명;
                        result[TagArg.A04_보유품목] = 보유품목.보유품목코드;

                        // 2020.12.28 추가
                        result[보유품목.보유품목코드] = $"{보유품목.수량}";

                        // TODO: 품목정보에서 LOT 개수정보가 추가되면 그 정보를 넣을 것
                        result[$"{TagArg.A05_수량}.Text"] = $"{(보유품목.품목.LOT기본수량 == 0 ? 1 : 보유품목.품목.LOT기본수량)}";
                        result[TagArg.A05_수량] = $"{(보유품목.품목.LOT기본수량 == 0 ? 1 : 보유품목.품목.LOT기본수량)}";
                    }
                }

                static void 보유품목_연속_반출(Data.ApiDbContext dc, Dictionary<string, dynamic> result, string actionArgsCode, int su)
                {
                    var 보유품목 = dc.보유품목정보
                        .Include(x => x.품목)
                        .FirstOrDefault(x => x.보유품목코드 == actionArgsCode); // 보유품목
                    if (보유품목 != default)
                    {
                        result[$"{TagArg.A04_보유품목}.Text"] = 보유품목.품목.품목명;
                        result[TagArg.A04_보유품목] = 보유품목.보유품목코드;

                        // 2020.12.28 추가
                        result[보유품목.보유품목코드] = $"{su}";

                        // TODO: 품목정보에서 LOT 개수정보가 추가되면 그 정보를 넣을 것
                        result[$"{TagArg.A05_수량}.Text"] = $"{(보유품목.품목.LOT기본수량 == 0 ? 1 : 보유품목.품목.LOT기본수량)}";
                        result[TagArg.A05_수량] = $"{(보유품목.품목.LOT기본수량 == 0 ? 1 : 보유품목.품목.LOT기본수량)}";
                    }
                }

                static void 검사보유품목_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, string 품목코드)
                {
                    var 보유품목 = dc.보유품목정보
                        .Include(x => x.품목)
                        .FirstOrDefault(x => x.보유품목코드 == 품목코드); // 보유품목
                    if (보유품목 != default)
                    {
                        result[$"{TagArg.A04_보유품목}.Text"] = 보유품목.품목.품목명;
                        result[TagArg.A04_보유품목] = 보유품목.보유품목코드;

                        // 2020.12.28 추가
                        result[보유품목.보유품목코드] = $"{보유품목.수량}";

                        // TODO: 품목정보에서 LOT 개수정보가 추가되면 그 정보를 넣을 것
                        result[$"{TagArg.A05_수량}.Text"] = $"{(보유품목.품목.LOT기본수량 == 0 ? 1 : 보유품목.품목.LOT기본수량)}";
                        result[TagArg.A05_수량] = $"{(보유품목.품목.LOT기본수량 == 0 ? 1 : 보유품목.품목.LOT기본수량)}";
                    }
                }

                static void 자재입고불량_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, string 보유품목코드)
                {
                    decimal su = 0;
                    decimal t_su = 0;
                    보유품목불량정보 불량보유품목 = null;
                    var 불량유형목록 = result[$"{TagArg.A09_자재입고불량유형}.Items"];
                    for (var lcv = 0; lcv < 불량유형목록.Count; lcv++)
                    {
                        string 불량코드 = 불량유형목록[lcv].코드;
                        불량보유품목 = dc.보유품목불량정보.FirstOrDefault(x => x.보유품목코드 == 보유품목코드 && x.불량유형코드 == 불량코드);
                        // 보유품목이 없을 경우 무시한다.
                        if (불량보유품목 != default)
                        {
                            result[$"{TagArg.A04_보유품목}.Text"] = 불량보유품목.보유품목;
                            result[TagArg.A04_보유품목] = 불량보유품목.보유품목코드;

                            // 2020.12.28 추가
                            result[불량보유품목.보유품목코드] = $"{불량보유품목.수량}";
                            su = 불량보유품목.수량;
                            t_su = t_su + su;
                            /*// TODO: 품목정보에서 LOT 개수정보가 추가되면 그 정보를 넣을 것
                            result[$"{TagArg.A05_수량}.Text"] = $"{(보유품목.품목.LOT기본수량 == 0 ? 1 : 보유품목.품목.LOT기본수량)}";
                            result[TagArg.A05_수량] = $"{(보유품목.품목.LOT기본수량 == 0 ? 1 : 보유품목.품목.LOT기본수량)}";*/
                        }
                        else
                        {
                            result[$"{TagArg.A04_보유품목}.Text"] = 보유품목코드;
                            result[TagArg.A04_보유품목] = 보유품목코드;

                            // 2020.12.28 추가
                            result[보유품목코드] = $"{0}";
                            t_su = t_su + 0;
                        }
                    }
                    // 2020.12.31 추가
                    result[불량보유품목.보유품목코드] = $"{t_su}";
                }

                static void 자재입고불량유형_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs)
                {
                    var 불량유형목록 = dc.공통코드
                        .Where(x => x.상위코드 == TagArg.A09_자재입고불량유형)
                        .OrderBy(x => x.정렬순번)
                        .Where(x => x.삭제유무 != true && x.사용유무 == true)
                        .Select(x => new 코드정보 { 코드 = x.코드, 코드명 = x.코드명 })
                        .ToList();

                    result[$"{TagArg.A09_자재입고불량유형}.Items"] = 불량유형목록;

                    var select = 불량유형목록.FirstOrDefault(x => x.코드 == actionArgs[TagArg.A09_자재입고불량유형]);
                    if (select != default)
                    {
                        result[$"{TagArg.A09_자재입고불량유형}.Text"] = select.코드명;
                        result[TagArg.A09_자재입고불량유형] = select.코드;
                    }
                }

                static void 불량유형_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, string faultyTypeCode)
                {
                    var 불량유형목록 = dc.공통코드
                        .Where(x => x.상위코드 == faultyTypeCode)
                        .OrderBy(x => x.정렬순번)
                        .Where(x => x.삭제유무 != true && x.사용유무 == true)
                        .Select(x => new 코드정보 { 코드 = x.코드, 코드명 = x.코드명 })
                        .ToList();

                    result[$"{faultyTypeCode}.Items"] = 불량유형목록;

                    var select = 불량유형목록.FirstOrDefault(x => x.코드 == actionArgs[faultyTypeCode]);
                    if (select != default)
                    {
                        result[$"{faultyTypeCode}.Text"] = select.코드명;
                        result[faultyTypeCode] = select.코드;
                    }
                }

                static void 공정설비_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs)
                {
                    var 공정설비정보 = dc.보유품목정보
                        .Include(x => x.품목)
                        .Where(x => x.삭제유무 != true && x.사용유무 == true)
                        .FirstOrDefault(x => x.보유품목코드 == actionArgs[TagArg.A07_공정설비]);
                    if (공정설비정보 != default)
                    {
                        result[$"{TagArg.A07_공정설비}.Text"] = 공정설비정보.품목.품목명;
                        result[TagArg.A07_공정설비] = 공정설비정보.보유품목코드;
                    }
                }

                static void 품질검사_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs)
                {
                    var 품질검사 = dc.품질검사정보
                        .FirstOrDefault(x => x.품질검사코드 == actionArgs[TagArg.A13_품질검사]);
                    if (품질검사 != default)
                    {
                        result[$"{TagArg.A13_품질검사}.Text"] = 품질검사.품질검사명;
                        result[TagArg.A13_품질검사] = 품질검사.품질검사코드;
                    }
                }

                static void 품질검사공정_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs)
                {
                    var 공정단위 = dc.공정단위정보
                        .Include(x => x.공정검사목록).ThenInclude(x => x.품질검사)
                        .FirstOrDefault(x => x.공정단위코드 == actionArgs[TagArg.A06_공정]);
                    if (공정단위 != default)
                    {
                        var 공정검사목록 = string.Join("\n", 공정단위.공정검사목록.Select(x => x.품질검사.품질검사명));

                        result[$"{TagArg.A06_공정}.Text"] = $"{공정검사목록}";
                        result[TagArg.A06_공정] = 공정단위.공정단위코드;
                    }
                }

                static void 품질검사보유품목_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs)
                {
                    var 보유품목 = dc.보유품목정보
                        .Include(x => x.품목)
                        .FirstOrDefault(x => x.보유품목코드 == actionArgs[TagArg.A04_보유품목]); // 보유품목
                    if (보유품목 != default)
                    {
                        result[$"{TagArg.A04_보유품목}.Text"] = 보유품목.품목.품목명;
                        result[TagArg.A04_보유품목] = 보유품목.보유품목코드;
                    }
                }

                static void 공정_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs)
                {
                    var 공정생산품정보 = dc.공정단위정보
                        .Include(x => x.공정)
                        .Include(x => x.도면)
                        .Include(x => x.공정품)
                        .Include(x => x.공정자재목록)
                        .FirstOrDefault(x => x.공정단위코드 == actionArgs[TagArg.A06_공정]);
                    if (공정생산품정보 != default)
                    {
                        result[$"{TagArg.A06_공정}.Text"] = $"{공정생산품정보.공정.공정명}\n{공정생산품정보.도면?.도면명 ?? ""}\n{공정생산품정보.공정품?.품목명}";
                        result[TagArg.A06_공정] = 공정생산품정보.공정단위코드;
                    }
                }

                static void 생산지시_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs)
                {
                    var 생산지시 = dc.생산지시정보
                        .Include(x => x.생산계획)
                        .Include(x => x.생산지시공정차수목록).ThenInclude(x => x.생산품공정차수).ThenInclude(x => x.공정단위).ThenInclude(x => x.공정설비목록)
                        .FirstOrDefault(x => x.생산지시코드 == actionArgs[TagArg.A14_생산지시]);
                    if (생산지시 != default)
                    {
                        result[$"{TagArg.A14_생산지시}.Text"] = 생산지시.생산계획.생산계획명;
                        result[TagArg.A14_생산지시] = 생산지시.생산지시코드;
                        //result[$"{TagArg.A15_생산품목}.Text"] = 생산지시.생산계획.생산품코드;
                        result[TagArg.A15_생산품목] = 생산지시.생산계획.생산품공정코드;
                        result[TagArg.A05_수량] = 생산지시.생산수량;     //"S9205";

                        var 생산지시공정차수목록 = 생산지시.생산지시공정차수목록;
                        var 목록수 = 생산지시공정차수목록.Count;
                        if (목록수 > 0)
                        {
                            foreach (var item in 생산지시공정차수목록)
                            {
                                if (item.생산품공정차수.공정단위.공정설비목록.Count != 0)
                                {
                                    string 단위코드 = item.생산품공정차수.공정단위코드;
                                    if (item.생산품공정차수.공정단위코드 == actionArgs[TagArg.A06_공정])
                                    {
                                        result[TagArg.A07_공정설비] = item.생산품공정차수.공정단위.공정설비목록[0].설비코드;
                                        break;
                                    }
                                }

                            }
                        }

                    }
                }

                static void 생산품_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs)
                {
                    var 생산지시 = dc.생산지시정보
                        .Include(x => x.생산계획)
                        .FirstOrDefault(x => x.생산지시코드 == actionArgs[TagArg.A14_생산지시]);
                    if (생산지시 != default)
                    {
                        //  //result[$"{TagArg.A14_생산지시}.Text"] = 생산지시.생산계획.생산계획명;
                        result[TagArg.A14_생산지시] = 생산지시.생산지시코드;
                        //  //result[$"{TagArg.A15_생산품목}.Text"] = 생산지시.생산계획.생산품코드;
                        result[TagArg.A16_검사수량] = 생산지시.검사수량;
                        result[TagArg.A17_합격수량] = 생산지시.합격수량;
                    }
                }

                static void 자재출고유형_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs)
                {
                    var 출고유형목록 = dc.공통코드
                        .Where(x => x.상위코드 == TagArg.A18_자재출고유형)
                        .OrderBy(x => x.정렬순번)
                        .Where(x => x.삭제유무 != true && x.사용유무 == true)
                        .Select(x => new 코드정보 { 코드 = x.코드, 코드명 = x.코드명 })
                        .ToList();

                    result[$"{TagArg.A18_자재출고유형}.Items"] = 출고유형목록;

                    var select = 출고유형목록.FirstOrDefault(x => x.코드 == actionArgs[TagArg.A18_자재출고유형]);
                    if (select != default)
                    {
                        result[$"{TagArg.A18_자재출고유형}.Text"] = select.코드명;
                        result[TagArg.A18_자재출고유형] = select.코드;
                    }
                }

                static void 자재입고유형_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs)
                {
                    var 입고유형목록 = dc.공통코드
                        .Where(x => x.상위코드 == TagArg.A19_자재입고유형)
                        .OrderBy(x => x.정렬순번)
                        .Where(x => x.삭제유무 != true && x.사용유무 == true)
                        .Select(x => new 코드정보 { 코드 = x.코드, 코드명 = x.코드명 })
                        .ToList();

                    result[$"{TagArg.A19_자재입고유형}.Items"] = 입고유형목록;

                    var select = 입고유형목록.FirstOrDefault(x => x.코드 == actionArgs[TagArg.A19_자재입고유형]);
                    if (select != default)
                    {
                        result[$"{TagArg.A19_자재입고유형}.Text"] = select.코드명;
                        result[TagArg.A19_자재입고유형] = select.코드;
                    }
                }



                static void 공정이력정보_저장(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, string 공정상태, string 회사코드)
                {
                    var now = DateTime.Now;
                    var WORK_DT = now.ToString("yyyyMMdd");

                    var 생산지시코드 = actionArgs[$"{TagArg.A14_생산지시}"];
                    var 생산지시정보 = dc.생산지시정보
                        .Include(x => x.생산계획)
                        .FirstOrDefault(x => x.생산지시코드 == 생산지시코드);
                    var 생산품코드 = 생산지시정보.생산계획.생산품코드;
                    string 공정단위코드 = result[$"{TagArg.A06_공정}"];

                    var info = dc.공정이력정보
                        .FirstOrDefault(x => x.회사코드 == 회사코드 && x.생산지시코드 == 생산지시코드 && x.공정단위코드 == 공정단위코드 && x.작업일 == WORK_DT);

                    if (info == default)
                    {
                        string r_val = "";
                        if (actionArgs.ContainsKey(TagArg.A07_공정설비))
                        {
                            if (!result.ContainsKey(TagArg.A07_공정설비))
                            {
                                r_val = "";
                            }
                            else
                            {
                                r_val = result[TagArg.A07_공정설비];
                            }
                        }
                        var 공정이력정보 = new 공정이력정보
                        {
                            회사코드 = 회사코드,
                            생산지시코드 = result[$"{TagArg.A14_생산지시}"],
                            생산지시명 = 생산지시정보.생산지시명,
                            공정단위코드 = result[$"{TagArg.A06_공정}"],
                            설비코드 = r_val, //actionArgs.ContainsKey(TagArg.A07_공정설비) == true ? result[TagArg.A07_공정설비] : null,
                            생산품공정코드 = result[$"{TagArg.A15_생산품목}"],
                            작업자사번 = actionArgs["S9201"],
                            공정상태 = 공정상태,
                            //공정차수 = actionArgs["S9205"],
                            생산수량 = actionArgs.ContainsKey(TagArg.A05_수량) == true ? Convert.ToDecimal(actionArgs[TagArg.A05_수량]) : 0,
                            목표수량 = result[$"{TagArg.A05_수량}"],
                            시작일 = 생산지시정보.시작일,
                            완료목표일 = 생산지시정보.완료목표일,
                            작업일 = WORK_DT,
                            생산품코드 = 생산품코드,
                        };


                        //if (공정상태.Equals("공정종료"))
                        //{
                        //    생산지시정보.실생산량 = 생산지시정보.실생산량 + Convert.ToDecimal(actionArgs[TagArg.A05_수량]);
                        //    dc.생산지시정보.Update(생산지시정보);
                        //}


                        dc.공정이력정보.Add(공정이력정보);
                    }
                    else
                    {
                        info.공정상태 = 공정상태;
                        dc.공정이력정보.Update(info);
                    }

                    dc.SaveChanges();

                    var 공정이력부모 = dc.공정이력정보.OrderByDescending(x => x.공정단위코드 == 공정단위코드 && x.회사코드 == 회사코드 ).FirstOrDefault();

                    if (공정이력부모 != null)
                    {

                        //var 공정이력상세 = dc.공정이력상세정보.OrderByDescending(x => x.공정이력인덱스 == 공정이력부모.인덱스).FirstOrDefault();

                        var 공정이력상세정보 = new 공정이력상세정보
                        {
                            회사코드 = 공정이력부모.회사코드,
                            공정이력인덱스 = 공정이력부모.인덱스,
                            공정상태 = 공정상태,
                            시작타임 = DateTime.Now,
                            작업자사번 = 공정이력부모.작업자사번,

                        };
                        dc.공정이력상세정보.Add(공정이력상세정보);

                    }
                    dc.SaveChanges();

                }

                static void 공정불량정보_저장(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, string 공정상태, string 회사코드)
                {
                    var now = DateTime.Now;
                    var WORK_DT = now.ToString("yyyyMMdd");

                    var 생산지시코드 = actionArgs[$"{TagArg.A14_생산지시}"];
                    var 생산지시정보 = dc.생산지시정보
                        .Include(x => x.생산계획)
                        .FirstOrDefault(x => x.생산지시코드 == 생산지시코드);
                    var 생산품코드 = 생산지시정보.생산계획.생산품코드;
                    string 공정단위코드 = result[$"{TagArg.A06_공정}"];

                    var info = dc.공정이력정보
                        .FirstOrDefault(x => x.회사코드 == 회사코드 && x.생산지시코드 == 생산지시코드 && x.공정단위코드 == 공정단위코드 && x.작업일 == WORK_DT);

                    if (info == default)
                    {
                        var 공정이력정보 = new 공정불량정보
                        {
                            회사코드 = 회사코드,
                            생산지시코드 = result[$"{TagArg.A14_생산지시}"],
                            생산지시명 = 생산지시정보.생산지시명,
                            공정단위코드 = result[$"{TagArg.A06_공정}"],
                            설비코드 = actionArgs.ContainsKey(TagArg.A07_공정설비) == true ? result[TagArg.A07_공정설비] : null,
                            생산품공정코드 = result[$"{TagArg.A15_생산품목}"],
                            작업자사번 = actionArgs["S9201"],
                            불량유형 = 공정상태,
                            불량수량 = 1,
                            작업일 = WORK_DT,
                            생산품코드 = 생산품코드,
                        };


                        //if (공정상태.Equals("공정종료"))
                        //{
                        //    생산지시정보.실생산량 = 생산지시정보.실생산량 + Convert.ToDecimal(actionArgs[TagArg.A05_수량]);
                        //    dc.생산지시정보.Update(생산지시정보);
                        //}


                        dc.공정불량정보.Add(공정이력정보);
                    }





                    dc.SaveChanges();

                }

                static bool 공정이력정보_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, bool Start)
                {
                    var now = DateTime.Now;
                    var WORK_DT = now.ToString("yyyyMMdd");
                    string 생산지시코드 = result[$"{TagArg.A14_생산지시}"];
                    string 공정단위코드 = result[$"{TagArg.A06_공정}"];
                    var 공정이력정보 = dc.공정이력정보
                        //.Include(x => x.생산지시)
                        //.Include(x => x.공정단위)
                        .Where(x => x.생산지시코드 == 생산지시코드 && x.공정단위코드 == 공정단위코드 && x.작업일 == WORK_DT)
                        .FirstOrDefault();
                    if (공정이력정보 != default)
                    {
                        var 공정설비정보 = dc.보유품목정보
                        .Include(x => x.품목)
                        .Where(x => x.삭제유무 != true && x.사용유무 == true)
                        .FirstOrDefault(x => x.보유품목코드 == 공정이력정보.설비코드);
                        if (공정설비정보 != default)
                        {
                            result[$"{TagArg.A07_공정설비}.Text"] = 공정설비정보.품목.품목명;
                            result[TagArg.A07_공정설비] = 공정설비정보.보유품목코드;
                        }
                        else
                        {
                            result[$"{TagArg.A07_공정설비}.Text"] = null;
                            result[TagArg.A07_공정설비] = null;
                        }

                        if (!Start)
                        {
                            if (공정이력정보.공정상태 == "공정종료")
                                return false;
                            else
                                return true;
                        }
                        else
                        {
                            if (공정이력정보.공정상태 == "공정시작")
                                return false;
                            else
                                return true;
                        }

                    }
                    else
                    {
                        if (!Start)
                            return false;

                        return true;
                    }
                }

                static bool 공정이력_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, bool Start)
                {
                    var now = DateTime.Now;
                    var WORK_DT = now.ToString("yyyyMMdd");
                    string 생산지시코드 = result[$"{TagArg.A14_생산지시}"];
                    string 공정단위코드 = result[$"{TagArg.A06_공정}"];
                    var 공정이력정보 = dc.공정이력정보
                        //.Include(x => x.생산지시)
                        //.Include(x => x.공정단위)
                        .Where(x => x.생산지시코드 == 생산지시코드 && x.공정단위코드 == 공정단위코드 && x.작업일 == WORK_DT)
                        .FirstOrDefault();
                    if (공정이력정보 != default)
                    {
                        var 공정설비정보 = dc.보유품목정보
                        .Include(x => x.품목)
                        .Where(x => x.삭제유무 != true && x.사용유무 == true)
                        .FirstOrDefault(x => x.보유품목코드 == 공정이력정보.설비코드);
                        //if (공정설비정보 != default)
                        //{
                        //    result[$"{TagArg.A07_공정설비}.Text"] = 공정설비정보.품목.품목명;
                        //    result[TagArg.A07_공정설비] = 공정설비정보.보유품목코드;
                        //}

                        if (!Start)
                        {
                            if (공정이력정보.공정상태 == "공정종료")
                                return false;
                            else
                                return true;
                        }
                        else
                        {
                            if (공정이력정보.공정상태 == "공정시작")
                                return false;
                            else
                                return true;
                        }

                    }
                    else
                    {
                        if (!Start)
                            return false;

                        return true;
                    }
                }

                //2021.05.19
                static void 공정실적_저장(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs,
                                string 실적수량, string 불량수량, string 자재불량수량, string 회사코드)
                {
                    var now = DateTime.Now;
                    var WORK_DT = now.ToString("yyyyMMdd");
                    string 생산지시코드 = actionArgs[$"{TagArg.A14_생산지시}"];
                    var 생산지시정보 = dc.생산지시정보
                        .Include(x => x.생산계획)
                        .FirstOrDefault(x => x.생산지시코드 == 생산지시코드);
                    string 생산품코드 = 생산지시정보.생산계획.생산품코드;
                    string 공정단위코드 = result[$"{TagArg.A06_공정}"];
                    string 공정상태 = "공정종료";

                    if (실적수량 == null || 실적수량 == "")
                        실적수량 = "0";
                    if (불량수량 == null || 불량수량 == "")
                        불량수량 = "0";
                    if (자재불량수량 == null || 자재불량수량 == "")
                        자재불량수량 = "0";

                    var info = dc.공정이력정보
                        .FirstOrDefault(x => x.회사코드 == 회사코드 && x.생산지시코드 == 생산지시코드 && x.공정단위코드 == 공정단위코드 && x.작업일 == WORK_DT);

                    if (info != default)
                    {
                        info.생산수량 = info.생산수량 + Convert.ToDecimal(실적수량);  //actionArgs.ContainsKey(TagArg.A05_수량) == true ? Convert.ToDecimal(actionArgs[TagArg.A05_수량]) : 0,
                        info.공정상태 = "공정종료";
                        info.불량수량 = info.불량수량 + Convert.ToDecimal(불량수량);
                        info.자재불량수량 = info.자재불량수량 + Convert.ToDecimal(자재불량수량);
                        dc.공정이력정보.Update(info);
                    }


                    if (공정상태.Equals("공정종료"))
                    {
                        생산지시정보.실생산량 = 생산지시정보.실생산량 + Convert.ToDecimal(actionArgs[TagArg.A05_수량]);
                        dc.생산지시정보.Update(생산지시정보);
                    }

                    var 공정이력부모 = dc.공정이력정보.OrderByDescending(x => x.공정단위코드 == 공정단위코드).FirstOrDefault();

                    if (공정이력부모 != null)
                    {

                        var 공정이력상세 = dc.공정이력상세정보.OrderByDescending(x => x.인덱스 ).FirstOrDefault();

                        if (공정이력상세 != null)
                        {
                            공정이력상세.생산수량 = Convert.ToDecimal(실적수량);
                            공정이력상세.불량수량 = Convert.ToDecimal(불량수량);
                            공정이력상세.자재불량수량 = Convert.ToDecimal(자재불량수량);
                            공정이력상세.공정상태 = 공정상태;
                            공정이력상세.종료타임 = DateTime.Now;
                            dc.공정이력상세정보.Update(공정이력상세);
                        }

                    }

                    dc.SaveChanges();



                    /*
                    var sawonCode = actionArgs["S9201"];
                    //var 생산지시코드 = actionArgs[TagArg.A14_생산지시];
                    var 공정코드 = actionArgs[TagArg.A06_공정];
                    var 공정설비 = actionArgs[TagArg.A07_공정설비];

                    var 생산지시코드 = actionArgs[$"{TagArg.A14_생산지시}"];
                    var 생산지시정보 = dc.생산지시정보
                        .Include(x => x.생산계획)
                        .FirstOrDefault(x => x.생산지시코드 == 생산지시코드);
                    var 생산품코드 = 생산지시정보.생산계획.생산품코드;



                    var 생산실적헤더정보 = new 생산실적헤더정보
                    {
                        회사코드 = 회사코드,
                        생산지시코드 = 생산지시코드,
                        공정단위코드 = 공정코드,
                        생산품코드 = 생산품코드,
                        생산품공정코드 = 생산지시정보.생산계획.생산품공정.생산품공정코드,
                        사업장코드 = "1000",
                        실적공정코드_창고코드 = "2000",
                        실적작업장코드_장소코드 = "2001",
                        재작업여부 = 생산지시정보.재작업여부,
                        생산지시명 = 생산지시정보.생산지시명,
                        실적수량 = Convert.ToDecimal(실적수량),
                        불량수량 = Convert.ToDecimal(불량수량),

                    };
                    var 생산실적상세정보 = new 생산실적상세정보
                    {
                        회사코드 = 회사코드,
                        생산지시코드 = 생산지시코드,
                        작업자사번 = sawonCode,
                        //사용수량 = Edit사용수량,
                        실적등록일 = DateTime.Now,
                        비고 = "",

                    };

                    var 공정단위자재목록 = dc.공정단위자재정보
                        .Where(x => x.회사코드 == 회사코드 && x.공정단위코드 == 공정코드)
                        .Where_미삭제_사용()
                        .Order_등록최신().ToList();

                    var listBom = 공정단위자재목록.ToList();




                    try
                    {
                        var 생산지시유무 = dc.생산실적헤더정보.Where(x => x.회사코드 == 회사코드 && x.생산지시코드 == 생산지시코드).FirstOrDefault();

                        //var 작업순번 = dc.생산실적상세정보.Count(x => x.회사코드 == 상세.회사코드 && x.생산지시코드 == 상세.생산지시코드 ) + 1;
                        var 작업자생산실적 = new 작업자생산실적정보
                        {
                            회사코드 = 회사코드,
                            생산지시코드 = 생산지시코드,
                            생산지시명 = 생산지시정보.생산지시명,
                            작업순번 = dc.작업자생산실적정보.Count(x => x.회사코드 == 회사코드) + 1,
                            공정단위코드 = 공정코드,
                            작업자사번 = sawonCode,
                            생산품코드 = 생산품코드,
                            실적수량 = 생산실적헤더정보.실적수량,
                            불량수량 = 생산실적헤더정보.불량수량,
                            실적등록일 = 생산실적상세정보.실적등록일,

                        };
                        dc.작업자생산실적정보.Add(작업자생산실적);

                        if (생산지시유무 == null)
                            dc.생산실적헤더정보.Add(생산실적헤더정보);
                        else
                        {
                            생산지시유무.실적수량 = 생산지시유무.실적수량 + 생산실적헤더정보.실적수량;
                            생산지시유무.불량수량 = 생산지시유무.불량수량 + 생산실적헤더정보.불량수량;

                            dc.생산실적헤더정보.Update(생산지시유무);
                        }

                        dc.SaveChanges();
                        foreach (var item in listBom)
                        {
                            var 작업순번 = dc.생산실적상세정보.Count(x => x.회사코드 == 회사코드 && x.생산지시코드 == 생산지시코드) + 1;
                            생산실적상세정보.사용수량 = item.수량 * (생산실적헤더정보.실적수량 + 생산실적헤더정보.불량수량);
                            생산실적상세정보.사용품번 = item.자재코드;

                            생산실적상세정보.작업순번 = 작업순번;
                            dc.생산실적상세정보.Add(생산실적상세정보);

                            dc.SaveChanges();
                        }


                        //result = true;
                    }
                    catch (Exception ex)
                    {
                        //result = false;
                    }

                   


                    //bool result = await Remote.Command.생산관리.작업생산실적_저장(생산실적헤더정보, 생산실적상세정보, listBom);


                    //var 공정이력정보 = new 공정이력정보
                    //{
                    //    회사코드 = 회사코드,
                    //    생산지시코드 = result[$"{TagArg.A14_생산지시}"],
                    //    생산지시명 = 생산지시정보.생산지시명,
                    //    공정단위코드 = result[$"{TagArg.A06_공정}"],
                    //    설비코드 = actionArgs.ContainsKey(TagArg.A07_공정설비) == true ? result[TagArg.A07_공정설비] : null,
                    //    //생산품공정코드 = result[$"{TagArg.A15_생산품목}"],
                    //    작업자사번 = actionArgs["S9201"],
                    //    공정상태 = 공정상태,
                    //    생산수량 = actionArgs.ContainsKey(TagArg.A05_수량) == true ? Convert.ToDecimal(actionArgs[TagArg.A05_수량]) : 0,
                    //    목표수량 = result[$"{TagArg.A05_수량}"],
                    //    시작일 = 생산지시정보.시작일,
                    //    완료목표일 = 생산지시정보.완료목표일,
                    //    //생산품코드 = 생산품코드,
                    //};


                    //if (공정상태.Equals("공정종료"))
                    //{
                    //    생산지시정보.실생산량 = 생산지시정보.실생산량 + Convert.ToDecimal(actionArgs[TagArg.A05_수량]);
                    //    dc.생산지시정보.Update(생산지시정보);
                    //}


                    //dc.공정이력정보.Add(공정이력정보);

                    dc.SaveChanges();
                    */

                }

                //2021.05.22
                //static void 자재구매유형_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs)
                //{
                //    var 입고유형목록 = dc.공통코드
                //           .Where(x => x.상위코드 == TagArg.A19_자재입고유형)
                //           .OrderBy(x => x.정렬순번)
                //           .Where(x => x.삭제유무 != true && x.사용유무 == true)
                //           .Select(x => new 코드정보 { 코드 = x.코드, 코드명 = x.코드명 })
                //           .ToList();

                //    result[$"{TagArg.A19_자재구매유형}.Items"] = 입고유형목록;

                //    var select = 입고유형목록.FirstOrDefault(x => x.코드 == actionArgs[TagArg.A19_자재구매유형]);
                //    if (select != default)
                //    {
                //        result[$"{TagArg.A19_자재구매유형}.Text"] = select.코드명;
                //        result[TagArg.A19_자재구매유형] = select.코드;
                //    }
                //}
            }

            static IReadOnlyDictionary<string, string> GetActionArgs(string actionArgsScheme, string[] actionArgsData)
            {
                var a = actionArgsScheme.Split(',', StringSplitOptions.RemoveEmptyEntries);
                var b = actionArgsData;

                var result = new Dictionary<string, string>();
                for (var i = 0; i < a.Length; i++)
                {
                    var key = a[i];
                    var value = i >= b.Length ? null : b[i];

                    result[key] = value;
                }

                return result;
            }

            void SaveLog()
            {
                if (actionInfo.변경액션유무 == true)
                {
                    //var actionArgListAry = actionArgList[1].Split(':');
                    //var actionAA = actionArgListAry[0];
                    //string[] AAA = actionArgList;
                    //AAA[1] = actionAA;
                    // 액션로그 기록
                    var log = new 액션로그
                    {
                        //회사코드 = compCode,
                        직원사번 = actionArgs["S9201"],   // 작업자
                        액션코드 = actionCode,
                        액션인자 = string.Join(',', actionArgList),
                        액션시각 = DateTime.Now,
                        연동장비식별번호 = 연동장비.식별번호
                    };
                    dc.액션로그.Add(log);
                    dc.SaveChanges();
                }

                // 액션로그가 기록되면 메시지를 발송한다.
                cp.GetSubject<(string, string)>("global.message.param1").OnNext(("ReceivedActionTag", actionCode));
            }

            void SaveLogComp(string compCode)
            {
                if (actionInfo.변경액션유무 == true)
                {
                    //var actionArgListAry = actionArgList[1].Split(':');
                    //var actionAA = actionArgListAry[0];
                    //string[] AAA = actionArgList;
                    //AAA[1] = actionAA;
                    // 액션로그 기록
                    var log = new 액션로그
                    {
                        회사코드 = compCode,
                        직원사번 = actionArgList[0],   // 작업자
                        액션코드 = actionCode,
                        //액션인자 = actionArgList[4],
                        액션시각 = DateTime.Now,
                        연동장비식별번호 = 연동장비.식별번호
                    };
                    dc.액션로그.Add(log);
                    dc.SaveChanges();
                }

                // 액션로그가 기록되면 메시지를 발송한다.
                cp.GetSubject<(string, string)>("global.message.param1").OnNext(("ReceivedActionTag", actionCode));
            }


            //2021.05.11
            static void 바코드품목_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, string actionArgsStr, string Comp)
            {
                var actArgs_Ary = actionArgsStr.Split(':');
                var actArgsCode = actArgs_Ary[0];
                var actLotCode = actArgs_Ary[1];
                var 보유품목 = dc.바코드발급정보
                    .Include(x => x.품목)
                    .Where(x => x.품목코드 == actArgsCode && x.LOT번호 == actLotCode && x.회사코드 == Comp)
                    .FirstOrDefault(); // 보유품목
                if (보유품목 != default)
                {
                    result[$"{TagArg.A04_보유품목}.Text"] = 보유품목.품목.품목명;
                    result[TagArg.A04_보유품목] = 보유품목.품목코드;

                    // 2020.12.28 추가
                    result[보유품목.품목코드] = $"{보유품목.수량}";

                    // TODO: 품목정보에서 LOT 개수정보가 추가되면 그 정보를 넣을 것
                    result[$"{TagArg.A05_수량}.Text"] = $"{보유품목.수량}";
                    result[TagArg.A05_수량] = $"{보유품목.수량}";
                }
            }

            //2021.05.11
            static bool 바코드품목_얻기_초기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, string actionArgsStr, string Comp)
            {
                var actArgs_Ary = actionArgsStr.Split(':');
                var actArgsCode = actArgs_Ary[0];
                var actLotCode = actArgs_Ary[1];
                var 보유품목 = dc.바코드발급정보
                    .Include(x => x.품목)
                    .Where(x => x.품목코드 == actArgsCode && x.LOT번호 == actLotCode && x.회사코드 == Comp)
                    .FirstOrDefault(); // 보유품목
                if (보유품목 != default)
                {
                    result[$"{TagArg.A04_보유품목}.Text"] = 보유품목.품목.품목명;
                    result[TagArg.A04_보유품목] = 보유품목.품목코드;

                    // 2020.12.28 추가
                    result[보유품목.품목코드] = $"{보유품목.수량}";

                    // TODO: 품목정보에서 LOT 개수정보가 추가되면 그 정보를 넣을 것
                    result[$"{TagArg.A05_수량}.Text"] = $"{보유품목.수량}";
                    result[TagArg.A05_수량] = $"{보유품목.수량}";

                    // 2021.05.25
                    result[TagArg.A06_공정] = $"{보유품목.구분}";

                    if (보유품목.입고유무 == false)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            //2021.05.11
            static void 바코드품목_얻기_출고(Data.ApiDbContext dc, Dictionary<string, dynamic> result, string actionArgsStr, string Comp)
            {
                var actArgs_Ary = actionArgsStr.Split(':');
                var actArgsCode = actArgs_Ary[0];
                var actLotCode = actArgs_Ary[1];
                var 보유품목 = dc.보유품목정보
                    .Include(x => x.품목)
                    .Where(x => x.품목코드 == actArgsCode && x.회사코드 == Comp)
                    .FirstOrDefault(); // 보유품목
                if (보유품목 != default)
                {
                    result[$"{TagArg.A04_보유품목}.Text"] = 보유품목.품목.품목명;
                    result[TagArg.A04_보유품목] = 보유품목.품목코드;

                    // 2020.12.28 추가
                    result[보유품목.품목코드] = $"{보유품목.수량}";

                    // TODO: 품목정보에서 LOT 개수정보가 추가되면 그 정보를 넣을 것
                    result[$"{TagArg.A05_수량}.Text"] = $"{보유품목.수량}";
                    result[TagArg.A05_수량] = $"{보유품목.수량}";
                }
            }

            //2021.05.25
            static void 바코드품목_얻기_위치(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, string actionArgsStr, string Comp)
            {
                //품목코드
                var 품번_LOT = actionArgs[TagArg.A04_보유품목];
                //위치상세코드
                var 위치상세코드 = actionArgs[TagArg.A03_장소위치];
                var 장소위치코드 = 위치상세코드.Substring(0, 8);

                var actArgs_Ary = actionArgsStr.Split(':');
                var actArgsCode = actArgs_Ary[0];
                var actLotCode = actArgs_Ary[1];

                var 입고보유품목 = dc.보유품목임시위치정보
                                    .Where(x => x.보유품목코드 == actArgsCode && x.LOT번호 == actLotCode && x.회사코드 == Comp)// && x.품목_LOT번호 == 품번_LOT)
                                    .FirstOrDefault();
                var 보유품목 = dc.바코드발급정보
                    .Include(x => x.품목)
                    .Where(x => x.품목코드 == actArgsCode && x.LOT번호 == actLotCode && x.회사코드 == Comp)
                    .FirstOrDefault(); // 보유품목
                if (보유품목 != default && 입고보유품목 != default)
                {
                    result[$"{TagArg.A04_보유품목}.Text"] = 보유품목.품목.품목명;
                    result[TagArg.A04_보유품목] = 보유품목.품목코드;

                    // 2020.12.28 추가
                    result[보유품목.품목코드] = $"{입고보유품목.수량}";

                    // TODO: 품목정보에서 LOT 개수정보가 추가되면 그 정보를 넣을 것
                    result[$"{TagArg.A05_수량}.Text"] = $"{입고보유품목.수량}";
                    result[TagArg.A05_수량] = $"{입고보유품목.수량}";
                }
            }

            static void 자재발주유형_얻기(Data.ApiDbDZICUBEContext dcDz, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, string actionArgsStr, string Comp)
            {
                var actArgs_Ary = actionArgsStr.Split(':');
                var actArgsCode = actArgs_Ary[0];
                var actLotCode = actArgs_Ary[1];

                var now = DateTime.Now;
                var 기준day = now.AddDays(-180);
                //코드정보2 입고목록List;

                //var 입고유형목록 = dcDz.VL_MES_PO
                //           .Where(x => x.CO_CD == Comp && x.ITEM_CD == actArgsCode )  // && x.DUE_DT == 납기일)
                //           .Select(x => new 코드정보2 { 코드 = x.PO_NB + ':' + x.SHIPREQ_DT, 코드명 = x.PO_NB + ':' + x.SHIPREQ_DT })
                //           .ToList();

                var 입고유형List = dcDz.VL_MES_PO
                           .Where(x => x.CO_CD == Comp && x.ITEM_CD == actArgsCode)  // && x.DUE_DT == 납기일)
                                                                                     //.Select(x => new 코드정보2 { 코드 = x.PO_NB + ':' + x.SHIPREQ_DT, 코드명 = x.PO_NB + ':' + x.SHIPREQ_DT })
                           .ToList();
                // && (DateTime.Compare(기준day, DateTime.ParseExact(x.PO_DT, "yyyyMMdd", null)
                if (입고유형List != default)
                {
                    var 입고List = 입고유형List.Where(x => DateTime.ParseExact(x.PO_DT, "yyyyMMdd", null) > 기준day)
                        .Select(x => new 코드정보2 { 코드 = x.PO_NB + ':' + x.SHIPREQ_DT, 코드명 = x.PO_NB + ':' + x.TR_NM })
                        .ToList();

                    result[$"{TagArg.A19_자재입고유형}.Items"] = 입고List;

                    var select1 = 입고List.FirstOrDefault(x => x.코드 == actionArgs[TagArg.A19_자재입고유형]);
                    if (select1 != default)
                    {
                        result[$"{TagArg.A19_자재입고유형}.Text"] = select1.코드명;
                        result[TagArg.A19_자재입고유형] = select1.코드;
                    }
                }
                //var 입고유형목록 = dc.발주서정보
                //    .Where(x => x.회사코드 == Comp && x.품번 == actArgsCode)
                //    .Select(x => new 코드정보2 { 코드 = x.발주번호+':'+ x.출하예정일, 코드명 = x.출하예정일 })
                //    .ToList();

                //result[$"{TagArg.A19_자재입고유형}.Items"] = 입고유형목록;

                //var select = 입고유형목록.FirstOrDefault(x => x.코드 == actionArgs[TagArg.A19_자재입고유형]);
                //if (select != default)
                //{
                //    result[$"{TagArg.A19_자재입고유형}.Text"] = select.코드명;
                //    result[TagArg.A19_자재입고유형] = select.코드;
                //}
            }

            static void 자재발주유형_외주얻기(Data.ApiDbDZICUBEContext dcDz, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, string actionArgsStr, string Comp)
            {
                var actArgs_Ary = actionArgsStr.Split(':');
                var actArgsCode = actArgs_Ary[0];
                var actLotCode = actArgs_Ary[1];

                var now = DateTime.Now;
                var 기준day = now.AddDays(-180);

                var 입고유형목록 = dcDz.VL_MES_WO_WF
                    .Where(x => x.CO_CD == Comp && x.ITEM_CD == actArgsCode)
                    .Select(x => new 코드정보2 { 코드 = x.WO_CD + ':' + x.ITEM_CD, 코드명 = x.WO_CD + ':' + x.ITEM_NM })
                    .ToList();


                //var 입고유형목록 = dcDz.VL_MES_WO_WF
                //    .Where(x => x.CO_CD == Comp && x.ITEM_CD == actArgsCode && (DateTime.Compare(기준day, DateTime.ParseExact(x.ORD_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)) < 0))
                //    .Select(x => new 코드정보2 { 코드 = x.WO_CD + ':' + x.ITEM_CD, 코드명 = x.ITEM_NM })
                //    .ToList();

                result[$"{TagArg.A19_자재입고유형}.Items"] = 입고유형목록;

                var select = 입고유형목록.FirstOrDefault(x => x.코드 == actionArgs[TagArg.A19_자재입고유형]);
                if (select != default)
                {
                    result[$"{TagArg.A19_자재입고유형}.Text"] = select.코드명;
                    result[TagArg.A19_자재입고유형] = select.코드;
                }
            }

            static void 자재발주유형_외주_Date(Data.ApiDbDZICUBEContext dcDz, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, string actionArgsStr, string Comp)
            {
                var actArgs_Ary = actionArgsStr.Split(':');
                var actArgsCode = actArgs_Ary[0];
                var actLotCode = actArgs_Ary[1];

                var now = DateTime.Now;
                var 기준day = now.AddDays(-180);

                var 입고유형목록 = dcDz.VL_MES_WO_WF
                    .Where(x => x.CO_CD == Comp && x.ITEM_CD == actArgsCode)
                    .Select(x => new 코드정보2 { 코드 = x.WO_CD + ':' + x.ORD_DT, 코드명 = x.WO_CD + ':' + x.ITEM_NM })
                    .ToList();


                //var 입고유형목록 = dcDz.VL_MES_WO_WF
                //    .Where(x => x.CO_CD == Comp && x.ITEM_CD == actArgsCode && (DateTime.Compare(기준day, DateTime.ParseExact(x.ORD_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)) < 0))
                //    .Select(x => new 코드정보2 { 코드 = x.WO_CD + ':' + x.ITEM_CD, 코드명 = x.ITEM_NM })
                //    .ToList();

                result[$"{TagArg.A19_자재입고유형}.Items"] = 입고유형목록;

                var select = 입고유형목록.FirstOrDefault(x => x.코드 == actionArgs[TagArg.A19_자재입고유형]);
                if (select != default)
                {
                    result[$"{TagArg.A19_자재입고유형}.Text"] = select.코드명;
                    result[TagArg.A19_자재입고유형] = select.코드;
                }
            }

            //2021.05.12
            static void 바코드입고품목_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, string Comp, bool isStart)
            {
                //품목코드
                var 품번_LOT = actionArgs[TagArg.A04_보유품목];
                //위치상세코드
                var 위치상세코드 = actionArgs[TagArg.A03_장소위치];
                var 장소위치코드 = 위치상세코드.Substring(0, 8);

                var actArgs_Ary = 품번_LOT.Split(':');
                var actArgsCode = actArgs_Ary[0];
                var actLotCode = actArgs_Ary[1];
                보유품목위치정보 입고보유품목;

                입고보유품목 = dc.보유품목위치정보
                                           .Include(x => x.보유품목)
                                           .ThenInclude(y => y.품목)
                                           .Where(x => x.보유품목코드 == actArgsCode && x.장소위치코드 == 장소위치코드 && x.품목_LOT번호 == 품번_LOT)
                                           .FirstOrDefault();
                //if (isStart)
                //{
                //    입고보유품목 = dc.보유품목위치정보
                //                           .Include(x => x.보유품목)
                //                           .ThenInclude(y => y.품목)
                //                           .Where(x => x.보유품목코드 == actArgsCode && x.장소위치코드 == 장소위치코드 && x.품목_LOT번호 == 품번_LOT)
                //                           .FirstOrDefault();
                //}
                //else
                //{
                //    입고보유품목 = dc.보유품목위치정보
                //                           .Include(x => x.보유품목)
                //                           .ThenInclude(y => y.품목)
                //                           .Where(x => x.보유품목코드 == actArgsCode && x.위치상세코드 == 위치상세코드)// && x.품목_LOT번호 == 품번_LOT)
                //                           .FirstOrDefault();
                //}

                if (입고보유품목 != default)
                {
                    result[$"{TagArg.A04_보유품목}.Text"] = 입고보유품목.보유품목.품목.품목명;
                    result[TagArg.A04_보유품목] = 입고보유품목.보유품목코드;

                    // 2020.12.28 추가
                    result[입고보유품목.보유품목코드] = $"{입고보유품목.수량}";

                    // TODO: 품목정보에서 LOT 개수정보가 추가되면 그 정보를 넣을 것
                    result[$"{TagArg.A05_수량}.Text"] = $"{(입고보유품목.보유품목.품목.LOT기본수량 == 0 ? 1 : 입고보유품목.보유품목.품목.LOT기본수량)}";
                    result[TagArg.A05_수량] = $"{(입고보유품목.보유품목.품목.LOT기본수량 == 0 ? 1 : 입고보유품목.보유품목.품목.LOT기본수량)}";
                }

            }

            static void 자재주문유형_얻기(Data.ApiDbDZICUBEContext dcDz, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, string actionArgsStr, string Comp)
            {
                var actArgs_Ary = actionArgsStr.Split(':');
                var actArgsCode = actArgs_Ary[0];
                var actLotCode = actArgs_Ary[1];

                var 출고유형목록 = dcDz.VL_MES_SO
                           .Where(x => x.CO_CD == Comp && x.ITEM_CD == actArgsCode) // && x.DUE_DT == 납기일)
                           .Select(x => new 코드정보2 { 코드 = x.SO_NB + ':' + x.SHIPREQ_DT + ':' + x.SO_QT, 코드명 = x.SO_NB + ':' + x.TR_NM })
                           .ToList();
                //var 입고유형목록 = dc.발주서정보
                //    .Where(x => x.회사코드 == Comp && x.품번 == actArgsCode)
                //    .Select(x => new 코드정보2 { 코드 = x.발주번호+':'+ x.출하예정일, 코드명 = x.출하예정일 })
                //    .ToList();

                result[$"{TagArg.A18_자재출고유형}.Items"] = 출고유형목록;

                var select = 출고유형목록.FirstOrDefault(x => x.코드 == actionArgs[TagArg.A18_자재출고유형]);
                if (select != default)
                {
                    result[$"{TagArg.A18_자재출고유형}.Text"] = select.코드명;
                    result[TagArg.A18_자재출고유형] = select.코드;
                }
            }


            static void 자재이동유형_얻기(Data.ApiDbContext dc, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, string actionArgsStr, string Comp)
            {
                var actArgs_Ary = actionArgsStr.Split(':');
                var actArgsCode = actArgs_Ary[0];
                var actLotCode = actArgs_Ary[1];

                var 출고유형목록 = dc.공통코드
                       .Where(x => x.상위코드 == TagArg.A19_자재이동유형)
                       .OrderBy(x => x.정렬순번)
                       .Where(x => x.삭제유무 != true && x.사용유무 == true)
                       .Select(x => new 코드정보 { 코드 = x.코드, 코드명 = x.코드명 })
                       .ToList();

                //var 출고유형목록 = dcDz.VL_MES_SO
                //           .Where(x => x.CO_CD == Comp && x.ITEM_CD == actArgsCode) // && x.DUE_DT == 납기일)
                //           .Select(x => new 코드정보2 { 코드 = x.SO_NB + ':' + x.SHIPREQ_DT, 코드명 = x.SHIPREQ_DT })
                //           .ToList();
                //var 입고유형목록 = dc.발주서정보
                //    .Where(x => x.회사코드 == Comp && x.품번 == actArgsCode)
                //    .Select(x => new 코드정보2 { 코드 = x.발주번호+':'+ x.출하예정일, 코드명 = x.출하예정일 })
                //    .ToList();

                result[$"{TagArg.A19_자재이동유형}.Items"] = 출고유형목록;

                var select = 출고유형목록.FirstOrDefault(x => x.코드 == actionArgs[TagArg.A19_자재이동유형]);
                if (select != default)
                {
                    result[$"{TagArg.A19_자재이동유형}.Text"] = select.코드명;
                    result[TagArg.A19_자재이동유형] = select.코드;
                }
            }

            static void 생산이동유형_얻기(Data.ApiDbDZICUBEContext dcDz, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, string actionArgsStr, string Comp)
            {


                //품목코드
                var 품번_LOT = actionArgs[TagArg.A04_보유품목];
                var pum_Ary = 품번_LOT.Split(':');
                var 품번 = pum_Ary[0];
                //위치상세코드
                var 위치상세코드 = actionArgs[TagArg.A03_장소위치];
                var 장소위치코드 = 위치상세코드.Substring(0, 8);
                var actArgs_Ary = actionArgsStr.Split(':');
                var actArgsCode = actArgs_Ary[0];
                var actLotCode = actArgs_Ary[1];

                //var 출고유형목록 = dc.공통코드
                //       .Where(x => x.상위코드 == TagArg.A19_자재이동유형)
                //       .OrderBy(x => x.정렬순번)
                //       .Where(x => x.삭제유무 != true && x.사용유무 == true)
                //       .Select(x => new 코드정보 { 코드 = x.코드, 코드명 = x.코드명 })
                //       .ToList();



                var now = DateTime.Now;
                var 기준day = now.AddDays(-180);


                var BOM_Single = dcDz.VL_MES_BOM.Where(x => x.CO_CD == Comp && x.ITEMCHILD_CD == 품번).FirstOrDefault();


                var 더존비오엠 = BOM_Single;
                List<string> Parents = new List<string>();

                List<string> ParentsP = new List<string>();

                // 모품목 찾기
                if (BOM_Single != null)
                {
                    var BOM_Multi = dcDz.VL_MES_BOM.Where(x => x.CO_CD == Comp && x.ITEMCHILD_CD == 품번).ToList();
                    if (BOM_Multi.Count > 0)
                    {
                        // 1st 모품목 찾기
                        foreach (var BOMitem in BOM_Multi)
                        {
                            var ICheck = BOMitem.ITEMPARENT_CD.EndsWith("I");
                            if (ICheck)
                            {
                                더존비오엠 = dcDz.VL_MES_BOM.Where(x => x.CO_CD == Comp && x.ITEMCHILD_CD == BOMitem.ITEMPARENT_CD).FirstOrDefault();
                                if (더존비오엠 != default)
                                {
                                    if (!Parents.Contains(더존비오엠.ITEMPARENT_CD))
                                        Parents.Add(더존비오엠.ITEMPARENT_CD);
                                }

                            }
                            else
                            {
                                if (!Parents.Contains(더존비오엠.ITEMPARENT_CD))
                                    Parents.Add(BOMitem.ITEMPARENT_CD);
                            }
                        }

                        foreach (var Bitem in Parents)
                        {
                            var B_Multi = dcDz.VL_MES_BOM.Where(x => x.CO_CD == Comp && x.ITEMCHILD_CD == Bitem).ToList();
                            if (B_Multi.Count > 0)
                            {
                                // 1st 모품목 찾기
                                foreach (var BOMitem in B_Multi)
                                {
                                    var ICheck = BOMitem.ITEMPARENT_CD.EndsWith("I");
                                    if (ICheck)
                                    {
                                        더존비오엠 = dcDz.VL_MES_BOM.Where(x => x.CO_CD == Comp && x.ITEMCHILD_CD == BOMitem.ITEMPARENT_CD).FirstOrDefault();
                                        if (더존비오엠 != default)
                                        {
                                            if (!ParentsP.Contains(더존비오엠.ITEMPARENT_CD))
                                                ParentsP.Add(더존비오엠.ITEMPARENT_CD);
                                        }

                                    }
                                    else
                                    {
                                        if (!ParentsP.Contains(더존비오엠.ITEMPARENT_CD))
                                            ParentsP.Add(BOMitem.ITEMPARENT_CD);
                                    }
                                }

                            }
                        }

                        if (ParentsP.Count == 0)
                            ParentsP = Parents;
                    }

                }


                List<VL_MES_SO> 출고유형List = new List<VL_MES_SO>();
                List<VL_MES_SO> 출고유형 = new List<VL_MES_SO>();

                //for (var lcv = 0; lcv < Parents.Count; lcv++)
                //{
                //    VL_MES_SO 출고유형 = dcDz.VL_MES_SO
                //      .Where(x => x.CO_CD == Comp && x.ITEM_CD == Parents[lcv])
                //      .FirstOrDefault();

                //    if (출고유형 != default)
                //        출고유형List.Add(출고유형);
                //}

                for (var lcv = 0; lcv < ParentsP.Count; lcv++)
                {
                    출고유형 = dcDz.VL_MES_SO
                     .Where(x => x.CO_CD == Comp && x.ITEM_CD == ParentsP[lcv])
                     .ToList();

                    if (출고유형 != null)
                    {
                        foreach (var item in 출고유형)
                        {
                            출고유형List.Add(item);
                        }
                    }

                }

                //var 출고유형List = dcDz.VL_MES_SO
                //  .Where(x => x.CO_CD == Comp && x.ITEM_CD == 더존비오엠.ITEMPARENT_CD)
                //  .ToList();




                //var 출고유형목록 = dc.생산지시정보
                //    .Include(x => x.생산계획).Where(x => x.회사코드 == Comp)
                //    .Where(x => (DateTime.Compare(기준day, x.CreateTime) < 0))
                //    .Select(x => new 코드정보2 { 코드 = x.생산지시코드, 코드명 = x.생산지시명 })
                //    .ToList();

                //var 입고유형목록 = dc.발주서정보
                //    .Where(x => x.회사코드 == Comp && x.품번 == actArgsCode)
                //    .Select(x => new 코드정보2 { 코드 = x.발주번호+':'+ x.출하예정일, 코드명 = x.출하예정일 })
                //    .ToList();
                if (출고유형List != default)
                {
                    var 출고유형목록 = 출고유형List.Where(x => DateTime.ParseExact(x.SO_DT, "yyyyMMdd", null) > 기준day)
                        .Select(x => new 코드정보2 { 코드 = x.SO_NB + ':' + x.ITEM_CD, 코드명 = x.SO_NB + ':' + x.ITEM_NM })
                        .ToList();

                    result[$"{TagArg.A19_생산이동유형}.Items"] = 출고유형목록;

                    var select = 출고유형목록.FirstOrDefault(x => x.코드 == actionArgs[TagArg.A19_생산이동유형]);
                    if (select != default)
                    {
                        result[$"{TagArg.A19_생산이동유형}.Text"] = select.코드명;
                        result[TagArg.A19_생산이동유형] = select.코드;
                    }

                }

                출고유형List = null;
                Parents = null;
                출고유형 = null;

            }
            static void 외주이동유형_얻기(Data.ApiDbDZICUBEContext dcDz, Dictionary<string, dynamic> result, IReadOnlyDictionary<string, string> actionArgs, string actionArgsStr, string Comp)
            {
                //품목코드
                var 품번_LOT = actionArgs[TagArg.A04_보유품목];
                var pum_Ary = 품번_LOT.Split(':');
                var 품번 = pum_Ary[0];
                //위치상세코드
                var 위치상세코드 = actionArgs[TagArg.A03_장소위치];
                var 장소위치코드 = 위치상세코드.Substring(0, 8);

                var actArgs_Ary = actionArgsStr.Split(':');
                var actArgsCode = actArgs_Ary[0];
                var actLotCode = actArgs_Ary[1];


                var now = DateTime.Now;
                var 기준day = now.AddDays(-180);


                var BOM_Single = dcDz.VL_MES_BOM.Where(x => x.CO_CD == Comp && x.ITEMCHILD_CD == 품번).FirstOrDefault();
                var 더존비오엠 = BOM_Single;
                List<string> Parents = new List<string>();

                if (BOM_Single != null)
                {
                    var BOM_Multi = dcDz.VL_MES_BOM.Where(x => x.CO_CD == Comp && x.ITEMCHILD_CD == 품번).ToList();
                    if (BOM_Multi != null)
                    {
                        foreach (var BOMitem in BOM_Multi)
                        {
                            var ICheck = BOMitem.ITEMPARENT_CD.EndsWith("I");
                            if (ICheck)
                            {
                                더존비오엠 = dcDz.VL_MES_BOM.Where(x => x.CO_CD == Comp && x.ITEMCHILD_CD == BOMitem.ITEMPARENT_CD).FirstOrDefault();
                                if (더존비오엠 != default)
                                {
                                    Parents.Add(더존비오엠.ITEMPARENT_CD);
                                }

                            }
                            else
                            {
                                Parents.Add(BOMitem.ITEMPARENT_CD);
                            }
                        }
                    }

                }



                //if (BOM_Single != null)
                //{
                //    var len = BOM_Single.ITEMPARENT_CD.Length;
                //    var CharCheck = BOM_Single.ITEMPARENT_CD.EndsWith("I");
                //    if (CharCheck)
                //    {
                //        더존비오엠 = dcDz.VL_MES_BOM.Where(x => x.CO_CD == Comp && x.ITEMCHILD_CD == BOM_Single.ITEMPARENT_CD).FirstOrDefault();
                //    }
                //    else
                //    {
                //        더존비오엠 = BOM_Single;
                //    }
                //}
                //var 출고유형List = dcDz.VL_MES_WO_WF
                //   .Where(x => x.CO_CD == Comp && x.ITEM_CD == 더존비오엠.ITEMPARENT_CD)
                //   .ToList();

                List<VL_MES_WO_WF> 출고유형List = new List<VL_MES_WO_WF>();
                List<VL_MES_WO_WF> 출고유형 = new List<VL_MES_WO_WF>();

                for (var lcv = 0; lcv < Parents.Count; lcv++)
                {
                    출고유형 = dcDz.VL_MES_WO_WF
                     .Where(x => x.CO_CD == Comp && x.ITEM_CD == Parents[lcv])
                     .ToList();

                    if (출고유형 != null)
                    {
                        foreach (var item in 출고유형)
                        {
                            출고유형List.Add(item);
                        }
                    }

                }


                if (출고유형List != default)
                {
                    var 출고유형목록 = 출고유형List.Where(x => DateTime.ParseExact(x.ORD_DT, "yyyyMMdd", null) > 기준day)
                        .Select(x => new 코드정보2 { 코드 = x.WO_CD + ':' + x.ITEM_CD, 코드명 = x.WO_CD + ':' + x.ITEM_NM })
                        .ToList();

                    result[$"{TagArg.A19_외주이동유형}.Items"] = 출고유형목록;

                    var select = 출고유형목록.FirstOrDefault(x => x.코드 == actionArgs[TagArg.A19_외주이동유형]);
                    if (select != default)
                    {
                        result[$"{TagArg.A19_외주이동유형}.Text"] = select.코드명;
                        result[TagArg.A19_외주이동유형] = select.코드;
                    }

                }

                출고유형List = null;
                Parents = null;
                출고유형 = null;

            }

        }
    }
}


/*//
 * 
                ###############
                ## 이동 출고 ##
                ###############
                else if (actionCode == TagAction.A06_자재이동출고)
                {
                    var 위치상세 = dc.위치상세정보
                        .Include(x => x.장소위치)
                        .ThenInclude(y => y.장소)
                        .FirstOrDefault(x => x.위치상세코드 == actionArgs[TagArg.A03_장소위치] && x.회사코드 == Comp);  // 장소
                    if (위치상세 != default)
                    {
                        result[$"{TagArg.A03_장소위치}.Text"] = $"{위치상세.장소위치.위치명}\n{위치상세.위치명}";
                        result[TagArg.A03_장소위치] = 위치상세.위치상세코드;
                    }

                    // 2021.05.13
                    string[] actionArgs_Ary;
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    var actionArgsCode = "";

                    int 일련번호자리수;
                    bool 일련번호_flag = false;
                    if (actionArgsStr != null)
                    {
                        actionArgs_Ary = actionArgsStr.Split(':');
                        actionArgsCode = actionArgs_Ary[0];
                        var ary_len = actionArgs_Ary.Length;
                        if (ary_len > 2)
                        {
                            일련번호자리수 = actionArgs_Ary[2].Length;
                            if (일련번호자리수 == 5)
                                일련번호_flag = true;
                        }
                    }


                    if (isComplete == true)
                    {
                        SaveLogComp(Comp);


                        var 품번_LOT = actionArgs[TagArg.A04_보유품목];
                        //위치상세코드
                        var 위치상세코드 = actionArgs[TagArg.A03_장소위치];
                        var 장소위치코드 = 위치상세코드.Substring(0, 8);
                        decimal D수량 = decimal.Parse(actionArgList[3]);

                        string 이동유형 = "5"; //재고이동
                        var sawonCode = actionArgList[0];
                        // actionArgList 확인
                        //var 사유 = actionArgList[4];  //사유
                        //if (사유 == "S922201")
                        //{
                        //    이동유형 = "5";  //재고이동
                        //}
                        //else if (사유 == "S922202")
                        //{
                        //    이동유형 = "0";  //생산
                        //}
                        //else if (사유 == "S922203")
                        //{
                        //    이동유형 = "1";  //외주
                        //}


                        var actArgs_Ary = 품번_LOT.Split(':');
                        var actArgsCode = actArgs_Ary[0];
                        var actLotCode = actArgs_Ary[1];



                        var 입고보유품목 = dc.보유품목위치정보
                                                    .Include(x => x.보유품목)
                                                    .ThenInclude(y => y.품목)
                                                    .Where(x => x.보유품목코드 == actArgsCode && x.장소위치코드 == 장소위치코드 && x.품목_LOT번호 == 품번_LOT)
                                                    .FirstOrDefault();



                        var info1 = new 입고처리상세정보
                        {
                            품번 = 입고보유품목.보유품목코드, //품목코드,
                            회사코드 = Comp, //회사코드,
                            //품목코드 = 품목코드,
                            입고수량_관리단위 = Convert.ToInt32(D수량), //Convert.ToInt32(resp.PO_QT),
                            //장소코드 = str장소코드,
                            입고장소코드 = 입고보유품목.장소위치코드,
                            //보유년월일 = now.ToString("yyMMdd"),
                            작업순번 = 0,
                            LOT번호 = actLotCode,
                            비고 = 품번_LOT,
                        };

                        _자재관리.자재이동_임시등록(info1, "자재이동출고", 이동유형, 위치상세코드, null);

                        // 2021.05.12
                        바코드입고품목_얻기(dc, result, actionArgs, Comp, true);

                        return result;

                    }

                    // 보유품목 정보
                    if (actionArgsStr != null)
                    {

                        //2021.05.12
                        바코드품목_얻기(dc, result, actionArgsStr, Comp);

                        // 자재이동유형얻기 필요시 해제
                        //자재이동유형_얻기(dc, result, actionArgs, actionArgsStr, Comp);
                        
                    }

                }
                // ###############
                // ## 이동 입고 ##
                // ###############
                else if (actionCode == TagAction.A06_자재이동입고)
                {
                    var 위치상세 = dc.위치상세정보
                        .Include(x => x.장소위치)
                        .ThenInclude(y => y.장소)
                        .FirstOrDefault(x => x.위치상세코드 == actionArgs[TagArg.A03_장소위치] && x.회사코드 == Comp);  // 장소
                    if (위치상세 != default)
                    {
                        result[$"{TagArg.A03_장소위치}.Text"] = $"{위치상세.장소위치.위치명}\n{위치상세.위치명}";
                        result[TagArg.A03_장소위치] = 위치상세.위치상세코드;
                    }

                    // 2021.05.13
                    string[] actionArgs_Ary;
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    var actionArgsCode = "";

                    int 일련번호자리수;
                    bool 일련번호_flag = false;
                    if (actionArgsStr != null)
                    {
                        actionArgs_Ary = actionArgsStr.Split(':');
                        actionArgsCode = actionArgs_Ary[0];
                        var ary_len = actionArgs_Ary.Length;
                        if (ary_len > 2)
                        {
                            일련번호자리수 = actionArgs_Ary[2].Length;
                            if (일련번호자리수 == 5)
                                일련번호_flag = true;
                        }
                    }


                    if (isComplete == true)
                    {
                        SaveLogComp(Comp);

                        var sawonCode = actionArgList[0];
                        var 품번_LOT = actionArgs[TagArg.A04_보유품목];
                        //위치상세코드
                        var 위치상세코드 = actionArgs[TagArg.A03_장소위치];
                        var 장소위치코드 = 위치상세코드.Substring(0, 8);
                        //decimal D수량 = decimal.Parse(actionArgList[3]);

                        var actArgs_Ary = 품번_LOT.Split(':');
                        var actArgsCode = actArgs_Ary[0];
                        var actLotCode = actArgs_Ary[1];


                        var 입고보유품목 = dc.보유품목임시위치정보
                                             .Where(x => x.보유품목코드 == actArgsCode && x.LOT번호 == actLotCode && x.품목_LOT번호 == 품번_LOT)
                                             .FirstOrDefault();


                        if (입고보유품목 != default)
                        {
                            _자재관리.자재이동_임시등록_해제(입고보유품목, "자재이동입고", "5", 위치상세코드, sawonCode);
                        }

                        // 2021.05.12
                        바코드입고품목_얻기(dc, result, actionArgs, Comp, true);

                        return result;

                    }

                    // 보유품목 정보
                    if (actionArgsStr != null)
                    {

                        //2021.05.12
                        바코드품목_얻기(dc, result, actionArgsStr, Comp);

                    }

                }
                // ###################
                // ## 생산이동 출고 ##
                // ###################
                else if (actionCode == TagAction.A06_생산이동출고)
                {
                    var 위치상세 = dc.위치상세정보
                        .Include(x => x.장소위치)
                        .ThenInclude(y => y.장소)
                        .FirstOrDefault(x => x.위치상세코드 == actionArgs[TagArg.A03_장소위치] && x.회사코드 == Comp);  // 장소
                    if (위치상세 != default)
                    {
                        result[$"{TagArg.A03_장소위치}.Text"] = $"{위치상세.장소위치.위치명}\n{위치상세.위치명}";
                        result[TagArg.A03_장소위치] = 위치상세.위치상세코드;
                    }

                    // 2021.05.13
                    string[] actionArgs_Ary;
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    var actionArgsCode = "";

                    int 일련번호자리수;
                    bool 일련번호_flag = false;
                    if (actionArgsStr != null)
                    {
                        actionArgs_Ary = actionArgsStr.Split(':');
                        actionArgsCode = actionArgs_Ary[0];
                        var ary_len = actionArgs_Ary.Length;
                        if (ary_len > 2)
                        {
                            일련번호자리수 = actionArgs_Ary[2].Length;
                            if (일련번호자리수 == 5)
                                일련번호_flag = true;
                        }
                    }


                    if (isComplete == true)
                    {
                        SaveLogComp(Comp);


                        var 품번_LOT = actionArgs[TagArg.A04_보유품목];
                        //위치상세코드
                        var 위치상세코드 = actionArgs[TagArg.A03_장소위치];
                        var 장소위치코드 = 위치상세코드.Substring(0, 8);
                        decimal D수량 = decimal.Parse(actionArgList[3]);

                        string 이동유형 = "0"; //생산이동
                        var sawonCode = actionArgList[0];
                        var 사유 = actionArgList[4];  // 생산지시서 번호 
                        //if (사유 == "S922201")
                        //{
                        //    이동유형 = "5";  //재고이동
                        //}
                        //else if (사유 == "S922202")
                        //{
                        //    이동유형 = "0";  //생산
                        //}
                        //else if (사유 == "S922203")
                        //{
                        //    이동유형 = "1";  //외주
                        //}


                        var actArgs_Ary = 품번_LOT.Split(':');
                        var actArgsCode = actArgs_Ary[0];
                        var actLotCode = actArgs_Ary[1];



                        var 입고보유품목 = dc.보유품목위치정보
                                                    .Include(x => x.보유품목)
                                                    .ThenInclude(y => y.품목)
                                                    .Where(x => x.보유품목코드 == actArgsCode && x.장소위치코드 == 장소위치코드 && x.품목_LOT번호 == 품번_LOT)
                                                    .FirstOrDefault();



                        var info1 = new 입고처리상세정보
                        {
                            품번 = 입고보유품목.보유품목코드, //품목코드,
                            회사코드 = Comp, //회사코드,
                            //품목코드 = 품목코드,
                            입고수량_관리단위 = Convert.ToInt32(D수량), //Convert.ToInt32(resp.PO_QT),
                            //장소코드 = str장소코드,
                            입고장소코드 = 입고보유품목.장소위치코드,
                            //보유년월일 = now.ToString("yyMMdd"),
                            작업순번 = 0,
                            LOT번호 = actLotCode,
                            비고 = 품번_LOT,
                        };

                        _자재관리.자재이동_임시등록(info1, "자재이동출고", 이동유형, 위치상세코드,사유);

                        // 2021.05.12
                        바코드입고품목_얻기(dc, result, actionArgs, Comp, true);

                        return result;

                    }

                    // 보유품목 정보
                    if (actionArgsStr != null)
                    {

                        //2021.05.12
                        바코드품목_얻기(dc, result, actionArgsStr, Comp);

                        //자재이동유형_얻기(dc, result, actionArgs, actionArgsStr, Comp);
                        생산이동유형_얻기(dc, result, actionArgs, actionArgsStr, Comp);
                        //자재이동출고유형얻기 필요
                    }

                }
                // ###################
                // ## 생산이동 입고 ##
                // ###################
                else if (actionCode == TagAction.A06_생산이동입고)
                {
                    var 위치상세 = dc.위치상세정보
                        .Include(x => x.장소위치)
                        .ThenInclude(y => y.장소)
                        .FirstOrDefault(x => x.위치상세코드 == actionArgs[TagArg.A03_장소위치] && x.회사코드 == Comp);  // 장소
                    if (위치상세 != default)
                    {
                        result[$"{TagArg.A03_장소위치}.Text"] = $"{위치상세.장소위치.위치명}\n{위치상세.위치명}";
                        result[TagArg.A03_장소위치] = 위치상세.위치상세코드;
                    }

                    // 2021.05.13
                    string[] actionArgs_Ary;
                    var actionArgsStr = actionArgs[TagArg.A04_보유품목];
                    var actionArgsCode = "";

                    int 일련번호자리수;
                    bool 일련번호_flag = false;
                    if (actionArgsStr != null)
                    {
                        actionArgs_Ary = actionArgsStr.Split(':');
                        actionArgsCode = actionArgs_Ary[0];
                        var ary_len = actionArgs_Ary.Length;
                        if (ary_len > 2)
                        {
                            일련번호자리수 = actionArgs_Ary[2].Length;
                            if (일련번호자리수 == 5)
                                일련번호_flag = true;
                        }
                    }


                    if (isComplete == true)
                    {
                        SaveLogComp(Comp);

                        var sawonCode = actionArgList[0];
                        var 품번_LOT = actionArgs[TagArg.A04_보유품목];
                        //위치상세코드
                        var 위치상세코드 = actionArgs[TagArg.A03_장소위치];
                        var 장소위치코드 = 위치상세코드.Substring(0, 8);
                        //decimal D수량 = decimal.Parse(actionArgList[3]);

                        var actArgs_Ary = 품번_LOT.Split(':');
                        var actArgsCode = actArgs_Ary[0];
                        var actLotCode = actArgs_Ary[1];


                        var 입고보유품목 = dc.보유품목임시위치정보
                                             .Where(x => x.보유품목코드 == actArgsCode && x.LOT번호 == actLotCode && x.품목_LOT번호 == 품번_LOT)
                                             .FirstOrDefault();


                        if (입고보유품목 != default)
                        {
                            //_자재관리.자재이동_임시등록_해제(입고보유품목, "자재이동입고", "자재이동입고", 위치상세코드, sawonCode);
                            _자재관리.자재이동_임시등록_해제(입고보유품목, "자재이동입고", "0", 위치상세코드, sawonCode);
                        }

                        // 2021.05.12
                        바코드입고품목_얻기(dc, result, actionArgs, Comp, true);

                        return result;

                    }

                    // 보유품목 정보
                    if (actionArgsStr != null)
                    {

                        //2021.05.12
                        바코드품목_얻기(dc, result, actionArgsStr, Comp);

                        //자재이동유형_얻기(dc, result, actionArgs, actionArgsStr, Comp);
                        //자재이동출고유형얻기 필요
                    }

                }
 *
 //*/