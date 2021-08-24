using HiSFS.Rpc.Shared.Realm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.PlatformServices;
using System.Text;
using System.Threading.Tasks;
using WampSharp.Binding;
using WampSharp.RawSocket;
using WampSharp.V2;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Client;
using WampSharp.V2.Fluent;

namespace HiSFS.Rpc.Router
{
    /// <summary>
    /// HiSFS RPC Router
    /// 
    /// # 개요
    /// - RPC Router는 WAMP 프로토콜을 이용해서 구독과 호출 방식의 메시지 처리를 한다.
    ///   https://wamp-proto.org/routing.html
    /// - WebSocket을 기반으로 하나, Raw Socket도 사용 가능하다.
    /// - 프로그래밍 언어에 종속적이지 않으며이는 타 프로그래밍 언어 (Java, JavaScript 등)으로 구현된 이기종 시스템과의 연동을 위해 필요하다.
    /// - 구독 방식은 게시자(Publisher)와 구독자(Subscriber) 형태로 메시지를 송/수신 하며 1:1 및 1:N 의 메시지 송출을 할 수 있다.
    /// - 호출 방식은 호출자(Caller)와 호출대상자(Callee)로 구성되며 1:1의 메시지 송/수신을 하며 결과 값도 호출자가 획득할 수 있다.
    ///    ※ WebAPI 대신 WAMP 프로토콜로 API 구현을 대체한다.
    ///    ※ WAMP 프로토콜을 이용해 파일 송/수신도 가능할지를 확인해보자. WAMP으로 파일 송/수신을 할 경우 어떤 이점이 있을지 생각해보자.
    /// 
    /// # 기능
    /// - 구독과 호출 방식의 등록
    /// - 메시지 송출 및 함수 호출시 원격 로그 기록 (모니터링)
    /// - RPC Router 오류 발생시 재시작 (재시작시 재등록 요청)
    /// - (1차 미구현) 분산 처리
    /// - (후순위) 
    /// </summary>
    public sealed class RpcRouteHost : IDisposable
    {
        private static readonly int DefaultPort = 31202;
        private static IWampHost CreateDefaultHost() => new WampHost();
        private static IWampTransport DefaultTransport => new RawSocketTransport(TcpListener.Create(DefaultPort));
        private static IWampBinding DefaultBinding => new JTokenMsgpackBinding();


        public static RpcRouteHost Default => new RpcRouteHost();


        private bool disposedValue;
        private IWampHost? host;


        private RpcRouteHost()
        {
        }

        public void Start()
        {
            if (disposedValue == true)
                throw new ObjectDisposedException(nameof(RpcRouteHost));

            if (host != null)
                host.Dispose();

            host = CreateDefaultHost();
            host.RegisterTransport(DefaultTransport, DefaultBinding);

            host.Open();
        }


        public IRealm GetRealm(string realmName)
        {
            if (disposedValue == true)
                throw new ObjectDisposedException(nameof(RpcRouteHost));

            if (host == null)
                throw new NullReferenceException();

            var hostedRealm = host.RealmContainer.GetRealmByName(realmName);
            var realmService = new RealmService(hostedRealm.Services);
            
            return new Realm(realmService);
        }


        public void Stop()
        {
            if (disposedValue == true)
                throw new ObjectDisposedException(nameof(RpcRouteHost));

            if (host == null)
                return;


            host.Dispose();
            host = null;
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    Stop();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: 큰 필드를 null로 설정합니다.

                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RpcRouteHost()
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
    }
}
