namespace FalloutVault.Devices.Interfaces;

public interface IFanController : IOnOff
{
    int TargetRpm { get; }

    double SpeedRpm { get; }
}
