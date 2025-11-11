namespace FalloutVault.Devices.Interfaces;

public interface IFanController : IOnOff
{
    double TargetRpm { get; set; }

    double SpeedRpm { get; }
}
