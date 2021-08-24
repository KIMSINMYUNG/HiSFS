using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using HiSFS.Agent.Service.Devices;
using HiSFS.Api.Shared.Client;
using HiSFS.Api.Shared.Models;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace HiSFS.Agent.Service.Drivers
{
    /// <summary>
    /// MT-LINKi 실시간 데이터 수집기 드라이버
    /// </summary>
    public class MTLINKiReceiverDriver : IDriver
    {
        public IDevice Create(string connectionString)
        {
            var result = new MTLINKiReceiver();
            result.ConnectionString = connectionString;
            return result;
        }
    }


    public class MTLINKiReceiver : IRemotableDevice
    {
        private MongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<L1_Pool_Opened> L1_Pool_Opened_Collection { get; set; }
        private CancellationTokenSource receiveLoopTs;
        

        public bool IsConnected { get; private set; }

        public string DeviceName => "MT-LINKi Realtime Receiver";

        public string ConnectionString { get; set; }
        public IRemoteCommand Command { private get; set; }
        public bool IsRemoteReady { private get; set; }

        public MTLINKiReceiver()
        {
        }

        private async Task ReceiveLoopAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested == true)
                return;

            while (token.IsCancellationRequested == false)
            {
                try
                {
                    var list = (await L1_Pool_Opened_Collection.FindAsync(x => true)).ToList();
                    //var list = new List<L1_Pool_Opened>
                    //{
                    //    new L1_Pool_Opened { Name = "CNC_1", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "CNC_2", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "CNC_3", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "CNC_4", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "CNC_5", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "CNC_6", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "CNC_7", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "CNC_8", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "MCT_1", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "MCT_2", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "MCT_4", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "MCT_5", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "MCT_6", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "MCT_7", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "MCT_8", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "ROBO_DR_1", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "ROBO_DR_2", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "ROBO_DR_3", Value = "DISCONNECT" },
                    //    new L1_Pool_Opened { Name = "ROBO_DR_4", Value = "DISCONNECT" },
                    //};

                    // 원격 사용가능일 때만 전송한다.
                    if (IsRemoteReady == true)
                    {
                        var nlist = new List<설비가동현황정보>();
                        foreach (var item in list)
                        {
                            nlist.Add(new 설비가동현황정보
                            {
                                코드 = item.Name,
                                상태 = item.Value
                            });
                        }

                        await Command.에이전트.설비가동현황_저장(nlist);
                    }

                    //foreach (var item in list)
                    //{
                    //    Console.WriteLine($"{item.Name} : {item.Value}");
                    //}
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{DeviceName} : {e.Message}");
                }

                await Task.Delay(1000, token);
            }
        }

        public void Open()
        {
            if (IsConnected == true)
                return;

            client = new MongoClient(ConnectionString);
            database = client.GetDatabase("MTLINKi");

            L1_Pool_Opened_Collection = database.GetCollection<L1_Pool_Opened>("L1_Pool_Opened");

            receiveLoopTs = new CancellationTokenSource();
            _ = ReceiveLoopAsync(receiveLoopTs.Token);
        }

        public void Close()
        {
            if (IsConnected == false)
                return;

            receiveLoopTs.Cancel();

            IsConnected = false;
        }

        public void Dispose()
        {
            Close();
        }
    }

    public class L1_Pool_Opened
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("L1Name")]
        public string Name { get; set; }
        [BsonElement("updatedate")]
        public DateTime UpdateDate { get; set; }
        [BsonElement("enddate")]
        public DateTime? EndDate { get; set; }
        [BsonElement("timespan")]
        public TimeSpan TimeSpan { get; set; }
        [BsonElement("signalname")]
        public string SignalName { get; set; }
        [BsonElement("value")]
        public string Value { get; set; }
        [BsonElement("filter")]
        public string Filter { get; set; }
        [BsonElement("TypeID")]
        public string TypeId { get; set; }
        [BsonElement("Judge")]
        public string Judge { get; set; }
        [BsonElement("Error")]
        public string Error { get; set; }
        [BsonElement("Warning")]
        public string Warning { get; set; }
    }
}
