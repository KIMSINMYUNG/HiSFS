using HiSFS.Agent.Service.Devices;
using HiSFS.Agent.Service.Drivers;
using HiSFS.Api.Shared.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HiSFS.Agent.Service
{
    /// <summary>
    /// 백그라운드에서 동작하는 에이전트 서비스
    /// 
    /// 바코드 프린터 및 디지털미터 등 연동 가능한 장비와 통신할 수 있게 하기 위한 서비스
    /// </summary>
    public class AgentService
    {
        private AgentSettings config;
        private RemoteClient remote;

        private IList<IDevice> devices = new List<IDevice>();


        public AgentService(AgentSettings config)
        {
            this.config = config;
        }


        public void Start()
        {
            remote = new RemoteClient();
            remote.ReadyEvent += Remote_ReadyEvent;
            remote.Start(config.RouterUri);

            foreach (var deviceInfo in config.Devices)
            {
                if (deviceInfo.IsUse == false)
                    continue;

                var strDriver = deviceInfo.DeviceDriver;
                var driver = Activator.CreateInstance(Type.GetType(strDriver)) as IDriver;

                var device = driver.Create(deviceInfo.ConnectionString);
                if (device is IRemotableDevice rd)
                    rd.Command = remote.Command;
                if (device is IUWave uwave)
                {
                    if (uwave.IsPressedButtonEventSupported == true)
                    {
                        // 버튼이 눌려졌을 때 서버로 전송하도록 한다.
                        uwave.PressedButtonEvent += (s, e) =>
                        {
                            // 눌려진 값을 서버로 전송한다.
                            Console.WriteLine($"{e.Id} : {e.Value} {e.Unit}");

                            remote.Command.에이전트.디지털미터_전송(e.Id.ToString(), e.Value, e.Unit.ToString());
                        };
                    }
                }

                devices.Add(device);

                try
                {
                    Log(deviceInfo.DeviceName, "opening...");
                    device.Open();
                    Log(deviceInfo.DeviceName, "opened.");
                }
                catch (Exception e)
                {
                    Log(deviceInfo.DeviceName, $"exception : {e.Message}");
                }
            }
        }

        private void Remote_ReadyEvent(object sender, RemoteServiceReadyEventArgs e)
        {
            foreach (var device in devices)
            {
                if (device is IRemotableDevice rd)
                    rd.IsRemoteReady = e.IsReady;
            }
        }

        public void Stop()
        {
            foreach (var device in devices)
                device.Dispose();
            
            remote.ReadyEvent -= Remote_ReadyEvent;
            remote.Stop();
        }

        public void Log(string deviceName, string message)
        {
            Console.WriteLine($"{deviceName} : {message}");
        }
    }
}
