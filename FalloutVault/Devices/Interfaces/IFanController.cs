namespace FalloutVault.Devices.Interfaces;

public interface IFanController
{
    bool IsOn { get; set; }

    double TargetRpm { get; set; }

    double SpeedRpm { get; }
}
