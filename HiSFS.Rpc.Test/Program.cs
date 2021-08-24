using HiSFS.Rpc.Shared;
using System;

namespace HiSFS.Rpc.Test
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            using var client = RpcClient.Create("localhost", 31202, "realm");

            client.Start();

            await client.WaitForConnected();

            client.GetRealm().Service.GetSubject<bool>("event.message").OnNext(true);

            Console.Write("ENTER를 누르면 종료합니다.");
            Console.ReadLine();

            await client.WaitForConnected();

            client.GetRealm().Service.GetSubject<bool>("event.message").OnNext(true);
        }
    }
}
