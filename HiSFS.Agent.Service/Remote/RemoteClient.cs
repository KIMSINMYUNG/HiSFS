using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using HiSFS.Api.Shared.Services;
using HiSFS.Shared.Wamp;

using Newtonsoft.Json;

using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Fluent;

namespace HiSFS.Api.Shared.Client
{
    public class RemoteClient
    {
        private RemoteCommand command;
        private WampChannelReconnector reconnector;
        private ManualResetEvent readyResetEvent = new ManualResetEvent(false);

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

        public void Start(string uri)
        {
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

            command = new RemoteCommand();

            reconnector = new WampChannelReconnector(channel, async () =>
            {
                readyResetEvent.Reset();
                IsReady = false;

                await channel.Open();
                realm = channel.RealmProxy;

                var 에이전트서비스 = channel.RealmProxy.Services.GetCalleeProxy<I에이전트서비스>(new PrefixCalleeProxyInterceptor(nameof(I에이전트서비스)));
                (command as RemoteCommand).에이전트 = 에이전트서비스;

                IsReady = true;
                readyResetEvent.Set();
            });
            reconnector.Start();
        }

        public void Stop()
        {
            reconnector.Dispose();
        }
    }

    public interface IRemoteCommand
    {
        I에이전트서비스 에이전트 { get; }
    }

    public class RemoteCommand : IRemoteCommand
    {
        public I에이전트서비스 에이전트 { get; set; }

        public RemoteCommand()
        {
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
}