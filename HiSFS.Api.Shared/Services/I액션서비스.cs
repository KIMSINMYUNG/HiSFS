using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace HiSFS.Api.Shared.Services
{
    public interface I액션서비스
    {
        [WampProcedure("연동장비_등록확인")]
        Task<(bool, bool)> 연동장비_등록확인(string 장비ID);
        [WampProcedure("연동장비_등록")]
        Task 연동장비_등록(string deviceId, string deviceKind);
        [WampProcedure("연동장비_승인")]
        Task<bool> 연동장비_승인(string deviceId, bool isConfirm);
        [WampProcedure("액션태그_처리")]
        Task<IReadOnlyDictionary<string, dynamic>> 액션태그_처리(string Comp, string deviceId, string actionCode, string[] actionArgList, bool isComplete);
        //[WampProcedure("액션태그_스키마확인")]
        //Task<IReadOnlyDictionary<string, dynamic>> 액션태그_스키마확인(string deviceId, string actionCode);

        [WampProcedure("출고_판단")]
        Task<bool> 출고_판단(string deviceId, string actionCode, string 보유품목코드);

        [WampProcedure("입고_판단")]
        Task<bool> 입고_판단(string deviceId, string actionCode, string 발주서코드);

        [WampProcedure("주문출고_판단")]
        Task<bool> 주문출고_판단(string deviceId, string actionCode, string 주문서코드);
    }

    public class 코드정보
    {
        public string 코드 { get; set; }
        public string 코드명 { get; set; }
    }

    //2021.05.11
    public class 코드정보2
    {
        public string 코드 { get; set; }
        public string 코드명 { get; set; }
    }
}
