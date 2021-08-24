using HiSFS.Api.Host.Data;
using HiSFS.Api.Host.Services;
using HiSFS.Api.Shared.Services;
using HiSFS.Shared.Wamp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Binding;
using WampSharp.RawSocket;
using WampSharp.V2;
using WampSharp.V2.Binding;
using WampSharp.V2.Realm;

namespace HiSFS.Api.Host
{
    /// <summary>
    /// WAMP  프로토콜 기반 API 호스팅 서비스
    /// 
    /// 노출되는 API는 동적으로 읽어와 등록된 API로 구성된다.
    /// </summary>
    public class WampApiService : IHostedService, IContextProvider
    {
        // TODO: 포트 등 이후 환경설정으로 변경해야 한다.
        private static readonly int DefaultPort = 31200;
        private static readonly string DefaultServiceUri = $"ws://0.0.0.0:{DefaultPort}/";
        private static readonly string DefaultRealm = "HiSFS.Api";


        private readonly ILogger<WampApiService> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IConfiguration config;

        private IWampHost host;
        private IWampHostedRealm realm;


        public WampApiService(IServiceScopeFactory serviceScopeFactory, ILogger<WampApiService> logger, IConfiguration config)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
            this.config = config;
        }

        public ISubject<T> GetSubject<T>(string messageId)
        {
            return realm.Services.GetSubject<T>(messageId);
        }

        /// <summary>
        /// 서비스를 시작한다.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });

            host = new DefaultWampHost(DefaultServiceUri,
                new List<IWampBinding>
                {
                    new JTokenJsonBinding(jsonSerializer),
                    new JTokenMsgpackBinding(jsonSerializer)
                });
            //host = new WampHost();
            //host.RegisterTransport(new RawSocketTransport(TcpListener.Create(DefaultPort)), new JTokenMsgpackBinding());

            host.Open();

            realm = host.RealmContainer.GetRealmByName(DefaultRealm);
           

            RegistApiServices();

            logger.LogInformation("API 서비스 시작됨");

            //qrSubject = realm.Services.GetSubject("global.message.qr1", new MyQrTupleEventConverter());

            return Task.CompletedTask;

            // ------
            void RegistApiServices()
            {
                
                var 자재관리 = new 자재관리서비스(this as IContextProvider);
                var 품질관리 = new 품질관리서비스(this as IContextProvider);
                var 공통관리 = new 공통서비스(this as IContextProvider, config, 품질관리);
                //2021.04.30
                var 기준관리 = new 기준정보서비스(this as IContextProvider);

                realm.Services.RegisterCallee(new 공통서비스(this as IContextProvider, config, 품질관리), new PrefixCalleeRegistrationInterceptor(nameof(I공통서비스)));
                //var 공통관리 = new 공통서비스(this as IContextProvider, config, 품질관리);
                realm.Services.RegisterCallee(new 기준정보서비스(this as IContextProvider), new PrefixCalleeRegistrationInterceptor(nameof(I기준정보서비스)));
                //var 자재관리 = new 자재관리서비스(this as IContextProvider);
                realm.Services.RegisterCallee(자재관리, new PrefixCalleeRegistrationInterceptor(nameof(I자재관리서비스)));
                //var 품질관리 = new 품질관리서비스(this as IContextProvider);
                realm.Services.RegisterCallee(품질관리, new PrefixCalleeRegistrationInterceptor(nameof(I품질관리서비스)));
                realm.Services.RegisterCallee(new 생산관리서비스(this as IContextProvider), new PrefixCalleeRegistrationInterceptor(nameof(I생산관리서비스)));
                realm.Services.RegisterCallee(new 시스템서비스(this as IContextProvider), new PrefixCalleeRegistrationInterceptor(nameof(I시스템서비스)));
                realm.Services.RegisterCallee(new 에이전트서비스(this as IContextProvider, config), new PrefixCalleeRegistrationInterceptor(nameof(I에이전트서비스)));
                realm.Services.RegisterCallee(new 액션서비스(this as IContextProvider, 자재관리, 품질관리, 공통관리,기준관리), new PrefixCalleeRegistrationInterceptor(nameof(I액션서비스)));
            }
        }

        /// <summary>
        /// 서비스를 종료한다.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            host.Dispose();

            logger.LogInformation("API 서비스 중지됨");

            return Task.CompletedTask;
        }

        DbContextScope<ApiDbContext> IContextProvider.GetDbContextScope()
        {
            var scope = serviceScopeFactory.CreateScope();
            return new DbContextScope<ApiDbContext>(scope, config);
        }
        DbContextScope<ApiDbDZICUBEContext> IContextProvider.GetDbContextScopeDZ()
        {
            var scope = serviceScopeFactory.CreateScope();
            return new DbContextScope<ApiDbDZICUBEContext>(scope, config);
        }
    }

    public class DbContextScope<TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        private readonly IServiceScope scope;

        public IConfiguration Config { get; }

        public DbContextScope(IServiceScope scope, IConfiguration config)
        {
            this.scope = scope;
            this.Config = config;
        }

        public TDbContext DbContext
        {
            get
            {
                var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                return context;
            }
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }

    

    public interface IContextProvider
    {
        DbContextScope<ApiDbContext> GetDbContextScope();

        DbContextScope<ApiDbDZICUBEContext> GetDbContextScopeDZ();


        ISubject<T> GetSubject<T>(string messageId);
 
    }
}
