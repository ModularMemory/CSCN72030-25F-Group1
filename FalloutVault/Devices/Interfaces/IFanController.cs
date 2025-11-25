namespace FalloutVault.Devices.Interfaces;

public interface IFanController : IOnOff
{
    double TargetRpm { get; }

    double SpeedRpm { get; }
}
