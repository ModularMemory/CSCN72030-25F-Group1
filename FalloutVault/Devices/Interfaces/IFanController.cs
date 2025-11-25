namespace FalloutVault.Devices.Interfaces;

public interface IFanController : IOnOff
{
    int TargetRpm { get; }

    int SpeedRpm { get; }
}
