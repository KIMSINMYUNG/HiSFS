using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HiSFS.Agent.Service.Devices
{
    public class MitutoyoUWave : IUWave
    {
        public bool IsPressedButtonEventSupported => true;
        public bool IsGetValueSupported => false;
        public string DeviceName => "Mitutoyo U-WAVE";

        public string ConnectionString { get; set; }
        public bool IsConnected { get; private set; }


        public event EventHandler<PressedButtonEventArgs> PressedButtonEvent;


        private SerialPort s;
        private CancellationTokenSource cts;
        private Task receivedTask;


        public void Close()
        {
            if (IsConnected == false)
                return;

            Dispose();

            IsConnected = false;
        }

        public void Dispose()
        {
            cts.Cancel();
            receivedTask.Wait();
            s.Dispose();
        }

        public decimal GetValue()
        {
            throw new NotSupportedException();
        }

        public void Open()
        {
            if (IsConnected == true)
                return;

            // ConnectionString {{{
            var map = new Dictionary<string, string>();
            var @params = ConnectionString.Split(';');
            foreach (var param in @params)
            {
                var temp = param.Split('=');
                if (temp.Length == 1 && string.IsNullOrWhiteSpace(temp[0]) == true)
                    continue;

                var key = temp[0].Trim().ToLower();
                var value = "";
                if (temp.Length > 1)
                    value = temp[1].Trim();

                map[key] = value;
            }
            // }}}

            var comPort = map["port"];
            s = new SerialPort(comPort, 57600, Parity.None, 8, StopBits.One);
            s.RtsEnable = true;
            s.NewLine = "\r";
            s.ReadTimeout = 2000;
            s.Open();
            s.DiscardInBuffer();
            s.DiscardOutBuffer();

            IsConnected = true;

            cts = new CancellationTokenSource();
            receivedTask = Task.Run(async () =>
            {
                while (cts.IsCancellationRequested == false)
                {
                    await Task.Yield();

                    try
                    {
                        var result = s.ReadLine();
                        if (result.StartsWith("DT") == false)
                            continue;

                        var id = result[2..7];
                        var value = result[7..^1];
                        var unit = result[^1..];

                        try
                        {
                            PressedButtonEvent?.Invoke(this, new PressedButtonEventArgs(int.Parse(id), decimal.Parse(value), unit == "M" ? UWaveUnit.mm : UWaveUnit.None));
                        }
                        catch
                        {
                        }
                    }
                    catch (TimeoutException)
                    {
                        continue;
                    }
                    catch (Exception)
                    {
                        await Task.Delay(500);
                    }
                }
            }, cts.Token);
        }
    }
}
