using System.ComponentModel.DataAnnotations;
using FalloutVault.Models;

namespace FalloutVault.Devices.Interfaces;

public interface IFanController : IOnOff
{
    Watt MotorWattage { get; }

    [Range(0, int.MaxValue)]
    int MaxRpm { get; }

    [Range(0, int.MaxValue)]
    int TargetRpm { get; }

    double SpeedRpm { get; }
}
