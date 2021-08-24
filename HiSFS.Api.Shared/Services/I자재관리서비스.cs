using HiSFS.Api.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace HiSFS.Api.Shared.Services
{
    public interface I자재관리서비스
    {
        [WampProcedure("자재현황_조회")]
        Task<IEnumerable<보유품목정보>> 자재현황_조회(string 회사코드 = null);
        [WampProcedure("자재현황_이력_조회")]
        Task<IEnumerable<보유품목이력>> 자재현황_이력_조회(string 보유품목코드, string 회사코드);

        [WampProcedure("보유품목_발행")]
        Task<string> 보유품목_발행(품목정보 품목, 보유품목정보 보유품목);

        [WampProcedure("보유품목코드_저장")]
        Task 보유품목코드_저장(보유품목정보 info, bool isAdd);
        //2021.03.11 추가
        [WampProcedure("자재현황_입고_조회")]
        Task<IEnumerable<보유품목이력>> 자재현황_입고_조회(string 변경유형코드, string 회사코드);


        //2021.03.15 추가
        [WampProcedure("자재현황_입고분기_조회")]
        Task<IEnumerable<보유품목이력>> 자재현황_입고분기_조회(string 변경유형코드, string datetime, string 회사코드);


        [WampProcedure("자재관리_보유품목일련번호생성_저장")]
        Task 자재관리_보유품목일련번호생성_저장(품목정보 품목, 보유품목정보 보유품목, DateTime 보유년월일, DateTime 생산년월일, string 사용자일련번호);

        [WampProcedure("자재관리_보유품목_입고")]
        Task 자재관리_보유품목_입고(string 보유품목코드, decimal 수량, string 장소코드, string 사유);

        [WampProcedure("자재관리_보유품목일련번호생성_Update")]
        Task 자재관리_보유품목일련번호생성_Update(string _보유품목코드, decimal 수량, string 장소코드, string 위치코드, string 사유);


        [WampProcedure("보유품목일지_저장")]
        Task 보유품목일지_저장(품목정보 품목, 보유품목정보 보유품목);

        [WampProcedure("보유품목일지_조회")]
        Task<IEnumerable<보유품목일지>> 보유품목일지_조회(string 보유품목코드);

        [WampProcedure("보유품목일련정보_상세")]
        Task<List<보유품목일련정보>> 보유품목일련정보_상세(string 보유품목일지코드, string 품목코드, string 보유년월일);

        [WampProcedure("자재관리_보유품목일련번호생성_PDA입고")]
        Task 자재관리_보유품목일련번호생성_PDA입고(string 보유품목코드, decimal 수량);

        [WampProcedure("보유품목일지_PDA저장")]
        Task 보유품목일지_PDA저장(string 보유품목코드, decimal 수량);

        [WampProcedure("보유품목일련정보_마지막순번조회")]
        Task<int> 보유품목일련정보_마지막순번조회(string 품목코드, DateTime 보유년월일);

        [WampProcedure("보유품목일지_삭제")]
        Task<bool> 보유품목일지_삭제(보유품목일지 info);


        //ERP 보유품목 입고
        [WampProcedure("입고관리보유품목입고_등록")]
        Task<string> 입고관리보유품목입고_등록(입고처리상세정보 상세, bool isAdd);

        [WampProcedure("자재관리_입고관리보유품목이력_등록")]
        Task 자재관리_입고관리보유품목이력_등록(보유품목정보 보유품목, decimal 수량, string 장소코드, string 사유);

        [WampProcedure("출고관리보유품목입고_등록")]
        Task<string> 출고관리보유품목입고_등록(출고처리상세정보 상세, bool isAdd);

        [WampProcedure("보유품목입고_위치등록")]

        public Task<bool> 보유품목입고_위치등록(입고처리상세정보 보유품목_P, string 입출고여부, string 사유, string P_장소위치코드, bool isAdd);




        [WampProcedure("보유품목출고_위치등록")]

        Task<bool> 보유품목출고_위치등록(보유품목정보 보유품목_P, string 입출고여부, string 사유);

        [WampProcedure("보유품목_품목출고")]
        Task 보유품목_품목출고(string 보유품목코드, decimal 수량, string 사유);

        //2021.05.10
        [WampProcedure("품목코드_바코드발급")]
        Task<string> 품목코드_바코드발급(string 품목코드, string 회사코드, string 수량, string sawon, bool YN, string 구분);

        [WampProcedure("보유품목LOT입고_위치등록")]
        Task<bool> 보유품목LOT입고_위치등록(입고처리상세정보 보유품목_P, string 입출고여부, string 사유, string P_장소위치코드, bool isAdd);

        [WampProcedure("자재위치현황_조회")]
        Task<IEnumerable<보유품목위치정보>> 자재위치현황_조회(string 회사코드, string 보유품목코드);

        [WampProcedure("자재장소위치별품목현황_조회")]
        Task<IEnumerable<보유품목위치정보>> 자재장소위치별품목현황_조회(string 회사코드, string 장소위치코드);

        //2021.05.13
        [WampProcedure("보유품목LOT출고_위치등록")]
        Task<bool> 보유품목LOT출고_위치등록(보유품목정보 보유품목_P, string 입출고여부, string 사유, string P_장소위치코드);

        //2021.05.18
        [WampProcedure("자재이동_임시등록")]
        Task<bool> 자재이동_임시등록(입고처리상세정보 보유품목_P, string 입출고여부, string 사유, string P_장소위치코드, string 지시서);

        [WampProcedure("자재이동_임시등록_해제")]
        Task<bool> 자재이동_임시등록_해제(보유품목임시위치정보 보유품목_P, string 입출고여부, string 사유, string P_장소위치코드, string 사원코드);

        //2021.05.18
        //ERP 보유품목 입고
        [WampProcedure("입고관리보유품목입고Action_등록")]
        Task<string> 입고관리보유품목입고Action_등록(입고처리상세정보 상세, bool isAdd);


        [WampProcedure("품목코드_NOT바코드발급")]
        Task<string> 품목코드_NOT바코드발급(string 품목코드, string 회사코드, string 수량, string sawon, bool YN);
       
        /*
        [WampProcedure("생산이동_임시등록")]
        Task<bool> 생산이동_임시등록(입고처리상세정보 보유품목_P, string 입출고여부, string 사유, string P_장소위치코드, string 지시서);
        [WampProcedure("생산이동_임시등록_해제")]
        public Task<bool> 생산이동_임시등록_해제(보유품목임시위치정보 보유품목_P, string 입출고여부, string 사유, string P_장소위치코드, string 사원코드)
        */
    }
}
