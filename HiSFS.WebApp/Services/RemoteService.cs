using HiSFS.Api.Shared.Services;
using HiSFS.Shared.Wamp;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Fluent;

namespace HiSFS.WebApp.Services
{
    /// <summary>
    /// API Host와 통신하여 원격 API 기능을 제공하는 서비스
    /// </summary>
    public class RemoteService : IDisposable
    {
        private readonly IConfiguration config;
        private readonly RemoteCommand command;
        private readonly WampChannelReconnector reconnector;
        private readonly ManualResetEvent readyResetEvent = new ManualResetEvent(false);
        
        private IWampRealmProxy realm;

        private bool isReady;

        public event EventHandler<RemoteServiceReadyEventArgs> ReadyEvent;

        public IRemoteCommand Command => command;


        /// <summary>
        /// 원격 서비스가 준비된 상태인지의 유무
        /// </summary>
        public bool IsReady
        {
            get => isReady;
            private set
            {
                if (value == isReady)
                    return;

                isReady = value;

                try
                {
                    ReadyEvent?.Invoke(this, IsReady == true ? RemoteServiceReadyEventArgs.ReadyEventArgs : RemoteServiceReadyEventArgs.UnreadyEventArgs);
                }
                catch
                {
                }
            }
        }

        public RemoteService(IConfiguration config)
        {
            this.config = config;

            var section = config.GetSection("RemoteApi");
            //var port = int.Parse(section["Port"]);
            var uri = section["Uri"];

            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            var channel = new WampChannelFactory()
                .ConnectToRealm("HiSFS.Api")
                .WebSocketTransport(new Uri(uri))
                .MsgpackSerialization(jsonSerializer)
                .Build();
            //var channel = new DefaultWampChannelFactory(). .CreateMsgpackChannel(uri, "HiSFS.Api");

            command = new RemoteCommand();

            reconnector = new WampChannelReconnector(channel, async () =>
            {
                readyResetEvent.Reset();
                IsReady = false;

                await channel.Open();
                realm = channel.RealmProxy;

                var 공통서비스 = channel.RealmProxy.Services.GetCalleeProxy<I공통서비스>(new PrefixCalleeProxyInterceptor(nameof(I공통서비스)));
                (command as RemoteCommand).공통 = 공통서비스;
                var 기준정보서비스 = channel.RealmProxy.Services.GetCalleeProxy<I기준정보서비스>(new PrefixCalleeProxyInterceptor(nameof(I기준정보서비스)));
                (command as RemoteCommand).기준정보 = 기준정보서비스;
                var 자재관리서비스 = channel.RealmProxy.Services.GetCalleeProxy<I자재관리서비스>(new PrefixCalleeProxyInterceptor(nameof(I자재관리서비스)));
                (command as RemoteCommand).자재관리 = 자재관리서비스;
                var 품질관리서비스 = channel.RealmProxy.Services.GetCalleeProxy<I품질관리서비스>(new PrefixCalleeProxyInterceptor(nameof(I품질관리서비스)));
                (command as RemoteCommand).품질관리 = 품질관리서비스;
                var 생산관리서비스 = channel.RealmProxy.Services.GetCalleeProxy<I생산관리서비스>(new PrefixCalleeProxyInterceptor(nameof(I생산관리서비스)));
                (command as RemoteCommand).생산관리 = 생산관리서비스;
                var 시스템서비스 = channel.RealmProxy.Services.GetCalleeProxy<I시스템서비스>(new PrefixCalleeProxyInterceptor(nameof(I시스템서비스)));
                (command as RemoteCommand).시스템 = 시스템서비스;
                var 에이전트서비스 = channel.RealmProxy.Services.GetCalleeProxy<I에이전트서비스>(new PrefixCalleeProxyInterceptor(nameof(I에이전트서비스)));
                (command as RemoteCommand).에이전트 = 에이전트서비스;


                IsReady = true;
                readyResetEvent.Set();
            });
            reconnector.Start();

            //var channel = new WampChannelFactory();
            //var channel = channelFactory.ConnectToRealm("HiSFS.Api")
            //    .RawSocketTransport("127.0.0.1", port)
            //    //.AutoPing(TimeSpan.FromSeconds(5))
            //    .MsgpackSerialization()
            //    .Build();
        }

        public ISubject<T> GetSubject<T>(string messageId)
        {
            return realm.Services.GetSubject<T>(messageId);
        }

        public Task<bool> WaitForReadyRemoteService(int ms = -1)
        {
            if (IsReady == true)
                return Task.FromResult(true);

            return Task.Run(() =>
            {
                return readyResetEvent.WaitOne(ms);
            });
        }

        public void Dispose()
        {
            IsReady = false;
            reconnector.Dispose();
        }
    }

    public class RemoteServiceReadyEventArgs : EventArgs
    {
        public bool IsReady { get; }

        public RemoteServiceReadyEventArgs(bool isReady)
        {
            this.IsReady = isReady;
        }

        public static readonly RemoteServiceReadyEventArgs ReadyEventArgs = new RemoteServiceReadyEventArgs(true);
        public static readonly RemoteServiceReadyEventArgs UnreadyEventArgs = new RemoteServiceReadyEventArgs(false);
    }

    public interface IRemoteCommand
    {
        I공통서비스 공통 { get; }
        I기준정보서비스 기준정보 { get; }
        I자재관리서비스 자재관리 { get; }
        I품질관리서비스 품질관리 { get; }
        I생산관리서비스 생산관리 { get; }
        I시스템서비스 시스템 { get; }
        I에이전트서비스 에이전트 { get; }
    }

    public class RemoteCommand : IRemoteCommand
    {
        public I공통서비스 공통 { get; set; }
        public I기준정보서비스 기준정보 { get; set; }
        public I자재관리서비스 자재관리 { get; set; }
        public I품질관리서비스 품질관리 { get; set; }
        public I생산관리서비스 생산관리 { get; set; }
        public I시스템서비스 시스템 { get; set; }
        public I에이전트서비스 에이전트 { get; set; }

        public RemoteCommand()
        {
        }
    }
}