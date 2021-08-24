using Serilog;
using Serilog.Core;
using System;
using System.Runtime.CompilerServices;


namespace HiSFS.Rpc.Router
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            Log.Debug("!!");

            using var host = RpcRouteHost.Default;

            Console.WriteLine("시작중...");

            host.Start();

            var realm = host.GetRealm("realm");
            var subject = realm.Service.GetSubject<bool>("event.message")
                .Subscribe(x => Log.Debug($"{x}"));

            Console.Write("ENTER를 누르면 종료합니다.");
            Console.ReadLine();

            subject.Dispose();
        }
    }
}
