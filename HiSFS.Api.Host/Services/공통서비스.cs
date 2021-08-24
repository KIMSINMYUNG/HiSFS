using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.PivotView;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WampSharp.Binding;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.PubSub;

namespace HiSFS.Api.Host.Services
{

    /// <summary>
    /// 서비스 공통 API 노출
    /// </summary>
    public class 공통서비스 : I공통서비스
    {
        private readonly IContextProvider dcp;
        private readonly IConfiguration config;

        public string PrnUri;
        public string ServerUri;

        //2021.02.15 추가 품질검사 User관리 ///////////////////////////////////////////////////////////
        private static PointCodeDictionary<검사인력포인터> pointMap = new PointCodeDictionary<검사인력포인터>();
        private static PointCodeDictionary<IList<검사인력포인터>> pointListMap = new PointCodeDictionary<IList<검사인력포인터>>();

        public PointCodeDictionary<검사인력포인터> 포인터 => pointMap;
        public PointCodeDictionary<IList<검사인력포인터>> 포인터목록 => pointListMap;

        private 품질관리서비스 _품질관리;
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////


        private static readonly int DefaultPort = 31200;
        private static readonly string DefaultServiceUri = $"ws://0.0.0.0:{DefaultPort}/";
        private static readonly string DefaultRealm = "HiSFS.Api";

        public 공통서비스(IContextProvider dbContextProvider, IConfiguration config, 품질관리서비스 품질관리)
        {
            this.dcp = dbContextProvider;

            this.config = config;
            _품질관리 = 품질관리;

            var section = config.GetSection("RemoteApi");
            //var port = int.Parse(section["Port"]);
            PrnUri = section["PrnUri"];
            ServerUri = section["ServerUri"];

            pointMap.Clear();
            pointListMap.Clear();


            //realm = host.RealmContainer.GetRealmByName(DefaultRealm);
        }

        // 메뉴 {{{
        public Task<IEnumerable<메뉴정보>> 메뉴_조회()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.메뉴정보
                .Include(x => x.상위메뉴)
                    .ThenInclude(x => x.상위메뉴)
                .Where_미삭제()
                .OrderBy(x => x.뎁스)
                .ThenBy(x => x.정렬순번)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }
        // }}}

        // 공통코드 {{{
        public Task<IEnumerable<공통코드>> 공통코드_조회()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.공통코드
                .Include(x => x.하위.Where(y => y.삭제유무 != true).OrderBy(y => y.정렬순번))
                    .ThenInclude(x => x.하위.Where(y => y.삭제유무 != true).OrderBy(y => y.정렬순번))
                .Where_미삭제()
                .Where(x => x.상위코드 == null)
                .OrderBy(x => x.코드유형코드)
                .ThenBy(x => x.정렬순번)
                .ToList();

            var result = SearchDeep(list).ToList();

            return Task.FromResult(result.AsEnumerable());

            static IEnumerable<공통코드> SearchDeep(IEnumerable<공통코드> source)
            {
                if (source == null)
                    yield break;

                foreach (var node in source)
                {
                    yield return node;
                    foreach (var childNode in SearchDeep(node.하위))
                        yield return childNode;
                }
            }
        }

        public Task 공통코드_저장(공통코드 code, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isAdd == true)
                dc.공통코드.Add(code);
            else
                dc.공통코드.Update(code);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 공통코드_삭제(공통코드 code, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.공통코드.Remove(code);
            else
            {
                code.삭제유무 = true;
                dc.공통코드.Update(code);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }
        // }}}

        public Task<(직원정보, string)> 직원_인증(string userId, string password, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.직원정보
                .Include(x => x.권한정보)
                .Where_미삭제_사용()
                .FirstOrDefault(x => x.사번 == userId && x.회사코드 == 회사코드);

            if (result == default)
                return Task.FromResult<(직원정보, string)>((null, "사번 또는 암호가 일치하지 않습니다."));

            // 로그인 권한정보가 없을 경우 생성해준다.
            if (result.권한정보 == null)
            {
                result.권한정보 = new 직원권한정보
                {
                    회사코드 = result.회사코드,
                    //사번 = result.사번,
                    암호 = result.사번,
                    암호암호화유무 = false         // 이후 암호를 암호화 해야 함
                };
                dc.직원권한정보.Add(result.권한정보);
                dc.SaveChanges();
            }

            if (result.권한정보.암호암호화유무 == false && result.권한정보.암호 != password)
                return Task.FromResult<(직원정보, string)>((null, "사번 또는 암호가 일치하지 않습니다."));

            메뉴직원권한_반영(dc, result);

            return Task.FromResult<(직원정보, string)>((result, ""));
        }

        private static void 메뉴직원권한_반영(Data.ApiDbContext dc, 직원정보 result)
        {
            // 직원기준 메뉴 권한정보 만듬 {{{
            // 직원 권한 = 메뉴부서권한 + 메뉴유형별권한 + 메뉴직원권한

            var 부서코드 = result.부서코드;
            // var 권한유형코드 = result.권한코드; // 미적용
            var 직원사번 = result.사번;

            var menuList = dc.메뉴정보
                .Include(x => x.메뉴직원권한목록.Where(x => x.직원사번 == 직원사번))
                .Include(x => x.메뉴부서권한목록.Where(y => y.부서코드 == 부서코드))
                .Where_미삭제()
                .Where(x => string.IsNullOrEmpty(x.메뉴명) == false)
                .OrderBy(x => x.정렬순번)
                .ToList();

            var 메뉴직원권한목록 = new List<메뉴직원권한정보>();
            foreach (var item in menuList)
            {
                item.메뉴부서권한 = item.메뉴부서권한목록.FirstOrDefault();
                item.메뉴부서권한목록 = null;

                item.메뉴직원권한 = item.메뉴직원권한목록.FirstOrDefault();
                item.메뉴직원권한목록 = null;

                메뉴직원권한목록.Add(new 메뉴직원권한정보
                {
                    메뉴순번 = item.순번,
                    직원사번 = 직원사번,

                    // SYSTEM 설정과 상관없이 항상 모든 권한을 가진다.
                    읽기권한 = 직원사번 == "SYSTEM" ? true : (item.메뉴직원권한?.읽기권한 ?? (item.메뉴부서권한?.읽기권한 ?? false)),
                    등록권한 = 직원사번 == "SYSTEM" ? true : (item.메뉴직원권한?.등록권한 ?? (item.메뉴부서권한?.등록권한 ?? false)),
                    변경권한 = 직원사번 == "SYSTEM" ? true : (item.메뉴직원권한?.변경권한 ?? (item.메뉴부서권한?.변경권한 ?? false)),
                    삭제권한 = 직원사번 == "SYSTEM" ? true : (item.메뉴직원권한?.삭제권한 ?? (item.메뉴부서권한?.삭제권한 ?? false))
                });
            }
            result.메뉴직원권한목록 = 메뉴직원권한목록;
            // }}}
        }

        public Task<직원정보> 직원상세_조회(string userId)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.직원정보
                .Include(x => x.권한정보)
                .Where_미삭제_사용()
                .FirstOrDefault(x => x.사번 == userId);

            메뉴직원권한_반영(dc, result);

            return Task.FromResult(result);
        }

        public Task<IEnumerable<메시지정보>> 메시지_수신조회(string 사번)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.메시지정보
                .Include(x => x.발송인)
                .Include(x => x.수신인)
                .Where_미삭제()
                .Where(x => x.수신인사번 == 사번)
                .OrderBy(x => x.메시지확인유무)
                .ThenByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<메시지정보>> 메시지_송신조회(string 사번)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.메시지정보
                .Include(x => x.발송인)
                .Include(x => x.수신인)
                .Where_미삭제()
                .Where(x => x.발송인사번 == 사번)
                .OrderBy(x => x.메시지확인유무)
                .ThenByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task 메시지_송신(메시지정보 메시지)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            dc.메시지정보.Add(메시지);

            dc.SaveChanges();

            dcp.GetSubject<(string, string)>("global.message.param1").OnNext(("ReceivedMessage", 메시지.수신인사번));

            return Task.CompletedTask;
        }

        public Task 메시지_삭제(메시지정보 메시지)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            메시지.삭제유무 = true;
            dc.메시지정보.Update(메시지);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 메시지_확인(메시지정보 메시지)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            메시지.메시지확인유무 = true;
            dc.메시지정보.Update(메시지);

            dc.SaveChanges();

            dcp.GetSubject<(string, string)>("global.message.param1").OnNext(("ReceivedMessage", 메시지.수신인사번));

            return Task.CompletedTask;
        }

        public Task 메시지_전체확인(string 사번)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.메시지정보.Where(x => x.수신인사번 == 사번);
            foreach (var item in list)
            {
                item.메시지확인유무 = true;
                dc.메시지정보.Update(item);
            }

            dc.SaveChanges();

            dcp.GetSubject<(string, string)>("global.message.param1").OnNext(("ReceivedMessage", 사번));


            return Task.CompletedTask;
        }

        public Task<int> 메시지_읽지않은개수(string 사번)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var count = dc.메시지정보.Where(x => x.수신인사번 == 사번 && x.메시지확인유무 == false).Count();

            return Task.FromResult(count);
        }

        public Task PDA_메시지(string 사번)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            dcp.GetSubject<(string, string)>("global.message.qr").OnNext(("NotScan", 사번));

            return Task.CompletedTask;
        }

        // [추가] 2021-03-17
        public Task PDA_공정메시지(string 사번)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            dcp.GetSubject<(string, string)>("global.message.qr").OnNext(("NotScan", 사번));

            return Task.CompletedTask;
        }

        public Task 메시지ToPDA(string 사유, string 사번)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            dcp.GetSubject<(string, string)>("global.message.qr").OnNext((사유, 사번));

            return Task.CompletedTask;
        }

        public async Task QR_메시지(int cnt, string message_qr, string message_txt)
        {
            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            DefaultWampChannelFactory factory = new DefaultWampChannelFactory();

            IWampChannel channel =
                factory.CreateJsonChannel("ws://127.0.0.1:31200/ws", DefaultRealm);
            //host = new WampHost();
            //host.RegisterTransport(new RawSocketTransport(TcpListener.Create(DefaultPort)), new JTokenMsgpackBinding());

            await channel.Open();

            //QR Code Print 중간 집계 장소에서 Subscribe함< 예 : 콘솔 어플> 
            ISubject<(int, string, string)> qrSubject =
                channel.RealmProxy.Services.GetSubject("com.myapp.topicqr", new MyQrTupleEventConverter());
            qrSubject.OnNext((cnt, message_qr, message_txt));

            //dcp.GetSubject<(string, string)>("global.message.param1").OnNext(("ReceivedMessage", message_qr));

            //return Task.CompletedTask;
        }

        public class MyQrTupleEventConverter : WampEventValueTupleConverter<(int, string, string)>
        {
        }

        public Task<string> PrtUri()
        {
            return Task.FromResult(PrnUri);
        }

        public Task<string> Server_Uri()
        {
            return Task.FromResult(ServerUri);
        }

        public class PointCodeDictionary<TValue> : ConcurrentDictionary<string, TValue>
        where TValue : class
        {
            public new TValue this[string key]
            {
                get
                {
                    if (ContainsKey(key) == false)
                        return null;

                    return base[key];
                }
                set
                {
                    base[key] = value;
                }
            }
        }

        public Task 포인트_저장(string userid, string 품목코드, int 수량)
        {
            검사인력포인터 검사포인트 = new 검사인력포인터();
            검사포인트.사번 = userid;
            검사포인트.품목코드 = 품목코드;
            검사포인트.수량 = 수량;
            검사포인트.pointer = -1;

            //pointMap[검사포인트.사번] = 검사포인트;
            if (pointMap.ContainsKey(검사포인트.사번) == false)
            {
                pointMap[검사포인트.사번] = 검사포인트;
                if (pointListMap.ContainsKey(검사포인트.품목코드) == false)
                {
                    pointListMap[검사포인트.품목코드] = new List<검사인력포인터>();
                }

                pointListMap[검사포인트.품목코드].Add(검사포인트);
            }

            return Task.CompletedTask;
        }

        public Task 포인트_저장2(string userid, string 품목코드, int 수량, int 검사항목수)
        {
            검사인력포인터 검사포인트 = new 검사인력포인터();
            검사포인트.사번 = userid;
            검사포인트.품목코드 = 품목코드;
            검사포인트.수량 = 수량;
            검사포인트.pointer = -1;
            검사포인트.검사항목수 = 검사항목수;
            검사포인트.다음포인터 = -1;
            검사포인트.다음측정 = false;

            //pointMap[검사포인트.사번] = 검사포인트;
            if (pointMap.ContainsKey(검사포인트.사번) == false)
            {
                pointMap[검사포인트.사번] = 검사포인트;
                if (pointListMap.ContainsKey(검사포인트.품목코드) == false)
                {
                    pointListMap[검사포인트.품목코드] = new List<검사인력포인터>();
                }

                pointListMap[검사포인트.품목코드].Add(검사포인트);
            }

            return Task.CompletedTask;

        }

        public Task<int> 포인트_조회(string userid, string 품목코드, string 생산지시코드, string 회사코드)
        {
            var 품질검사목록 = pointMap[userid];//_품질관리.품질검사측정완료유무_조회(생산지시코드);
            var 검사완료목록 = _품질관리.품질검사측정완료유무_조회(생산지시코드, 회사코드);
            var nextCount = 검사완료목록.Result.GroupBy(x => x.시리얼넘버).Count() - 1;
            var 검사항목카운트 = 검사완료목록.Result.GroupBy(x => x.품질검사코드).Count();

            if (pointMap[userid].다음포인터 > -1 && pointMap[userid].다음측정 == true)
            {
                nextCount = -1;
            }
            else if (pointMap[userid].다음포인터 > -1 && pointMap[userid].다음측정 == false)
            {
                nextCount = pointMap[userid].다음포인터++;
            }

            IList<검사인력포인터> 검사포인트List;
            //pointMap[검사포인트.사번] = 검사포인트;
            if (pointMap.ContainsKey(userid) == true)
            {
                var 검사포인트 = pointMap[userid];
                var lcv = 0;
                if (검사포인트.품목코드 == 품목코드)
                {
                    검사포인트List = pointListMap[품목코드];
                    //row_select = 검사포인트.pointer;
                    foreach (var pomap in 검사포인트List)
                    {
                        //if (lcv == 0)
                        //{
                        //    nextCount = pomap.pointer;
                        //}
                        if (pomap.pointer > nextCount)
                        {
                            nextCount = pomap.pointer;
                        }

                    }
                    nextCount += 1;

                    if (nextCount > 검사포인트.수량)
                    {
                        nextCount = -1;
                    }
                    else if (nextCount == 검사포인트.수량)
                    {
                        nextCount = 0;

                    }

                    lcv = 0;
                    foreach (var pomap in pointListMap[품목코드])
                    {
                        if (pomap == 검사포인트)
                        {
                            pointListMap[품목코드][lcv].pointer = nextCount;
                            break;
                        }
                        lcv += 1;
                    }
                    pointMap[userid].다음측정 = false;
                }

                return Task.FromResult(nextCount);
            }
            else
            {
                검사포인트List = null;
                nextCount = -1;
            }

            return Task.FromResult(nextCount);

        }


        public Task<bool> 포인트_설정(string userid, string 품목코드, int row_select, int cnt)
        {
            if (pointMap.ContainsKey(userid) == true)
            {
                var 검사포인트 = pointMap[userid];
                var lcv = 0;
                // 새로운 row_select 값 
                //검사포인트.pointer = row_select;
                pointMap[userid].pointer = row_select;
                pointMap[userid].다음포인터 = cnt;
                pointMap[userid].다음측정 = true;

                lcv = 0;
                foreach (var pomap in pointListMap[품목코드])
                {
                    if (pomap == 검사포인트)
                    {
                        pointListMap[품목코드][lcv].pointer = row_select;
                        pointListMap[품목코드][lcv].다음포인터 = cnt;
                        pointListMap[품목코드][lcv].다음측정 = true;
                        break;
                    }
                    lcv += 1;
                }
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> 포인트_삭제(string userid, string 품목코드)
        {
            //pointMap[검사포인트.사번] = 검사포인트;
            if (pointMap.ContainsKey(userid) == true)
            {
                var 검사포인트 = pointMap[userid];
                if (검사포인트.품목코드 == 품목코드)
                {
                    pointListMap[품목코드].Remove(검사포인트);

                    if (pointListMap[품목코드].Count == 0)
                    {
                        pointListMap.Remove(품목코드, out IList<검사인력포인터> Value);
                    }

                    bool v = pointMap.Remove(userid, out 검사포인트);

                    return Task.FromResult(v);
                }
            }
            return Task.FromResult(false);
        }

        public Task<string> 공통코드명_조회(string 코드명)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.공통코드
                .Include(x => x.하위.Where(y => y.삭제유무 != true).OrderBy(y => y.정렬순번))
                    .ThenInclude(x => x.하위.Where(y => y.삭제유무 != true).OrderBy(y => y.정렬순번))
                .Where_미삭제()
                .Where(x => x.코드명 == 코드명)
                .OrderBy(x => x.코드유형코드).FirstOrDefault();


            return Task.FromResult(result.코드);

        }

        public Task PDA_작업지시서메시지(string 사번, List<string> 담당자들)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            dcp.GetSubject<(string, string)>("global.message.qr").OnNext(("JobOrder", 사번));

            foreach (var 담당자 in 담당자들)
            {

                dcp.GetSubject<(string, string)>("global.message.qr").OnNext(("JobOrder", 담당자));
            }

            return Task.CompletedTask;
        }







        /////////// 외주품질검사 포인트설정  20210531 ////

        public Task 외주포인트_저장(string userid, string 품목코드, int 수량)
        {
            검사인력포인터 검사포인트 = new 검사인력포인터();
            검사포인트.사번 = userid;
            검사포인트.품목코드 = 품목코드;
            검사포인트.수량 = 수량;
            검사포인트.pointer = -1;

            //pointMap[검사포인트.사번] = 검사포인트;
            if (pointMap.ContainsKey(검사포인트.사번) == false)
            {
                pointMap[검사포인트.사번] = 검사포인트;
                if (pointListMap.ContainsKey(검사포인트.품목코드) == false)
                {
                    pointListMap[검사포인트.품목코드] = new List<검사인력포인터>();
                }

                pointListMap[검사포인트.품목코드].Add(검사포인트);
            }

            return Task.CompletedTask;
        }

        public Task 외주포인트_저장2(string userid, string 품목코드, int 수량, int 검사항목수)
        {
            검사인력포인터 검사포인트 = new 검사인력포인터();
            검사포인트.사번 = userid;
            검사포인트.품목코드 = 품목코드;
            검사포인트.수량 = 수량;
            검사포인트.pointer = -1;
            검사포인트.검사항목수 = 검사항목수;
            검사포인트.다음포인터 = -1;
            검사포인트.다음측정 = false;

            //pointMap[검사포인트.사번] = 검사포인트;
            if (pointMap.ContainsKey(검사포인트.사번) == false)
            {
                pointMap[검사포인트.사번] = 검사포인트;
                if (pointListMap.ContainsKey(검사포인트.품목코드) == false)
                {
                    pointListMap[검사포인트.품목코드] = new List<검사인력포인터>();
                }

                pointListMap[검사포인트.품목코드].Add(검사포인트);
            }

            return Task.CompletedTask;

        }

        public Task<int> 외주포인트_조회(string userid, string 품목코드, string 지시번호, string 회사코드)
        {
            var 품질검사목록 = pointMap[userid];//_품질관리.품질검사측정완료유무_조회(생산지시코드);
            var 검사완료목록 = _품질관리.외주품질검사측정완료유무_조회(지시번호, 회사코드);
            var nextCount = 검사완료목록.Result.GroupBy(x => x.시리얼넘버).Count() - 1;
            var 검사항목카운트 = 검사완료목록.Result.GroupBy(x => x.품질검사코드).Count();

            if (pointMap[userid].다음포인터 > -1 && pointMap[userid].다음측정 == true)
            {
                nextCount = -1;
            }
            else if (pointMap[userid].다음포인터 > -1 && pointMap[userid].다음측정 == false)
            {
                nextCount = pointMap[userid].다음포인터++;
            }

            IList<검사인력포인터> 검사포인트List;
            //pointMap[검사포인트.사번] = 검사포인트;
            if (pointMap.ContainsKey(userid) == true)
            {
                var 검사포인트 = pointMap[userid];
                var lcv = 0;
                if (검사포인트.품목코드 == 품목코드)
                {
                    검사포인트List = pointListMap[품목코드];
                    //row_select = 검사포인트.pointer;
                    foreach (var pomap in 검사포인트List)
                    {
                        //if (lcv == 0)
                        //{
                        //    nextCount = pomap.pointer;
                        //}
                        if (pomap.pointer > nextCount)
                        {
                            nextCount = pomap.pointer;
                        }

                    }
                    nextCount += 1;

                    if (nextCount > 검사포인트.수량)
                    {
                        nextCount = -1;
                    }
                    else if (nextCount == 검사포인트.수량)
                    {
                        nextCount = 0;

                    }

                    lcv = 0;
                    foreach (var pomap in pointListMap[품목코드])
                    {
                        if (pomap == 검사포인트)
                        {
                            pointListMap[품목코드][lcv].pointer = nextCount;
                            break;
                        }
                        lcv += 1;
                    }
                    pointMap[userid].다음측정 = false;
                }

                return Task.FromResult(nextCount);
            }
            else
            {
                검사포인트List = null;
                nextCount = -1;
            }

            return Task.FromResult(nextCount);

        }

        public Task<bool> 외주포인트_설정(string userid, string 품목코드, int row_select, int cnt)
        {
            if (pointMap.ContainsKey(userid) == true)
            {
                var 검사포인트 = pointMap[userid];
                var lcv = 0;
                // 새로운 row_select 값 
                //검사포인트.pointer = row_select;
                pointMap[userid].pointer = row_select;
                pointMap[userid].다음포인터 = cnt;
                pointMap[userid].다음측정 = true;

                lcv = 0;
                foreach (var pomap in pointListMap[품목코드])
                {
                    if (pomap == 검사포인트)
                    {
                        pointListMap[품목코드][lcv].pointer = row_select;
                        pointListMap[품목코드][lcv].다음포인터 = cnt;
                        pointListMap[품목코드][lcv].다음측정 = true;
                        break;
                    }
                    lcv += 1;
                }
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> 외주포인트_삭제(string userid, string 품목코드)
        {
            //pointMap[검사포인트.사번] = 검사포인트;
            if (pointMap.ContainsKey(userid) == true)
            {
                var 검사포인트 = pointMap[userid];
                if (검사포인트.품목코드 == 품목코드)
                {
                    pointListMap[품목코드].Remove(검사포인트);

                    if (pointListMap[품목코드].Count == 0)
                    {
                        pointListMap.Remove(품목코드, out IList<검사인력포인터> Value);
                    }

                    bool v = pointMap.Remove(userid, out 검사포인트);

                    return Task.FromResult(v);
                }
            }
            return Task.FromResult(false);
        }







        ///////////////// 수입검사 /////////////
        ///


        public Task 수입검사포인트_저장(string userid, string 품목코드, int 수량)
        {
            검사인력포인터 검사포인트 = new 검사인력포인터();
            검사포인트.사번 = userid;
            검사포인트.품목코드 = 품목코드;
            검사포인트.수량 = 수량;
            검사포인트.pointer = -1;

            //pointMap[검사포인트.사번] = 검사포인트;
            if (pointMap.ContainsKey(검사포인트.사번) == false)
            {
                pointMap[검사포인트.사번] = 검사포인트;
                if (pointListMap.ContainsKey(검사포인트.품목코드) == false)
                {
                    pointListMap[검사포인트.품목코드] = new List<검사인력포인터>();
                }

                pointListMap[검사포인트.품목코드].Add(검사포인트);
            }

            return Task.CompletedTask;
        }

        public Task 수입검사포인트_저장2(string userid, string 품목코드, int 수량, int 검사항목수)
        {
            검사인력포인터 검사포인트 = new 검사인력포인터();
            검사포인트.사번 = userid;
            검사포인트.품목코드 = 품목코드;
            검사포인트.수량 = 수량;
            검사포인트.pointer = -1;
            검사포인트.검사항목수 = 검사항목수;
            검사포인트.다음포인터 = -1;
            검사포인트.다음측정 = false;

            //pointMap[검사포인트.사번] = 검사포인트;
            if (pointMap.ContainsKey(검사포인트.사번) == false)
            {
                pointMap[검사포인트.사번] = 검사포인트;
                if (pointListMap.ContainsKey(검사포인트.품목코드) == false)
                {
                    pointListMap[검사포인트.품목코드] = new List<검사인력포인터>();
                }

                pointListMap[검사포인트.품목코드].Add(검사포인트);
            }

            return Task.CompletedTask;

        }

        public Task<int> 수입검사포인트_조회(string userid, string 품목코드, string 발주번호, decimal 발주순번, string 회사코드)
        {
            var 품질검사목록 = pointMap[userid];//_품질관리.품질검사측정완료유무_조회(생산지시코드);
            var 검사완료목록 = _품질관리.발주서별품질검사측정완료유무_조회(발주번호,발주순번, 회사코드);
            var nextCount = 검사완료목록.Result.GroupBy(x => x.시리얼넘버).Count() - 1;
            var 검사항목카운트 = 검사완료목록.Result.GroupBy(x => x.품질검사코드).Count();

            if (pointMap[userid].다음포인터 > -1 && pointMap[userid].다음측정 == true)
            {
                nextCount = -1;
            }
            else if (pointMap[userid].다음포인터 > -1 && pointMap[userid].다음측정 == false)
            {
                nextCount = pointMap[userid].다음포인터++;
            }

            IList<검사인력포인터> 검사포인트List;
            //pointMap[검사포인트.사번] = 검사포인트;
            if (pointMap.ContainsKey(userid) == true)
            {
                var 검사포인트 = pointMap[userid];
                var lcv = 0;
                if (검사포인트.품목코드 == 품목코드)
                {
                    검사포인트List = pointListMap[품목코드];
                    //row_select = 검사포인트.pointer;
                    foreach (var pomap in 검사포인트List)
                    {
                        //if (lcv == 0)
                        //{
                        //    nextCount = pomap.pointer;
                        //}
                        if (pomap.pointer > nextCount)
                        {
                            nextCount = pomap.pointer;
                        }

                    }
                    nextCount += 1;

                    if (nextCount > 검사포인트.수량)
                    {
                        nextCount = -1;
                    }
                    else if (nextCount == 검사포인트.수량)
                    {
                        nextCount = 0;

                    }

                    lcv = 0;
                    foreach (var pomap in pointListMap[품목코드])
                    {
                        if (pomap == 검사포인트)
                        {
                            pointListMap[품목코드][lcv].pointer = nextCount;
                            break;
                        }
                        lcv += 1;
                    }
                    pointMap[userid].다음측정 = false;
                }

                return Task.FromResult(nextCount);
            }
            else
            {
                검사포인트List = null;
                nextCount = -1;
            }

            return Task.FromResult(nextCount);

        }

        public Task<bool> 수입검사포인트_설정(string userid, string 품목코드, int row_select, int cnt)
        {
            if (pointMap.ContainsKey(userid) == true)
            {
                var 검사포인트 = pointMap[userid];
                var lcv = 0;
                // 새로운 row_select 값 
                //검사포인트.pointer = row_select;
                pointMap[userid].pointer = row_select;
                pointMap[userid].다음포인터 = cnt;
                pointMap[userid].다음측정 = true;

                lcv = 0;
                foreach (var pomap in pointListMap[품목코드])
                {
                    if (pomap == 검사포인트)
                    {
                        pointListMap[품목코드][lcv].pointer = row_select;
                        pointListMap[품목코드][lcv].다음포인터 = cnt;
                        pointListMap[품목코드][lcv].다음측정 = true;
                        break;
                    }
                    lcv += 1;
                }
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> 수입검사포인트_삭제(string userid, string 품목코드)
        {
            //pointMap[검사포인트.사번] = 검사포인트;
            if (pointMap.ContainsKey(userid) == true)
            {
                var 검사포인트 = pointMap[userid];
                if (검사포인트.품목코드 == 품목코드)
                {
                    pointListMap[품목코드].Remove(검사포인트);

                    if (pointListMap[품목코드].Count == 0)
                    {
                        pointListMap.Remove(품목코드, out IList<검사인력포인터> Value);
                    }

                    bool v = pointMap.Remove(userid, out 검사포인트);

                    return Task.FromResult(v);
                }
            }
            return Task.FromResult(false);
        }


    }
}
