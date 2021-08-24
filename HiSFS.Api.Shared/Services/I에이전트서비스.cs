using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using HiSFS.Api.Shared.Models;

using WampSharp.V2.Rpc;

namespace HiSFS.Api.Shared.Services
{
    public interface I에이전트서비스
    {
        [WampProcedure("설비가동현황_저장")]
        Task 설비가동현황_저장(설비가동현황정보 info);
        [WampProcedure("설비가동현황_저장_list")]
        Task 설비가동현황_저장(IEnumerable<설비가동현황정보> list);
        [WampProcedure("설비가동현황_조회")]
        Task<IEnumerable<설비가동현황정보>> 설비가동현황_조회();
        [WampProcedure("디지털미터_전송")]
        Task 디지털미터_전송(string id, decimal value, string unit);
    }
}
