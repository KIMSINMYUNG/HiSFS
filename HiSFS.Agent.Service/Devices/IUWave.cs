using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Agent.Service.Devices
{
    public interface IUWave : IDevice
    {
        event EventHandler<PressedButtonEventArgs> PressedButtonEvent;

        decimal GetValue();

        bool IsPressedButtonEventSupported { get; }
        bool IsGetValueSupported { get; }
    }

    public class PressedButtonEventArgs : EventArgs
    {
        public int Id { get; }
        public decimal Value { get; }
        public UWaveUnit Unit { get; }

        public PressedButtonEventArgs(int Id, decimal value, UWaveUnit unit)
        {
            this.Id = Id;
            this.Value = value;
            this.Unit = unit;
        }
    }

    public enum UWaveUnit
    {
        None,
        mm
    }
}
