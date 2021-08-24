using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;

using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WampSharp.V2;
using WampSharp.V2.Fluent;

namespace HiSFS.Api.Host.Services
{
    public class 에이전트서비스 : I에이전트서비스
    {
        private readonly IContextProvider dcp;
        private readonly IConfiguration config;

        public string ServerUri;

        public 에이전트서비스(IContextProvider dbContextProvider, IConfiguration config)
        {
            this.dcp = dbContextProvider;
            this.config = config;
            var section = config.GetSection("RemoteApi");
            //var port = int.Parse(section["Port"]);
            //PrnUri = section["PrnUri"];
            ServerUri = section["ServerUri"];
        }

        public Task 설비가동현황_저장(설비가동현황정보 info)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            _설비가동현황_저장(info, dc);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        private void _설비가동현황_저장(설비가동현황정보 info, Data.ApiDbContext dc)
        {
            var isAdd = false;

            // track 방지 {{{
            info.설비 = null;
            info.상태유형 = null;
            // }}}

            var entity = dc.설비가동현황정보.FirstOrDefault(x => x.코드 == info.코드);
            if (entity == default)
            {
                isAdd = true;
                entity = info;
                entity.상태변경시각 = DateTime.Now;
            }

            if (entity.상태 != info.상태)
            {
                entity.이전상태 = entity.상태;
                entity.상태 = info.상태;
                entity.상태변경시각 = DateTime.Now;

                // 액션로그가 기록되면 메시지를 발송한다.
                dcp.GetSubject<string>("global.message").OnNext("ChangedMachineState");
            }

            entity.상태유지시간 = DateTime.Now - entity.상태변경시각;

            if (isAdd == true)
                dc.설비가동현황정보.Add(entity);
            else
                dc.설비가동현황정보.Update(entity);
        }

        public Task 설비가동현황_저장(IEnumerable<설비가동현황정보> list)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            foreach (var info in list)
                _설비가동현황_저장(info, dc);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<설비가동현황정보>> 설비가동현황_조회()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.설비가동현황정보
                .Include(x => x.설비)
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        /// <summary>
        /// 디지털미터의 값을 서버로 전송한다. 이때, 연동장비관리에 등록되어 있지 않는 경우 등록한다.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public Task 디지털미터_전송(string id, decimal value, string unit)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var info = dc.연동장비정보.FirstOrDefault(x => x.식별코드 == id);
            if (info == default)
            {
                info = new 연동장비정보
                {
                    식별코드 = id,
                    장비명 = $"디지털미터-{id}",
                    에이전트명 = null,
                    연동장비유형코드 = "S8103",
                    등록시각 = DateTime.Now,
                    사용유무 = false
                };

                dc.연동장비정보.Add(info);
                dc.SaveChanges();
            }

            info.테스트 = $"{value} {unit}";
            dc.연동장비정보.Update(info);
            dc.SaveChanges();

            Task task = Mitutoyo_메시지(id, value);

            dcp.GetSubject<string>("global.message").OnNext("ReceivedTest");

            //task.Dispose();

            return Task.CompletedTask;
        }

        private static readonly string DefaultRealm = "HiSFS.Api";

        private async Task Mitutoyo_메시지(string id, decimal value)
        {
            var jsonSerializer = Newtonsoft.Json.JsonSerializer.Create(new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });

            var server_Uri = await ServerAdd_Uri();

            DefaultWampChannelFactory factory = new DefaultWampChannelFactory();

            //IWampChannel channel = factory.CreateJsonChannel("ws://127.0.0.1:31200/ws", DefaultRealm);
            IWampChannel channel = factory.CreateJsonChannel(server_Uri, DefaultRealm);
            await channel.Open().ConfigureAwait(false); 

            // 캘리퍼스 데이터 
            ISubject<(string, decimal)> qrSubject =
                channel.RealmProxy.Services.GetSubject(id, new MyMitutoyoTupleEventConverter());
            qrSubject.OnNext((id, value));

            //return Task.CompletedTask;
        }

        public class MyMitutoyoTupleEventConverter : WampEventValueTupleConverter<(string, decimal)>
        {
        }

        private Task<string> ServerAdd_Uri()
        {
            return Task.FromResult(ServerUri);
        }
    }
}
