using System;
using System.Threading.Tasks;
using Topshelf;

namespace HiSFS.Agent.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var agentSettings = await AgentSettings.LoadAsync(Settings.AgentSettingsPath);
            await agentSettings.SaveAsync();

            var rc = HostFactory.Run(x =>
            {
                x.Service<AgentService>(s =>
                {
                    // 생성
                    s.ConstructUsing(name => new AgentService(agentSettings));

                    // 시작
                    s.WhenStarted(tc => tc.Start());
                    // 종료
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("HiSFS Device Agent Service");
                x.SetDisplayName("HiSFS Device Agent Service");
                x.SetServiceName("HiSFSDeviceAgent");
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
