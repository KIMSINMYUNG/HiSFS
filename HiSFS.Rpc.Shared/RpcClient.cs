using HiSFS.Rpc.Shared.Realm;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Fluent;

namespace HiSFS.Rpc.Shared
{
    public sealed class RpcClient : IDisposable
    {
        private bool disposedValue;


        private readonly IWampChannel channel;
        private WampChannelReconnector reconnector;
        private ManualResetEventSlim channelOpenedEvent = new ManualResetEventSlim(false);


        private RpcClient(string ipAddress, int port, string realmName)
        {
            channel = new WampChannelFactory().ConnectToRealm(realmName)
                .RawSocketTransport(ipAddress, port)
                .AutoPing(TimeSpan.FromSeconds(30))
                .MsgpackSerialization()
                .Build();
        }

        public void Start()
        {
            if (disposedValue == true)
                throw new ObjectDisposedException(nameof(RpcClient));

            if (reconnector != null)
                return;

            reconnector = new WampChannelReconnector(channel, async () =>
            {
                channelOpenedEvent.Reset();

                await channel.Open();

                channelOpenedEvent.Set();
            });
            reconnector.Start();
        }

        public Task WaitForConnected()
        {
            return Task.Run(() => channelOpenedEvent.Wait());
        }

        public IRealm GetRealm()
        {
            if (disposedValue == true)
                throw new ObjectDisposedException(nameof(RpcClient));

            var realm = channel.RealmProxy;
            var realmService = new RealmService(realm.Services);

            return new Realm.Realm(realmService);
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    reconnector.Dispose();
                    channel.Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RpcClient()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public static RpcClient Create(string ipAddress, int port, string realmName)
        {
            return new RpcClient(ipAddress, port, realmName);
        }
    }
}
