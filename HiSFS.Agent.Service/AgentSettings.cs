using HiSFS.Shared.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Agent.Service
{
    public class AgentSettings : Settings<AgentSettings>
    {
        /// <summary>
        /// 에이전트 고유ID
        /// 
        /// UUID가 없을 경우 최초 생성된다.
        /// </summary>
        public string Uuid { get; set; }

        public string RouterUri { get; set; }

        public string ZebraUri { get; set; }

        /// <summary>
        /// 연동 장비 목록
        /// 
        /// 복수개의 장비를 연동할 수 있다.
        /// </summary>
        public ICollection<DeviceInfo> Devices { get; set; }


        protected override void SetDefault()
        {
            RouterUri = "ws://iljinjustem.maum.in:31200/";
            Uuid = Guid.NewGuid().ToString();

            Devices = new List<DeviceInfo>
            {
                // U-WAVE용 드라이버 정보
                new DeviceInfo
                {
                    Uuid = Guid.NewGuid().ToString(),
                    DeviceName = "Mitutoyo U-WAVE",
                    DeviceDriver = "HiSFS.Agent.Service.Drivers.MitutoyoUWaveDriver",
                    ConnectionString = "Port=COM4",
                    IsUse = false
                },

                // MT-LINKi 리시버 드라이버 정보
                new DeviceInfo
                {
                    Uuid = Guid.NewGuid().ToString(),
                    DeviceName = "MT-LINKi Receiver",
                    DeviceDriver = "HiSFS.Agent.Service.Drivers.MTLINKiReceiverDriver",
                    ConnectionString = "mongodb://ILJIN:ILJIN@localhost/MTLINKi",
                    IsUse = true
                }
            };
        }

        /// <summary>
        /// 연동 장비 정보
        /// </summary>
        public class DeviceInfo
        {
            /// <summary>
            /// 연동 장비 고유ID
            /// 
            /// UUID가 없을 경우 최초 생성된다.
            /// </summary>
            public string Uuid { get; set; }
            public string DeviceName { get; set; }

            /// <summary>
            /// 연동장비 드라이버
            /// </summary>
            public string DeviceDriver { get; set; }

            /// <summary>
            /// 연결 문자
            /// </summary>
            public string ConnectionString { get; set; }

            public bool IsUse { get; set; }
        }
    }

    public static class Settings
    {
        public static string RootPath => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public static string SettingsPath => Path.Combine(RootPath, "Settings");

        public static string AgentSettingsPath => Path.Combine(SettingsPath, "Agent.Settings");
    }
}
